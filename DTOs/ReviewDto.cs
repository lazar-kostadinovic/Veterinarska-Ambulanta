using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.DTOs
{
    public class BaseReviewDto
    {
        public int VeterinarianId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }

    public class ReviewDto : BaseReviewDto
    {
        public int Id { get; set; }
    }

    public class ReviewDtoCreate : BaseReviewDto
    {

    }
    public class ReviewDtoUpdate : BaseReviewDto
    {
        public int Id { get; set; }
    }
}