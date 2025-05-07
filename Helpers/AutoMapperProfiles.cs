using AsadTutorialWebAPI.DTO;
using AsadTutorialWebAPI.Models;
using AutoMapper;

namespace AsadTutorialWebAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
                CreateMap<City,CityDTO>().ReverseMap();          
        }
    }
}
