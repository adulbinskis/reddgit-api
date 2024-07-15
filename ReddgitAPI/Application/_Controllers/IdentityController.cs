using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReddgitAPI.Application.Identity.Commands;
using ReddgitAPI.Application.Identity.Models;

namespace ReddgitAPI.Application._Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : MediatRController
    {
        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await Mediator.Send(new Register.Command
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            });

            if (response == null)
            {
                return BadRequest("Registration failed. Please check the provided details and try again.");
            }

            return CreatedAtAction(nameof(Register), new { email = response.Email }, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await Mediator.Send(new Authenticate.Command
            {
                Email = request.Email,
                Password = request.Password
            });

            if (response == null)
            {
                return BadRequest("Bad credentials");
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                // .Strict for deploy
                SameSite = SameSiteMode.None, 
                Expires = response.RefreshTokenExpirationDate,
            };

            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

            var authResponse = new AuthResponse
            {
                UserId = response.UserId,
                Email = response.Email,
                Token = response.Token,
                UserName = response.UserName
            };

            return Ok(authResponse);
        }


        [HttpGet("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var response = await Mediator.Send(new Refresh.Command
            { 
                RefreshToken = refreshToken
            });

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = response.RefreshTokenExpirationDate,
            };

            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

            var authResponse = new AuthResponse
            {
                UserId = response.UserId,
                Email = response.Email,
                Token = response.Token,
                UserName = response.UserName
            };

            return authResponse;
        }
    }
}
