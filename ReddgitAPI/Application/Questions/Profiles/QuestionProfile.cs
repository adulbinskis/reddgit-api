using ReddgitAPI.Application.Answers.Models;
using ReddgitAPI.Application.Questions.Models;
using ReddgitAPI.ORM.Entities;

namespace ReddgitAPI.Application.Questions.Profiles
{
    public class QuestionProfile : AutoMapper.Profile
    {
        public QuestionProfile() 
        {
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName));

            CreateMap<QuestionDto, Question>();

            CreateMap<Question, QuestionDetailDto>()
                  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                  .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<Answer, AnswerDetailDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName));
        }
    }
}
