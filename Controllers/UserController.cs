using AutoMapper;
using eVeterinarskaAmbulanta.DTOs;
using eVeterinarskaAmbulanta.Entities;
using eVeterinarskaAmbulanta.Models;
using eVeterinarskaAmbulanta.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace eVeterinarskaAmbulanta.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _repository.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        //GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id, bool includePets)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id, includePets);
                if (user == null)
                {
                    return NotFound();
                }

                if (includePets)
                {
                    return Ok(_mapper.Map<UserWithPetsDto>(user));
                }

                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpGet("Profile")]
        public async Task<ActionResult<UserDto>> GetUserProfile()
        {
            try
            {

                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;


                var user = await _repository.GetUserByEmailAsync(userEmail);

                if (user == null)
                {
                    return NotFound();
                }


                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // [HttpPost]
        // public async Task<ActionResult<UserDto>> PostUser(UserDtoCreate userDto)
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> PostUser([FromBody] UserDtoCreate userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                await _repository.AddAsync(user);
                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // PUT: api/user/2
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, UserDtoUpdate userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                if (id != user.Id)
                {
                    return BadRequest();
                }

                await _repository.UpdateAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id, false);
                if (user == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegistrationModel resource)
        {
            try
            {
                var response = await _repository.Register(resource);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }
    }
}
