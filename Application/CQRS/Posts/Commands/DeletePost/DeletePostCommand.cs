using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Posts.Commands.DeletePost;

public record DeletePostCommand(int Id) : IRequest<Unit>;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
{
    private readonly IApplicationDbContext _context;    
    private DefaultContainer defaults;

    public DeletePostCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        defaults = new DefaultContainer(mediator);
    }

    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts.FindAsync(new object[] { request.Id });        
        int defaultLanguageId = await defaults.GetDefaultPostLanguageId();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Post), request.Id);
        }

        var translations = _context.PostTranslations.Where(pt => pt.PostId == request.Id).ToList();

        if (translations.Any())
        {
            foreach (var translation in translations)
            {
                translation.Status = "Deleted";
                _context.Entry(translation).State = EntityState.Modified; // Mark the entity as modified
            }
        }

        entity.Status = "Deleted";

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
