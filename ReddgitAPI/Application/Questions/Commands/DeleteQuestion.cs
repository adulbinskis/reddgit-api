using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Entities;
using ReddgitAPI.ORM.Services;
using System.Security.Claims;

namespace ReddgitAPI.Application.Questions.Commands
{
    public class DeleteQuestion : IRequestHandler<DeleteQuestion.Command, QuestionDetailDto>
    {
        public class Command : IRequest<QuestionDetailDto>
        {
            public string QuestionId { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.QuestionId).NotEmpty();
                }
            }
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

        public async Task<QuestionDetailDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var question = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);

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

            var questionDto = _mapper.Map<QuestionDetailDto>(question);

            return questionDto;
        }
    }
}
