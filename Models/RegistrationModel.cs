using System.ComponentModel.DataAnnotations;
using eVeterinarskaAmbulanta.Entities;

namespace eVeterinarskaAmbulanta.Models
{

    public class RegistrationModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        //public UserRole Role { get; set; }
    }
}
