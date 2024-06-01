using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Services;

namespace ReddgitAPI.Application.Questions.Queries
{
    public class GetQuestionsListByName : IRequestHandler<GetQuestionsListByName.Query, List<QuestionDto>>
    {
        public class Query : IRequest<List<QuestionDto>>
        { 
            public string Title { get; set; }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestionsListByName(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<QuestionDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Questions.AsQueryable();

            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query
                    .Where(x => x.Deleted == false)
                    .Where(q => q.Title.Contains(request.Title))
                    .OrderByDescending(x => x.CreatedAt);
            }

            var questions = await query.ToListAsync(cancellationToken);

            var questionsDto = _mapper.Map<List<QuestionDto>>(questions);

            return questionsDto;
        }
    }
}
