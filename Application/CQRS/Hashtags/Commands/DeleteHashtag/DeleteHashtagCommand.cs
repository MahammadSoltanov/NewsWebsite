using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Hashtags;
using MediatR;

namespace Application.CQRS.Hashtags.Commands.DeleteHashtag;

public record DeleteHashtagCommand(int Id) : IRequest<Unit>;

public class DeleteHashtagCommandHandler : IRequestHandler<DeleteHashtagCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteHashtagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteHashtagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Hashtags.FindAsync(new object[] {request.Id}, cancellationToken);

        if(entity == null)
        {
            throw new NotFoundException(nameof(Hashtag), request.Id);
        }

        _context.Hashtags.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

