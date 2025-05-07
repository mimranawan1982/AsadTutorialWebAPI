using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AsadTutorialWebAPI.DTO;
using AsadTutorialWebAPI.Interfaces;
using AsadTutorialWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsadTutorialWebAPI.Controllers
{
  
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;

        public AccountController(IUnitOfWork uow, IConfiguration configuration)
        {
            this.uow = uow;
            this.configuration = configuration;
        }


        //api/Account/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserReqDTO userReq)
        {
           var user =  await uow.UserRepository.Authenticate(userReq.Username, userReq.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            var response = new UserResDTO();
            response.username = user.Username;
            response.Token = CreateJWT(user);
            return Ok(response);
        }


        //api/Account/Login
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserReqDTO userReq)
        {
            if (await uow.UserRepository.UserAlreadyExist(userReq.Username))
                return BadRequest("User already exist");

            uow.UserRepository.Register(userReq.Username, userReq.Password);
            uow.SaveAsync();
            return Ok(201);
        }

        private string CreateJWT(User user)
        {

            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new Claim[] {
             new Claim(ClaimTypes.Name, user.Username),
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
     
    }
}
