using Application.Common.Interfaces;
using Application.CQRS.CategoryTranslations.Commands.CreateCategoryTranslation;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.CategoryTranslations.Commands.UpdateCategoryTranslation;

public record UpdateCategoryTranslationCommand : IRequest<string>
{
    public string Title { get; set; }
    public int LanguageId { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateCategoryTranslationCommandHandler : IRequestHandler<UpdateCategoryTranslationCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdateCategoryTranslationCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<string> Handle(UpdateCategoryTranslationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CategoryTranslations.FirstOrDefaultAsync(
            ct => ct.LanguageId == request.LanguageId && ct.CategoryId == request.CategoryId
        , cancellationToken);

        string message;

        if(entity == null)
        {
            CreateCategoryTranslationCommand createCategoryTranslationCommand = new CreateCategoryTranslationCommand()
            {
                Title = request.Title,
                CategoryId = request.CategoryId,
                LanguageId = request.LanguageId
            };

            int id = await _mediator.Send(createCategoryTranslationCommand);
            message = $"Translation with Id = {id} was successfully created";
            return message;
        }

        else 
        {
            entity.Title = request.Title;
            await _context.SaveChangesAsync(cancellationToken);
            message = $"Translation with Id = {entity.Id} was successfully updated";
            return message;
        }

    }
}

