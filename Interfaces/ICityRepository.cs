using AsadTutorialWebAPI.Models;

namespace AsadTutorialWebAPI.Interfaces
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        void AddCity(City C);
        void DeleteCity(int CityId);
        Task<City> GetCityById(int id);
    }
}