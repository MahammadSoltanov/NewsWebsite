using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Commands.ChangePostTranslationsStatuses;

public record ChangePostTranslationsStatusesCommand : IRequest<Unit>
{
    public List<PostTranslationStatusObj> ChangedPosts { get; set; }
}

public class ChangePostTranslationsStatusesCommandHandler : IRequestHandler<ChangePostTranslationsStatusesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ChangePostTranslationsStatusesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangePostTranslationsStatusesCommand request, CancellationToken cancellationToken)
    {
        var posts = await _context.PostTranslations.ToListAsync(cancellationToken);

        foreach (var changedPost in request.ChangedPosts)
        {
            foreach (var post in posts)
            {
                if (post.Id == changedPost.Id)
                {
                    post.Status = changedPost.Status;
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

