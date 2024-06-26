﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Entities;
using ReddgitAPI.ORM.Services;
using System.Security.Claims;
using System.Runtime.Serialization;
using FluentValidation;

namespace ReddgitAPI.Application.Questions.Commands
{
    public class CreateQuestion : IRequestHandler<CreateQuestion.Command, QuestionDetailDto>
    {
        public class Command : IRequest<QuestionDetailDto>
        {
            public string Title { get; set; }
            public string Content { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                    RuleFor(x => x.Content).NotEmpty().MaximumLength(2048);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IBaseEntityService _baseEntityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateQuestion(ApplicationDbContext dbContext, IMapper mapper, IBaseEntityService baseEntityService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _baseEntityService = baseEntityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<QuestionDetailDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in token.");
            }

            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Content = request.Content,
                UserId = userId
            };

            await _baseEntityService.SetCreatedPropertiesAsync(question);

            _dbContext.Questions.Add(question);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            var questionDto = _mapper.Map<QuestionDetailDto>(question);

            return questionDto;
        }
    }
}
