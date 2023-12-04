using AutoMapper;
using eVeterinarskaAmbulanta.DTOs;
using eVeterinarskaAmbulanta.Entities;
using eVeterinarskaAmbulanta.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eVeterinarskaAmbulanta.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetRepository _repository;
        private readonly IMapper _mapper;
        public PetController(IPetRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/pet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetAllPets()
        {
            try
            {
                var pets = await _repository.GetAllAsync();

                if (pets == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<PetDto>>(pets));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpGet("/userpets")]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetUserPets(int userId)
        {
            try
            {
                if (!await _repository.UserExistsAsync(userId))
                {
                    return NotFound();
                }

                var pets = await _repository.GetPetsFromUserAsync(userId);

                if (pets == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<PetDto>>(pets));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetDto>> GetPet(int id, bool includeAppointments)
        {
            try
            {
                var pet = await _repository.GetByIdAsync(id, includeAppointments);
                if (pet == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<PetWithAppDto>(pet));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PetDto>> PostPet(int userId, PetDtoCreate petDto)
        {
            try
            {
                if (!await _repository.UserExistsAsync(userId))
                {
                    return NotFound();
                }

                var pet = _mapper.Map<Pet>(petDto);
                await _repository.AddAsync(userId, pet);

                return Ok(_mapper.Map<PetDto>(pet));
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // PUT: api/pet/2
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPet(int userId, int id, PetDtoUpdate petDto)
        {
            try
            {
                if (!await _repository.UserExistsAsync(userId))
                {
                    return NotFound();
                }

                var pet = _mapper.Map<Pet>(petDto);
                if (id != pet.Id)
                {
                    return BadRequest();
                }

                await _repository.UpdateAsync(userId, pet);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // DELETE: api/pet/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePet(int userId, int id)
        {
            try
            {
                if (!await _repository.UserExistsAsync(userId))
                {
                    return BadRequest();
                }

                var pet = await _repository.GetPetFromUserAsync(userId, id);
                if (pet == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(pet);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
    }
}
