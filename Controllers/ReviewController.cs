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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAppointments()
        {
            try
            {
                var reviews = await _repository.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // GET: api/review/veterinarian/{id}
        [HttpGet("veterinarian/{id}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByVeterinarian(int id)
        {
            try
            {
                var reviews = await _repository.GetReviewsFromVetAsync(id);
                if (reviews == null || !reviews.Any())
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
        [HttpGet("/vetreviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetVetReviews(int vetId)
        {
            try
            {
                if (!await _repository.VeterinarianExistsAsync(vetId))
                {
                    return NotFound();
                }

                var reviews = await _repository.GetReviewsFromVetAsync(vetId);

                if (reviews == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
        // POST: api/review
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> PostReview(ReviewDtoCreate reviewDto)
        {
            try
            {
                if (!await _repository.UserExistsAsync(reviewDto.UserId))
                {
                    return BadRequest();
                }

                if (!await _repository.VeterinarianExistsAsync(reviewDto.VeterinarianId))
                {
                    return BadRequest();
                }

                var review = _mapper.Map<Review>(reviewDto);
                await _repository.AddAsync(review);
                return Ok(_mapper.Map<ReviewDto>(review));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        // PUT: api/review/5
        //[HttpPut("{id}")]
        //public async Task<ActionResult> PutReview(int id, ReviewDtoUpdate reviewDto)
        //{
        //    try
        //    {
        //        var review = _mapper.Map<Review>(reviewDto);
        //        if (id != review.Id)
        //        {
        //            return BadRequest();
        //        }

        //        await _repository.UpdateAsync(review);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error: {ex}");
        //    }
        //}

        // DELETE: api/review/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            try
            {
                var review = await _repository.GetByIdAsync(id);
                if (review == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(review);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
    }
}