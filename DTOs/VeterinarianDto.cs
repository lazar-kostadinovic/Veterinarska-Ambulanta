using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.DTOs
{
    public class BaseVeterinarianDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AmbulanceId { get; set; }

    }

    public class VeterinarianDto : BaseVeterinarianDto
    {
        public int Id { get; set; }

    }

    public class VeterinarianDtoCreate : BaseVeterinarianDto
    {

    }

    public class VeterinarianDtoUpdate : BaseVeterinarianDto
    {
        public int Id { get; set; }
    }


}
