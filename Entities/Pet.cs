  using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(20)]
        public string? Species { get; set; }
        public int Age { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
        public List<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public Pet() { }
    }
}
