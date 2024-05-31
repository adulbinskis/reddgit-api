using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReddgitAPI.Application.Identity.Commands;
using ReddgitAPI.Application.Identity.Models;
using ReddgitAPI.Application.Identity.Queries;

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

            return Ok(response);
        }


        [HttpGet("checkAuth")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string>> CheckAuth()
        {
            var response = await Mediator.Send(new CheckAuth.Query());

            return response;
        }
    }
}
