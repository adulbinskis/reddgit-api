using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Answers.Models;
using ReddgitAPI.ORM.Services;

namespace ReddgitAPI.Application.Answers.Queries
{
    public class GetQuestionAnswers : IRequestHandler<GetQuestionAnswers.Query, List<AnswerDetailDto>>
    {
        public class Query : IRequest<List<AnswerDetailDto>>
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

        public GetQuestionAnswers(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<AnswerDetailDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var answers = await _dbContext.Answers
                .Where(x => x.Deleted == false)
                .Where(x => x.QuestionId == request.QuestionId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            var answerDtos = _mapper.Map<List<AnswerDetailDto>>(answers);
            return answerDtos;
        }
    }
}