using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Categories;
using MediatR;

namespace Application.CQRS.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<Unit>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }

        var translations = _context.CategoryTranslations.Where(ct => ct.Id == request.Id).ToList();

        if (translations.Any())
        {
            foreach (var translation in translations)
            {
                _context.CategoryTranslations.Remove(translation);
            }
        }

        _context.Categories.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

