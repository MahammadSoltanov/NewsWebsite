using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Behaviours.SignIn;

public record SignInCommand : IRequest<SignInResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInResult>
{
    private readonly IApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;

    public SignInCommandHandler(IApplicationDbContext context, SignInManager<User> signInManager)
    {
        _context = context;
        _signInManager = signInManager;
    }

    public async Task<SignInResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if(user == null) 
        {
            throw new NotFoundException(nameof(User), request.Email);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
        }

        return result;
    }

}