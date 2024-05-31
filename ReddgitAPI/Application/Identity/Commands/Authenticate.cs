using MediatR;
using Microsoft.AspNetCore.Identity;
using ReddgitAPI.Application.Identity.Services;
using ReddgitAPI.ORM.Entities;
using ReddgitAPI.ORM.Services;
using ReddgitAPI.Application.Identity.Models;

namespace ReddgitAPI.Application.Identity.Commands
{
    public class Authenticate : IRequestHandler<Authenticate.Command, AuthResponse>
    {
        public class Command : IRequest<AuthResponse>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public Authenticate(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null || !await _userManager.CheckPasswordAsync(managedUser, request.Password))
            {
                return null;
            }

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (userInDb == null)
            {
                return null;
            }

            var accessToken = _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                UserId = userInDb.Id,
                Email = userInDb.Email,
                Token = accessToken
            };
        }
    }
}
