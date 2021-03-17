using AutoMapper;
using WSUserAccountManager.Database.Entities;
using WSUserAccountManager.Helpers;
using WSUserAccountManager.Models;
using WSUserAccountManager.Models.Messages;

namespace WSUserAccountManager.MapperProfiles
{
    public class UserAccountProfile : Profile
    {
        public UserAccountProfile()
        {
            CreateMap<Models.UserAccount, Database.Entities.UserAccount>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName))
             .ForSourceMember(src => src.VerificationCode, opt => opt.DoNotValidate())
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => RandomGenerator.Id(11)));

            CreateMap<Register, Models.UserAccount>();
        }
    }
}
;