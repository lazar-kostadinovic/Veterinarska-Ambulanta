using System.ComponentModel.DataAnnotations;
using eVeterinarskaAmbulanta.Entities;

namespace eVeterinarskaAmbulanta.DTOs;


public class BasePetDto
{
    public string Name { get; set; }
    public string Species { get; set; }
    public int Age { get; set; }
    public int userId { get; set; }
}


public class PetDto : BasePetDto
{
    public int Id { get; set; }
}


public class PetDtoCreate : BasePetDto
{

}

public class PetDtoUpdate : BasePetDto
{
    public int Id { get; set; }
}

public class PetWithAppDto : BasePetDto
{
    public int Id { get; set; }
    public int NumberOfAppointments
    {
        get
        {
            return Appointments.Count;
        }
    }
    public ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
}