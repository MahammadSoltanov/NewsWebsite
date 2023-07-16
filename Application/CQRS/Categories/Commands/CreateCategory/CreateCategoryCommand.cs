using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Categories;
using MediatR;

namespace Application.CQRS.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<int>
{
    public string Description { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category()
        {
            Description = request.Description
        };

        entity.AddDomainEvent(new CategoryCreatedEvent(entity));

        _context.Categories.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

