using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.PostTranslations.Commands.IncreaseViewCount;

public record IncreaseViewCountCommand(int Id) : IRequest<Unit>;

public class IncreaseViewCountCommandHandler : IRequestHandler<IncreaseViewCountCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public IncreaseViewCountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(IncreaseViewCountCommand request, CancellationToken cancellationToken)
    {
        var postTranslation = await _context.PostTranslations.FindAsync(new object[] {request.Id});

        if (postTranslation != null) 
        {
            postTranslation.ViewCount++;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

