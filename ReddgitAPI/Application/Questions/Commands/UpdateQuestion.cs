using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Entities;
using ReddgitAPI.ORM.Services;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace ReddgitAPI.Application.Questions.Commands
{
    public class UpdateQuestion : IRequestHandler<UpdateQuestion.Command, QuestionDto>
    {
        public class Command : IRequest<QuestionDto>
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IBaseEntityService _baseEntityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateQuestion(ApplicationDbContext dbContext, IMapper mapper, IBaseEntityService baseEntityService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _baseEntityService = baseEntityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<QuestionDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in token.");
            }

            var question = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (question == null)
            {
                throw new Exception($"Question with ID {request.Id} not found.");
            }

            if (question.UserId != userId)
            {
                throw new Exception("You are not authorized to update this question.");
            }

            question.Title = request.Title;
            question.Content = request.Content;

            await _baseEntityService.SetUpdatedPropertiesAsync(question);

            _dbContext.Questions.Update(question);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var questionDto = _mapper.Map<QuestionDto>(question);


            return questionDto;
        }
    }
}
