namespace eVeterinarskaAmbulanta.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int VeterinarianId { get; set; }
        public int PetId { get; set; }
        public DateTime Date { get; set; }
        //public TimeSpan Time { get; set; }
        public string Symptom { get; set; }
        public Veterinarian Veterinarian { get; set; }
        public Pet Pet { get; set; }
    }
}
