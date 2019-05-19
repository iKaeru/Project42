using AutoMapper;
using Model = Models.User;

namespace MemoryCardsAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Model.User, Model.UserRegistrationInfo>();
            CreateMap<Model.UserRegistrationInfo, Model.User>();
        }
    }
}