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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;
        public AppointmentController(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/appointment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
        {
            try
            {
                var appointments = await _repository.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<AppointmentDto>>(appointments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // GET: api/appointment/3
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
        {
            try
            {
                var appointment = await _repository.GetByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
        [HttpGet("veterinarian/{veterinarianId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByVeterinarian(int veterinarianId)
        {
            try
            {
                var appointments = await _repository.GetAppsFromVetAsync(veterinarianId);
                return Ok(_mapper.Map<IEnumerable<AppointmentDto>>(appointments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
        // POST: api/appointment
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> PostAppointment(AppointmentDtoCreate appointmentDto)
        {
            try
            {
                if (!await _repository.PetExistsAsync(appointmentDto.PetId))
                {
                    return BadRequest();
                }

                if (!await _repository.VeterinarianExistsAsync(appointmentDto.VeterinarianId))
                {
                    return BadRequest();
                }

                var appointment = _mapper.Map<Appointment>(appointmentDto);
                await _repository.AddAsync(appointment);
                return Ok(_mapper.Map<AppointmentDto>(appointment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // PUT: api/appointment/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAppointment(int id, AppointmentDtoUpdate appointmentDto)
        {
            try
            {
                var appointment = _mapper.Map<Appointment>(appointmentDto);
                if (id != appointment.Id)
                {
                    return BadRequest();
                }

                await _repository.UpdateAsync(appointment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // DELETE: api/appointment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            try
            {
                var appointment = await _repository.GetByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(appointment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
    }
}
