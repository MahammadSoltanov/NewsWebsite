using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IApplicationDbContext _context;    
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public CreateUserCommandHandler(IApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Name = request.Name,
            Surname = request.Surname,
            UserName = request.Email,
            Password = request.Password,
            RoleId = request.RoleId,
            SecurityStamp = Guid.NewGuid().ToString() // Set a new security stamp value
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role != null)
            {
                var resultRole = await _userManager.AddToRoleAsync(user, role.Name);
                if (!resultRole.Succeeded)
                {
                    // Handle role assignment error
                }
            }

            else
            {
                throw new NotFoundException(nameof(Role), request.RoleName);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Convert.ToInt32(user.Id);
        }

        else
        {
            throw new Exception(result.Errors.ToString());
        }
    }
}