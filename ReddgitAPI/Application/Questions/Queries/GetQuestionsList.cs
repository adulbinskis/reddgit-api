using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Entities;
using ReddgitAPI.ORM.Services;

namespace ReddgitAPI.Application.Questions.Queries
{
    public class GetQuestionsList : IRequestHandler<GetQuestionsList.Query, PaginatedQuestionsDto>
    {
        public class Query : IRequest<PaginatedQuestionsDto>
        { 
            public string SearchCriteria { get; set; }
            public int Page { get; set; }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestionsList(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedQuestionsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            IQueryable<Question> query = _dbContext.Questions
                .Where(x => !x.Deleted)
                .OrderByDescending(x => x.CreatedAt);

            if (!string.IsNullOrWhiteSpace(request.SearchCriteria))
            {
                string searchLower = request.SearchCriteria.ToLower();
                query = query.Where(q =>
                    q.Title.ToLower().Contains(searchLower));
            }

            int pageSize = 8;
            int offset = (request.Page - 1) * pageSize;

            var questions = await query
                .Skip(offset)
                .Take(pageSize)
                .Select(q => new QuestionDetailDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Content = q.Content,
                    UserName = q.ApplicationUser.UserName,
                    CreatedAt = q.CreatedAt,
                    UserId = q.UserId,
                })
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<QuestionDetailDto>>(questions);

            int totalQuestionsCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalQuestionsCount / pageSize);

            return new PaginatedQuestionsDto
            {
                Questions = dtos,
                TotalPages = totalPages
            };
        }
    }
}
