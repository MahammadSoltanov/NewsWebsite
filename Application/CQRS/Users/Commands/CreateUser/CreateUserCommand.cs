using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Users;
using MediatR;

namespace Application.CQRS.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User()
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            RoleId = request.RoleId
        };

        _context.Users.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Convert.ToInt32(entity.Id); 
    }
}