using Application.Common.Interfaces;
using Application.CQRS.Hashtags.Commands.CreateHashtag;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostHashtags.Commands.AddPostHashtags;

public record AddPostHashtagsCommand : IRequest<Unit>
{
    public List<string> Tags { get; set; }
    public int PostId { get; set; }
}

public class AddPostHashtagsCommandHandler : IRequestHandler<AddPostHashtagsCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public AddPostHashtagsCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(AddPostHashtagsCommand request, CancellationToken cancellationToken)
    {
        var allHashtags = await _context.Hashtags.ToListAsync(cancellationToken);

        foreach(var tag in request.Tags)
        {
            int hashtagId = 0;
            bool tagExistsInDb = false;

            foreach(var hashtag in allHashtags)
            {
                if(tag == hashtag.Title)
                {
                    tagExistsInDb = true;
                    hashtagId = hashtag.Id;
                }
            }

            if(!tagExistsInDb) 
            {
                CreateHashtagCommand createHashtagCommand = new CreateHashtagCommand()
                {
                    Title = tag,
                };              
                
                hashtagId = await _mediator.Send(createHashtagCommand);

                await LinkPostWithHashtag(request.PostId, hashtagId, cancellationToken);
            }

            else
            {
                await LinkPostWithHashtag(request.PostId, hashtagId, cancellationToken);
            }
        }

        return Unit.Value;
    }

    private async Task LinkPostWithHashtag(int postId, int hashtagId, CancellationToken cancellationToken)
    {
        var entity = new PostHashtag()
        {
            HashtagId = hashtagId,
            PostId = postId
        };

        _context.PostHashtags.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

