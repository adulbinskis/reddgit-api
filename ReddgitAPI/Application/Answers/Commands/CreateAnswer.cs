﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReddgitAPI.Application.Answers.Models;
using ReddgitAPI.ORM.Entities;
using ReddgitAPI.ORM.Services;
using System.Security.Claims;

namespace ReddgitAPI.Application.Answers.Commands
{
    public class CreateAnswer : IRequestHandler<CreateAnswer.Command, AnswerDetailDto>
    {
        public class Command : IRequest<AnswerDetailDto>
        {
            public string Content { get; set; }

            public string QuestionId { get; set; }
            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.QuestionId).NotEmpty();
                    RuleFor(x => x.Content).NotEmpty().MaximumLength(2048).MinimumLength(10);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IBaseEntityService _baseEntityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateAnswer(ApplicationDbContext dbContext, IMapper mapper, IBaseEntityService baseEntityService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _baseEntityService = baseEntityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AnswerDetailDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var question = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);

            if (string.IsNullOrEmpty(question.Id))
            {
                throw new Exception("Question not found");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in token.");
            }

            var answer = new Answer
            {
                Id = Guid.NewGuid().ToString(),
                Content = request.Content,
                UserId = userId,
                QuestionId = request.QuestionId
            };

            await _baseEntityService.SetCreatedPropertiesAsync(answer);

            var resp = _dbContext.Answers.Add(answer);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            var answerDto = _mapper.Map<AnswerDetailDto>(answer);

            return answerDto;
        }
    }
}
