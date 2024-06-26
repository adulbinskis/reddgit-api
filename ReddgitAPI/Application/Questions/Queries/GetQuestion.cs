﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Services;

namespace ReddgitAPI.Application.Questions.Queries
{
    public class GetQuestion : IRequestHandler<GetQuestion.Query, QuestionDetailDto>
    {
        public class Query : IRequest<QuestionDetailDto>
        { 
            public string QuestionId { get; set; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.QuestionId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestion(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<QuestionDetailDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var question = await _dbContext.Questions
                .OrderByDescending(x => x.CreatedAt)
                .Where(q => q.Id == request.QuestionId)
                .Include(q => q.ApplicationUser)
                .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
            {
                throw new Exception("Question not found");
            }

            var questionDetailDto = _mapper.Map<QuestionDetailDto>(question);
            return questionDetailDto;
        }
    }
}
