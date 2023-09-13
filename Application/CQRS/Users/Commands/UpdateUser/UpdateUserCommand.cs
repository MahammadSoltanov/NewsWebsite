using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

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
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public UpdateUserCommandHandler(IApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _userManager.FindByIdAsync((request.Id).ToString());

        if(entity == null) 
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        entity.Name = request.Name;
        entity.Surname = request.Surname;
        entity.Email = request.Email;
        

        if (entity.Password != request.Password)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(entity);
            var resultPassword = await _userManager.ResetPasswordAsync(entity, token, request.Password);
            
                if (!resultPassword.Succeeded)
            {
                var errorMessage = string.Join(", ", resultPassword.Errors.Select(error => error.Description));
                throw new ApplicationException($"Password update failed: {errorMessage}");
            }

            entity.Password = request.Password;
        }
        if (request.RoleId != entity.RoleId)
        {
            var currentRoles = await _userManager.GetRolesAsync(entity);
            await _userManager.RemoveFromRolesAsync(entity, currentRoles);

            var role = await _roleManager.FindByIdAsync((request.RoleId).ToString());
            if (role != null)
            {
                await _userManager.AddToRoleAsync(entity, role.Name);
                entity.RoleId = role.Id;
            }
            else
            {
                throw new NotFoundException(nameof(Role), request.RoleId);
            }
        }

        var result = await _userManager.UpdateAsync(entity);

        if(!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(error => error.Description));
            throw new ApplicationException($"User update failed: {errorMessage}");

        }

        return Unit.Value;
    }
}

