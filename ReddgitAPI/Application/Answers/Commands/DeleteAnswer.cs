﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Answers.Models;
using ReddgitAPI.ORM.Services;
using System.Security.Claims;

namespace ReddgitAPI.Application.Answers.Commands
{
    public class DeleteAnswer : IRequestHandler<DeleteAnswer.Command, AnswerDetailDto>
    {
        public class Command : IRequest<AnswerDetailDto>
        {
            public string AnswerId { get; set; }
            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.AnswerId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteAnswer(ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AnswerDetailDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var answer = await _dbContext.Answers.FirstOrDefaultAsync(x => x.Id == request.AnswerId);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in token.");
            }

            if (answer.UserId != userId)
            {
                throw new Exception("You are not authorized to update this answer.");
            }

            answer.Deleted = true;

            _dbContext.Update(answer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var answerDto = _mapper.Map<AnswerDetailDto>(answer);

            return answerDto;
        }
    }
}
