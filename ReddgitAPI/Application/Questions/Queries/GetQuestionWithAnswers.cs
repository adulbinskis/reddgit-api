using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Services;

namespace ReddgitAPI.Application.Questions.Queries
{
    public class GetQuestionWithAnswers : IRequestHandler<GetQuestionWithAnswers.Query, QuestionDetailDto>
    {
        public class Query : IRequest<QuestionDetailDto>
        { 
            public string QuestionId { get; set; }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestionWithAnswers(ApplicationDbContext dbContext, IMapper mapper)
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
                .Include(q => q.Answers.OrderByDescending(a => a.CreatedAt).Where(q => q.Deleted == false))
                    .ThenInclude(a => a.ApplicationUser)
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
