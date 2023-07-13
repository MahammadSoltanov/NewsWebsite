using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);

        if(entity == null) 
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        entity.Name = request.Name;
        entity.Surname = request.Surname;
        entity.Email = request.Email;   
        entity.Password = request.Password;   
        entity.RoleId = request.RoleId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

