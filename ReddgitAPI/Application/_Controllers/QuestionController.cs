using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReddgitAPI.Application.Questions.Commands;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.Application.Questions.Queries;

namespace ReddgitAPI.Application._Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : MediatRController
    {
        [HttpGet("[action]")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestionsListByName([FromQuery] GetQuestionsListByName.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestionsList()
        {
            return await Mediator.Send(new GetQuestionsList.Query());
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestion.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<QuestionDto>> UpdateQuestion([FromBody] UpdateQuestion.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<QuestionDto>> DeleteQuestion([FromBody] DeleteQuestion.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}
