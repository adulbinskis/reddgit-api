using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReddgitAPI.Application.Answers.Commands;
using ReddgitAPI.Application.Answers.Models;

namespace ReddgitAPI.Application._Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : MediatRController
    {
        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<AnswerDetailDto>> CreateAnswer([FromBody] CreateAnswer.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<AnswerDetailDto>> UpdateAnswer([FromBody] UpdateAnswer.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<AnswerDetailDto>> DeleteAnswer([FromBody] DeleteAnswer.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}
