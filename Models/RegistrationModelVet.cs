using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.Models
{

    public class RegistrationModelVet
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int ambulanceId { get; set; }
    }
}
