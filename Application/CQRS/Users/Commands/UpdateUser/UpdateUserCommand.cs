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

    public UpdateUserCommandHandler(IApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
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
        entity.RoleId = request.RoleId;
        entity.Password = request.Password != null ? request.Password : entity.Password;    

        var result = await _userManager.UpdateAsync(entity);

        if(!result.Succeeded)
        {

        }

        return Unit.Value;
    }
}

