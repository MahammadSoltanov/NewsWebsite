using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Posts.Commands.ChangePostsStatuses;

public record ChangePostsStatusesCommand : IRequest<Unit>
{
    public List<PostStatusObj> ChangedPosts { get; set; }
}

public class ChangePostsStatusesCommandHandler : IRequestHandler<ChangePostsStatusesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ChangePostsStatusesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangePostsStatusesCommand request, CancellationToken cancellationToken)
    {
        var posts = await _context.PostTranslations.ToListAsync(cancellationToken);

        foreach (var changedPost in request.ChangedPosts)
        {
            foreach (var post in posts)
            {
                if(post.Id == changedPost.Id)
                {
                    post.Status = changedPost.Status;
                }
            }
        }
        Console.WriteLine(posts);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

