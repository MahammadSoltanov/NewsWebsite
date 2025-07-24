using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Languages.Commands.DeleteLanguage;

public record DeleteLanguageCommand(int Id) : IRequest<Unit>;

public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public DeleteLanguageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Languages
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Language), request.Id);
        }

        _context.Languages.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


