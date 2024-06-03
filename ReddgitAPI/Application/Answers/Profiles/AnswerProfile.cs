using ReddgitAPI.Application.Answers.Models;
using ReddgitAPI.ORM.Entities;

namespace ReddgitAPI.Application.Answers.Profiles
{
    public class AnswerProfile : AutoMapper.Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, AnswerDetailDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName));
        }
    }
}
