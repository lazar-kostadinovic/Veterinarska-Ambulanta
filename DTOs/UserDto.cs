using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.DTOs;


public class BaseUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

}


public class UserDto : BaseUserDto
{
    public int Id { get; set; }
}


public class UserDtoCreate : BaseUserDto
{

}

public class UserDtoUpdate : BaseUserDto
{
    public int Id { get; set; }
}

public class UserWithPetsDto : BaseUserDto
{
    public int Id { get; set; }
    public int NumberOfPets
    {
        get
        {
            return Pets.Count;
        }
    }
    public ICollection<PetDto> Pets { get; set; } = new List<PetDto>();
}

