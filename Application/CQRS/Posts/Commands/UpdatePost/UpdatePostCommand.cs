using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Posts.Commands.UpdatePost;

public record UpdatePostCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string TitleImageUrl { get; set; }
    public int CategoryId { get; set; }

}

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);

        if(entity == null)
        {
            throw new NotFoundException(nameof(Post), request.Id);
        }

        entity.TitleImageUrl = request.TitleImageUrl;
        entity.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

