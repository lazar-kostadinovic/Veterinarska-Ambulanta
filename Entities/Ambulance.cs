using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.Entities
{
    public class Ambulance
    {
        
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Adress { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        public List<Veterinarian>? Veterinarinas { get; set; } 
            = new List<Veterinarian>();

        public Ambulance() { }
    }
}
