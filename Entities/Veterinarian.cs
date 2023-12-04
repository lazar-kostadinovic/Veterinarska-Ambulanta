using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eVeterinarskaAmbulanta.Entities
{
    public class Veterinarian
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Ambulance? Ambulance { get; set; }
        public int AmbulanceId { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Review>? Reviews { get; set; } = new List<Review>();

        public Veterinarian() { }
    }
}
