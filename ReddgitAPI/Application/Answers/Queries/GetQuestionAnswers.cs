using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Answers.Models;
using ReddgitAPI.ORM.Services;

namespace ReddgitAPI.Application.Answers.Queries
{
    public class GetQuestionAnswers : IRequestHandler<GetQuestionAnswers.Query, List<AnswerDto>>
    {
        public class Query : IRequest<List<AnswerDto>>
        {
            public string QuestionId { get; set; }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestionAnswers(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<AnswerDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var answers = await _dbContext.Answers
                .Where(x => x.Deleted == false)
                .Where(x => x.QuestionId == request.QuestionId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            var answerDtos = _mapper.Map<List<AnswerDto>>(answers);
            return answerDtos;
        }
    }
}
