using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eVeterinarskaAmbulanta.Models;
using eVeterinarskaAmbulanta.Repositories;
using eVeterinarskaAmbulanta.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace eVeterinarskaAmbulanta.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthVetController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IVeterinarianRepository _repository;

        public AuthVetController(IConfiguration config, IVeterinarianRepository repository)
        {
            _config = config;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel login)
        {
            var vet = await _repository.GetVetByEmailAsync(login.Email);

            var calculatedPassword = PasswordHasher.ComputeHash(login.Password, vet.PasswordSalt, _config["PasswordHasher:Pepper"], 3);
            if (vet == null || vet.Password != calculatedPassword)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, vet.Email),
                    new Claim(ClaimTypes.Name, vet.FirstName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });

        }
    }
}
