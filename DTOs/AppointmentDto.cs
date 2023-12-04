using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.DTOs
{
    public class BaseAppointmentDto
    {
        public int VeterinarianId { get; set; }
        public int PetId { get; set; }
        public DateTime Date { get; set; }
        //public TimeSpan Time { get; set; }
        public string Symptom { get; set; }
    }

    public class AppointmentDto : BaseAppointmentDto
    {
        public int Id { get; set; }
    }

    public class AppointmentDtoCreate : BaseAppointmentDto
    {

    }
    public class AppointmentDtoUpdate : BaseAppointmentDto
    {
        public int Id { get; set; }
    }
}
