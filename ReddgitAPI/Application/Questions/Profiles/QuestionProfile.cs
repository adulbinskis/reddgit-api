using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Entities;

namespace ReddgitAPI.Application.Questions.Profiles
{
    public class QuestionProfile : AutoMapper.Profile
    {
        public QuestionProfile() 
        {
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionDto, Question>();
        }
    }
}
