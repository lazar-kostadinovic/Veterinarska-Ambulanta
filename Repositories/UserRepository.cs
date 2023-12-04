using System.Data;
using eVeterinarskaAmbulanta.DbContexts;
using eVeterinarskaAmbulanta.Entities;
using eVeterinarskaAmbulanta.Models;
using eVeterinarskaAmbulanta.Services;
using Microsoft.EntityFrameworkCore;

namespace eVeterinarskaAmbulanta.Repositories;


public interface IUserRepository
{


    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int userId, bool includePets);

    //Task<bool> UserExistsAsync(int userId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
    Task<bool> Register(RegistrationModel resource);

}

public class UserRepository : IUserRepository
{
    private readonly AmbulanceContext _dbContext;
    private readonly string _pepper;
    private readonly int _iteration;

    public UserRepository(AmbulanceContext dbContext, IConfiguration config)
    {
        _dbContext = dbContext;
        _pepper = config["PasswordHasher:Pepper"];
        _iteration = config.GetValue<int>("PasswordHasher:Iteration");
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.OrderBy(u => u.FirstName).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int userId, bool includePets)
    {
        if (includePets)
        {
            return await _dbContext.Users.Include(u => u.Pets).
                Where(u => u.Id == userId).FirstOrDefaultAsync();
        }
        return await _dbContext.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
    }

    //public async Task<bool> UserExistsAsync(int userId)
    //{
    //    return await _dbContext.Users.AnyAsync(u => u.Id == userId);
    //}
    public async Task AddAsync(User user)
    {
        var newUser = new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PasswordSalt = PasswordHasher.GenerateSalt(),
            Role = user.Role
        };
        user.Password = PasswordHasher.ComputeHash(user.Password, newUser.PasswordSalt, _pepper, _iteration);

        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();

        await SaveChangesAsync();
    }
    // public async Task AddAsync(User user)
    // {
    //     await _dbContext.Users.AddAsync(user);
    //     await SaveChangesAsync();
    // }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Entry(user).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<bool> Register(RegistrationModel resource)
    {
        var user = new User
        {
            FirstName = resource.FirstName,
            LastName = resource.LastName,
            Email = resource.Email,
            PasswordSalt = PasswordHasher.GenerateSalt(),
            Role = UserRole.User
        };
        user.Password = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}

