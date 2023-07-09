using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Languages;
using MediatR;

namespace Application.CQRS.Languages.Commands.CreateLanguage;

public record CreateLanguageCommand : IRequest<int>
{
    public string Code { get; init; }
    public string Title { get; init; }
}

public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateLanguageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = new Language
        {
            Code = request.Code,
            Title = request.Title,
        };

        entity.AddDomainEvent(new LanguageCreatedEvent(entity));
        
        _context.Languages.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
        
        return entity.Id;
    }
}

