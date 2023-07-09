using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Hashtags.Commands.UpdateHashtag;

public record UpdateHashtagCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Title { get; set; }
}

public class UpdateHashtagCommandHandler : IRequestHandler<UpdateHashtagCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateHashtagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateHashtagCommand request,  CancellationToken cancellationToken)
    {
        var entity = await _context.Hashtags.FindAsync(new object[] {request.Id}, cancellationToken);

        if(entity == null)
        {
            throw new NotFoundException(nameof(Hashtag), request.Id);
        }

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
