using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Posts;
using MediatR;

namespace Application.CQRS.Posts.Commands.CreatePost;

public record CreatePostCommand : IRequest<int>
{
    public int CategoryId { get; set; }
    public string TitleImageUrl { get; set; }
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = new Post()
        {
            Status = "Pending",
            CategoryId = request.CategoryId,
            InsertDate = DateTime.Now,
            TitleImageUrl = request.TitleImageUrl,
        };

        entity.AddDomainEvent(new PostCreatedEvent(entity));

        _context.Posts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}