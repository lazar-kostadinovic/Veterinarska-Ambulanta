using System.ComponentModel.DataAnnotations;

namespace eVeterinarskaAmbulanta.Entities
{
    public enum UserRole
    {
        User, Veterinarian, Admin
    }

    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        [MaxLength(10)]
        public UserRole Role { get; set; }
        public List<Pet>? Pets { get; set; } = new List<Pet>();
        public List<Review>? Reviews { get; set; } = new List<Review>();

        public User() { }
    }
}
