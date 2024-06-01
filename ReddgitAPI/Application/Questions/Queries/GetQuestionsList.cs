using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Services;

namespace ReddgitAPI.Application.Questions.Queries
{
    public class GetQuestionsList : IRequestHandler<GetQuestionsList.Query, List<QuestionDto>>
    {
        public class Query : IRequest<List<QuestionDto>>
        { }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestionsList(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<QuestionDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var questions = await _dbContext.Questions
                .AsNoTracking()
                .Where(x => x.Deleted == false)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<QuestionDto>>(questions);
            return dtos;
        }
    }
}
