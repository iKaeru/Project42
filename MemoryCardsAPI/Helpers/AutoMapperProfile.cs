using AutoMapper;
using Model = Models.User;
using View = Client.Models.User;

namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Model.User, View.UserRegistrationInfo>();
            CreateMap<View.UserRegistrationInfo, Model.User>();
        }
    }
}