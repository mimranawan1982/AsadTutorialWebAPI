 
using AsadTutorialWebAPI.Interfaces;
using AsadTutorialWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AsadTutorialWebAPI.Data.Repo
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext dc;

        public CityRepository(DataContext dc)
        {
            this.dc = dc;
        }


        public void AddCity(City C)
        {
           dc.Cities.AddAsync(C);
        }

        public void DeleteCity(int CityId)
        {
            var city = dc.Cities.Find(CityId);
            dc.Cities.Remove(city);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await dc.Cities.ToListAsync();
        }

        public async Task<City> GetCityById(int id)
        {
            return await dc.Cities.FindAsync(id);
        }
    }
}
