using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.DTOs;


public class BaseAmbulanceDto
{
    public string Name { get; set; }
    public string Adress { get; set; }
    public string Phone { get; set; }
}


public class AmbulanceDto : BaseAmbulanceDto
{
    public int Id { get; set; }
}


public class AmbulanceDtoCreate : BaseAmbulanceDto
{

}

public class AmbulanceDtoUpdate : BaseAmbulanceDto
{
    public int Id { get; set; }
}
