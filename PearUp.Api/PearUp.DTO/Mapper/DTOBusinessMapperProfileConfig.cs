using AutoMapper;
using PearUp.BusinessEntity;
using PearUp.DTO;

namespace PearUp.Utilities
{
    public class DTOBusinessMapperProfileConfig : Profile
    {
        public DTOBusinessMapperProfileConfig()
    : this("DTO_Business")
        {

        }
        protected DTOBusinessMapperProfileConfig(string profileName)
        : base(profileName)
        {
            CreateMap<UserRegistrationDTO, UserMatchPreference>()
                .ReverseMap();

            CreateMap<UserRegistrationDTO, UserPhoneNumber>()
                .ReverseMap();

            CreateMap<UserRegistrationDTO, User>()
                .ForMember(d => d.MatchPreference, opt => opt.MapFrom(src => src))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src))
                .ReverseMap();
            CreateMap<InterestDTO, Interest>().ReverseMap();
            CreateMap<CreateInterestDTO, Interest>().ReverseMap();
        }
    }
}