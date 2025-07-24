using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Hashtags.Commands.CreateHashtag;

public record CreateHashtagCommand : IRequest<int>
{
    public string Title { get; set; }
}

public class CreateHashtagCommandHandler : IRequestHandler<CreateHashtagCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateHashtagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateHashtagCommand request, CancellationToken cancellationToken)
    {
        var entity = new Hashtag()
        {
            Title = request.Title,
        };

        _context.Hashtags.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}