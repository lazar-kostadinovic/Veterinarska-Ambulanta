using AutoMapper;
using eVeterinarskaAmbulanta.DTOs;
using eVeterinarskaAmbulanta.Entities;
using eVeterinarskaAmbulanta.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eVeterinarskaAmbulanta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmbulanceController : ControllerBase
    {
        private readonly IAmbulanceRepository _repository;
        private readonly IMapper _mapper;   
        public AmbulanceController(IAmbulanceRepository repository, IMapper mapper)
        {
            _repository = repository;  
            _mapper = mapper;   
        }

        // GET: api/ambulance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmbulanceDto>>> GetAmbulances()
        {
            try
            {
                var ambulances = await _repository.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<AmbulanceDto>>(ambulances));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // GET: api/ambulance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AmbulanceDto>> GetAmbulance(int id)
        {
            try
            {
                var ambulance = await _repository.GetByIdAsync(id);
                if (ambulance == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<AmbulanceDto>(ambulance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
        // POST: api/ambulance
        [HttpPost]
        public async Task<ActionResult<AmbulanceDto>> PostAmbulance(AmbulanceDtoCreate ambulanceDto)
        {
            try
            {
                var ambulance = _mapper.Map<Ambulance>(ambulanceDto);
                await _repository.AddAsync(ambulance);
                return Ok(_mapper.Map<AmbulanceDto>(ambulance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // PUT: api/ambulance/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAmbulance(int id, AmbulanceDtoUpdate ambulanceDto)
        {
            try
            {
                var ambulance = _mapper.Map<Ambulance>(ambulanceDto);
                if (id != ambulance.Id)
                {
                    return BadRequest();
                }

                await _repository.UpdateAsync(ambulance);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // DELETE: api/ambulance/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAmbulance(int id)
        {
            try
            {
                var ambulance = await _repository.GetByIdAsync(id);
                if (ambulance == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(ambulance);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
    }
}
