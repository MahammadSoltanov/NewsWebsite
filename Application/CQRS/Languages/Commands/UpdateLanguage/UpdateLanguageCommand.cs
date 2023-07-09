using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Languages.Commands.UpdateLanguage;

public record UpdateLanguageCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Code { get; init; }
}

public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateLanguageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Languages.FindAsync( new object[] { request.Id}, cancellationToken);
        
        if (entity == null) 
        {
            throw new NotFoundException(nameof(Language), request.Id);
        }

        entity.Title = request.Title;
        entity.Code = request.Code;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}