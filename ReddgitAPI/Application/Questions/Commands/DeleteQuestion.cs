using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Entities;
using ReddgitAPI.ORM.Services;
using System.Security.Claims;

namespace ReddgitAPI.Application.Questions.Commands
{
    public class DeleteQuestion : IRequestHandler<DeleteQuestion.Command, QuestionDto>
    {
        public class Command : IRequest<QuestionDto>
        {
            public string Id { get; set; }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteQuestion(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<QuestionDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var question = await _dbContext.Questions.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(x => x.Id == request.Id);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in token.");
            }

            if (question.UserId != userId)
            {
                throw new Exception("You are not authorized to update this question.");
            }

            question.Deleted = true;

            _dbContext.Update(question);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var questionDto = _mapper.Map<QuestionDto>(question);

            return questionDto;
        }
    }
}
