using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.CategoryTranslations.Commands.DeleteCategoryTranslation;

public record DeleteCategoryTranslationCommand(int Id) : IRequest<Unit>;

public class DeleteCategoryTranslationCommandHandler : IRequestHandler<DeleteCategoryTranslationCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryTranslationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCategoryTranslationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CategoryTranslations.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CategoryTranslation), request.Id);
        }

        _context.CategoryTranslations.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

