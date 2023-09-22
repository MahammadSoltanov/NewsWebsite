using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Application.CQRS.Posts.Commands.ChangePostsStatuses;

public record ChangePostsStatusesCommand : IRequest<Unit>
{
    public List<PostStatusObj> ChangedPosts { get; set; }
}

public class ChangePostsStatusesCommandHandler : IRequestHandler<ChangePostsStatusesCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private DefaultContainer _defaults;

    public ChangePostsStatusesCommandHandler(IApplicationDbContext context, IMediator mediator, IMapper mapper)
    {
        _context = context;
        _mediator = mediator;
        _mapper = mapper;
        _defaults = new DefaultContainer(_mediator);
    }

    public async Task<Unit> Handle(ChangePostsStatusesCommand request, CancellationToken cancellationToken)
    {
        var posts = await _context.Posts.ToListAsync(cancellationToken);

        foreach (var changedPost in request.ChangedPosts)
        {
            foreach (var post in posts)
            {
                if (post.Id == changedPost.Id)
                {
                    if (changedPost.Status == "Deleted")
                    {
                        await HandleDeletion(changedPost.Id, cancellationToken);
                    }

                    if (changedPost.Status == "Published")
                    {
                        post.PublishDate = DateTime.Now;
                        await HandlePublishment(changedPost.Id, cancellationToken);
                    }

                    else
                    {
                        await HandleDefault(changedPost.Id, changedPost.Status, cancellationToken);
                    }

                    post.Status = changedPost.Status;
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    public async Task HandleDeletion(int postId, CancellationToken cancellationToken)
    {
        var translations = await _context.PostTranslations
                                .Where(pt => pt.PostId == postId)
                                .ToListAsync(cancellationToken);

        int defaultLanguageId = await _defaults.GetDefaultPostLanguageId();

        if (translations != null)
        {
            foreach (var translation in translations)
            {
                translation.Status = "Deleted";
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task HandlePublishment(int postId, CancellationToken cancellationToken)
    {
        int defaultLanguageId = await _defaults.GetDefaultPostLanguageId();
        var translations = await _context.PostTranslations
                                 .Where(pt => pt.LanguageId == defaultLanguageId)
                                 .ToListAsync(cancellationToken);

        if (translations != null)
        {
            var translation = translations.FirstOrDefault(pt => pt.PostId == postId);

            if (translation != null)
            {
                translation.PublishDate = DateTime.Now;
                translation.Status = "Published";
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task HandleDefault(int postId, string newStatus, CancellationToken cancellationToken)
    {
        int defaultLanguageId = await _defaults.GetDefaultPostLanguageId();
        var translations = await _context.PostTranslations
                                 .Where(pt => pt.LanguageId == defaultLanguageId)
                                 .ToListAsync(cancellationToken);

        if (translations != null)
        {
            var translation = translations.FirstOrDefault(pt => pt.PostId == postId);
            
            if(translation != null)
            {
                translation.Status = newStatus;
            }
        }
    }
}