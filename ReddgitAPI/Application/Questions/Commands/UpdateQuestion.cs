using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Entities;
using ReddgitAPI.ORM.Services;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace ReddgitAPI.Application.Questions.Commands
{
    public class UpdateQuestion : IRequestHandler<UpdateQuestion.Command, QuestionDetailDto>
    {
        public class Command : IRequest<QuestionDetailDto>
        {
            public string QuestionId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.QuestionId).NotEmpty();
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                    RuleFor(x => x.Content).NotEmpty().MaximumLength(2048);
                }
            }
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

        public async Task<QuestionDetailDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in token.");
            }

            var question = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);

            if (question == null)
            {
                throw new Exception($"Question with ID {request.QuestionId} not found.");
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

            var questionDto = _mapper.Map<QuestionDetailDto>(question);


            return questionDto;
        }
    }
}
