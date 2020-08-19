using AutoMapper;
using ProjetoForest.DTO;
using ProjetoForest.Models;

namespace ProjetoForest.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() 
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
        }
    }
}