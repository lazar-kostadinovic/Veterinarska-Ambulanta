namespace eVeterinarskaAmbulanta.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VeterinarianId { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public User User { get; set; }
        public Veterinarian Veterinarian { get; set; }
        
    }
}