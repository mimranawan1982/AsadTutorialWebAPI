using AsadTutorialWebAPI.Data;
using AsadTutorialWebAPI.DTO;
using AsadTutorialWebAPI.Interfaces;
using AsadTutorialWebAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace AsadTutorialWebAPI.Controllers
{
    [Authorize]
    public class CityController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CityController(IUnitOfWork uow, IMapper mapper)
        {          
            this.uow = uow;
            this.mapper = mapper;
        }

       [AllowAnonymous]
        [HttpGet("cities")]
        public async Task<IActionResult> getCities()
        {
           // throw new UnauthorizedAccessException();
            var cit = await uow.CityRepository.GetCitiesAsync();
            var CityDto = mapper.Map<IEnumerable<CityDTO>>(cit);

            //var CityDto = from mc in cit select new CityDTO { Id = mc.Id, Name = mc.Name, xyz = "1" };

            return Ok(CityDto);
        }


        [HttpPost("Post")]
        public async Task<IActionResult> AddCity(CityDTO citydto)
        {
            //var cit = new City
            //{
            //    Name = c.Name,
            //    LastUpdatedBy = "1",
            //    LastUpdateOn = DateTime.Now
            //};

            var cit = mapper.Map<City>(citydto);
            cit.LastUpdatedBy = "1";
            cit.LastUpdateOn = DateTime.Now;

            uow.CityRepository.AddCity(cit);
            await uow.SaveAsync();
            return StatusCode(201);
        }


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCity(int id, CityDTO citydto)
        {
            var cit = await uow.CityRepository.GetCityById(id);

            if (cit == null)
                return BadRequest("Update not allowed");
            cit.LastUpdatedBy = "1";
            cit.LastUpdateOn = DateTime.Now;

           // mapper.Map<CityDTO>(cit);
            mapper.Map(citydto, cit);

            await uow.SaveAsync();
            return StatusCode(201);
        }



        [HttpPatch("UpdateCityPatch/{id}")]
        public async Task<IActionResult> UpdateCityPatch(int id, JsonPatchDocument<City> citypatch)
        {
            var cit = await uow.CityRepository.GetCityById(id);
            cit.LastUpdatedBy = "1";
            cit.LastUpdateOn = DateTime.Now;

            citypatch.ApplyTo(cit,ModelState);

            await uow.SaveAsync();
            return StatusCode(201);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            uow.CityRepository.DeleteCity(id);
            await uow.SaveAsync();
            return StatusCode(202);
        }
    }
}