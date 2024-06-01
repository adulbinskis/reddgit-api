﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReddgitAPI.Application.Answers.Models;
using ReddgitAPI.ORM.Services;
using System.Security.Claims;

namespace ReddgitAPI.Application.Answers.Commands
{
    public class UpdateAnswer : IRequestHandler<UpdateAnswer.Command, AnswerDto>
    {
        public class Command : IRequest<AnswerDto>
        {
            public string Id { get; set; }
            public string Content { get; set; }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IBaseEntityService _baseEntityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateAnswer(ApplicationDbContext dbContext, IMapper mapper, IBaseEntityService baseEntityService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _baseEntityService = baseEntityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AnswerDto> Handle(Command request, CancellationToken cancellationToken)
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var answer = await _dbContext.Answers.FirstOrDefaultAsync(x => x.Id == request.Id);


            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in token.");
            }

            if (answer.UserId != userId)
            {
                throw new Exception("You are not authorized to update this answer.");
            }

            answer.Content = request.Content;

            await _baseEntityService.SetUpdatedPropertiesAsync(answer);

            _dbContext.Answers.Update(answer);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var ansewrDto = _mapper.Map<AnswerDto>(answer);

            return ansewrDto;
        }
    }
}
