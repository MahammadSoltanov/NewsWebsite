using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Posts.Commands.DeletePost;

public record DeletePostCommand(int Id) : IRequest<Unit>;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
{
    IApplicationDbContext _context;

    public DeletePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts.FindAsync(new object[] { request.Id });

        if (entity == null) 
        {
            throw new NotFoundException(nameof(Post), request.Id);
        }

        var translations = _context.PostTranslations.Where(pt => pt.PostId == request.Id).ToList();

        if (translations.Any()) 
        {
            foreach(var translation in translations) 
            {
                _context.PostTranslations.Remove(translation);
            }
        }

        _context.Posts.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
