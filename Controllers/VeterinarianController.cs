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
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinarianController : ControllerBase
    {
        private readonly IVeterinarianRepository _repository;
        private readonly IMapper _mapper;

        public VeterinarianController(IVeterinarianRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/veterinarian
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeterinarianDto>>> GetAmbulanceVeterinarians()
        {
            try
            {
                var veterinarians = await _repository
                    .GetAllAsync();

                if (veterinarians == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<VeterinarianDto>>(veterinarians));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpGet("ambulancevets")]
        public async Task<ActionResult<IEnumerable<VeterinarianDto>>> GetVeterinarians(int ambulanceId)
        {
            try
            {
                if (!await _repository.AmbulanceExistsAsync(ambulanceId))
                {
                    return NotFound();
                }

                var veterinarians = await _repository
                    .GetVetsFromAmbulanceAsync(ambulanceId);

                if (veterinarians == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<VeterinarianDto>>(veterinarians));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VeterinarianDto>> GetPet(int id)
        {
            try
            {
                var vet = await _repository.GetByIdAsync(id);
                if (vet == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<VeterinarianDto>(vet));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // [HttpGet("Profile")]
        // public async Task<ActionResult<VeterinarianDto>> GetVetProfile()
        // {
        //     try
        //     {
        //         // Retrieve the authenticated vet's email from the claims
        //         var vetEmail = Veterinarian.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        //         // Retrieve the vet from the repository based on the email
        //         var vet = await _repository.GetVetByEmailAsync(vetEmail);

        //         if (vet == null)
        //         {
        //             return NotFound();
        //         }

        //         // Return the vet profile
        //         return Ok(_mapper.Map<VeterinarianDto>(vet));
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Error: {ex}");
        //     }
        // }

        [HttpGet("profile")]
        public async Task<ActionResult<VeterinarianDto>> GetVetProfile()
        {
            try
            {
                // Get the authenticated user's ID
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Get the veterinarian profile using the user ID
                var vetProfile = await _repository.GetVetByEmailAsync(userEmail);

                if (vetProfile == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<VeterinarianDto>(vetProfile));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        //POST: api/veterinarian
        [HttpPost]
        public async Task<ActionResult<VeterinarianDto>> PostVet(int ambulanceId, VeterinarianDtoCreate vetDto)
        {
            try
            {
                if (!await _repository.AmbulanceExistsAsync(ambulanceId))
                {
                    return NotFound();
                }

                var vet = _mapper.Map<Veterinarian>(vetDto);
                await _repository.AddAsync(ambulanceId, vet);

                return Ok(_mapper.Map<VeterinarianDto>(vet));
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // PUT: api/veterinarian/2
        [HttpPut("{id}")]
        public async Task<ActionResult> PutVeterinarian(int id, int ambulanceId, VeterinarianDtoUpdate vetDto)
        {
            try
            {
                if (!await _repository.AmbulanceExistsAsync(ambulanceId))
                {
                    return NotFound();
                }

                var vet = _mapper.Map<Veterinarian>(vetDto);
                if (id != vet.Id)
                {
                    return BadRequest();
                }

                await _repository.UpdateAsync(ambulanceId, vet);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
        // DELETE: api/veterinarian/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVet(int ambulanceId, int id)
        {
            try
            {
                if (!await _repository.AmbulanceExistsAsync(ambulanceId))
                {
                    return BadRequest();
                }

                var vet = await _repository.GetVetFromAmbulanceAsync(ambulanceId, id);
                if (vet == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(vet);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModelVet resource)
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

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetVeterinarianCount()
        {
            try
            {
                var count = await _repository.GetVeterinarianCount();

                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
    }
}
