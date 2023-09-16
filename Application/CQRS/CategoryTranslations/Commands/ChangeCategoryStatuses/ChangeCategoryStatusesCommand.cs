using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.CategoryTranslations.Commands.ChangeCategoryStatuses;

public record ChangeCategoryTranslationStatusesCommand : IRequest<Unit>
{
    public List<CategoryTranslationStatusObj> ChangedCategories { get; set; }
}

public class ChangeCategoryTranslationStatusesCommandHandler : IRequestHandler<ChangeCategoryTranslationStatusesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ChangeCategoryTranslationStatusesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeCategoryTranslationStatusesCommand request, CancellationToken cancellationToken)
    {
        var categories = await _context.CategoryTranslations.ToListAsync(cancellationToken);

        foreach (var changedCategory in request.ChangedCategories)
        {
            foreach (var category in categories)
            {
                if (category.Id == changedCategory.Id)
                {
                    category.Status = changedCategory.Status;
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

