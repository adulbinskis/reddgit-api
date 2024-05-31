using MediatR;
using Microsoft.AspNetCore.Identity;
using ReddgitAPI.Application.Identity.Models;
using ReddgitAPI.Application.Identity.Roles;
using ReddgitAPI.ORM.Entities;

namespace ReddgitAPI.Application.Identity.Commands
{
    public class Register : IRequestHandler<Register.Command, RegistrationResponse>
    {
        public class Command : IRequest<RegistrationResponse>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private readonly UserManager<ApplicationUser> _userManager;

        public Register(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegistrationResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _userManager.CreateAsync(
                new ApplicationUser { UserName = request.Username, Email = request.Email, Role = Role.User },
                request.Password
            );

            if (result.Succeeded)
            {
                return new RegistrationResponse
                {
                    Username = request.Username,
                    Email = request.Email
                };
            }

            return null;
        }
    }
}
