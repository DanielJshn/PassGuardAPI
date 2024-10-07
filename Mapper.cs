using AutoMapper;

namespace apief
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Password, PasswordDto>();
            CreateMap<AdditionalField, AdditionalFieldDto>();
        }
    }
}