using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Images.Commands.CreateImage;

public record CreateImageCommand : IRequest<int>
{
    public string Url { get; set; }
}

public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateImageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateImageCommand request, CancellationToken cancellationToken)
    {
        var entity = new ImageModel { Url = request.Url };

        _context.Images.Add(entity);
        
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

}
