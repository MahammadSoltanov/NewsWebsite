using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.CategoryTranslations;
using MediatR;

namespace Application.CQRS.CategoryTranslations.Commands.CreateCategoryTranslation;

public record CreateCategoryTranslationCommand : IRequest<int>
{
    public string Title { get; set; }
    public int LanguageId { get; set; }
    public int CategoryId { get; set; }
}

public class CreateCategoryTranslationCommandHandler : IRequestHandler<CreateCategoryTranslationCommand, int>
{    
    private readonly IApplicationDbContext _context;

    public CreateCategoryTranslationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCategoryTranslationCommand request,  CancellationToken cancellationToken)
    {
        var entity = new CategoryTranslation()
        { 
            Title = request.Title,
            LanguageId = request.LanguageId,
            CategoryId = request.CategoryId,
            InsertDate = DateTime.Now,
        };

        entity.AddDomainEvent(new CategoryTranslationCreatedEvent(entity));

        _context.CategoryTranslations.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
