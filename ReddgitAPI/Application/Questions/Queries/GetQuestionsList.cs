using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
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
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.CreatedAt)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Content = q.Content,
                    UserName = q.ApplicationUser.UserName,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<QuestionDto>>(questions);
            return dtos;
        }
    }
}
