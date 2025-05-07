using AsadTutorialWebAPI.Data;
using AsadTutorialWebAPI.Data.Repo;
using AsadTutorialWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace AsadTutorialWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EFCityController : ControllerBase
    {
      
        private readonly DataContext _context;
        public EFCityController(DataContext context)
        {
            this._context = context;
        }
    
        [HttpGet]
        public async Task<IActionResult> getCities()
        {
          
           var cit = await _context.Cities.ToListAsync();
          

            return Ok(cit);
        }


        //api/City/add?CityName="Miami"
        //[HttpPost("add")]

        //api/city/add/Miami
        //[HttpPost("add/{cityname}")]
        //public async Task<IActionResult> AddCity(string CityName)
        //{
        //    City c = new City();
        //    c.Name = CityName;
        //    await _context.Cities.AddAsync(c);
        //    await _context.SaveChangesAsync();
        //    return Ok(c);
        //}


        [HttpPost("Post")]
        public async Task<IActionResult> AddCity(City c)
        {
            await _context.Cities.AddAsync(c);
            await _context.SaveChangesAsync();
            return Ok(c);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var cit = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (cit != null)
            {
                _context.Cities.Remove(cit);
                await _context.SaveChangesAsync();
                return Ok(id);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}