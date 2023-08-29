using Application.Common.Interfaces;
using Application.CQRS.Hashtags.Commands.CreateHashtag;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostHashtags.Commands.UpdatePostHashtags;

public record UpdatePostHashtagsCommand : IRequest<Unit>
{
    public List<string> Tags { get; set; }
    public int PostId { get; set; }
}

public class UpdatePostHashtagsCommandHandler : IRequestHandler<UpdatePostHashtagsCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdatePostHashtagsCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdatePostHashtagsCommand request, CancellationToken cancellationToken)
    {
        //This code could be added to AddPostHashtagsCommand and it would work for updation and creation simultaneously 
        //but we want to have separation for possible changes in the futures
        
        foreach(var postHashtag in _context.PostHashtags) 
        {
            if(postHashtag.PostId == request.PostId) 
            {
                _context.PostHashtags.Remove(postHashtag);
            }
        }
                
        var allHashtags = await _context.Hashtags.ToListAsync(cancellationToken);

        foreach (var tag in request.Tags)
        {
            int hashtagId = 0;
            bool tagExistsInDb = false;

            foreach (var hashtag in allHashtags)
            {
                if (tag == hashtag.Title)
                {
                    tagExistsInDb = true;
                    hashtagId = hashtag.Id;
                }
            }

            if (!tagExistsInDb)
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