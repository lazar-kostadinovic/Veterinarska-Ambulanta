using eVeterinarskaAmbulanta.DbContexts;
using eVeterinarskaAmbulanta.Entities;
using Microsoft.EntityFrameworkCore;
using eVeterinarskaAmbulanta.Models;
using eVeterinarskaAmbulanta.Services;

namespace eVeterinarskaAmbulanta.Repositories;


public interface IVeterinarianRepository
{
    Task<IEnumerable<Veterinarian>> GetAllAsync();
    Task<Veterinarian> GetVetFromAmbulanceAsync(int ambulanceId, int veterinarianId);
    Task<IEnumerable<Veterinarian>> GetVetsFromAmbulanceAsync(int ambulanceId);
    Task<Veterinarian> GetVetByEmailAsync(string email);
    Task<bool> AmbulanceExistsAsync(int ambulanceId);
    Task<Veterinarian> GetByIdAsync(int id);
    Task<Veterinarian> GetVetProfileAsync(string userId);
    Task AddAsync(int ambulanceId, Veterinarian vet);
    Task UpdateAsync(int abulanceId, Veterinarian vet);
    Task DeleteAsync(Veterinarian vet);
    Task<bool> Register(RegistrationModelVet resource);
    Task<int> GetVeterinarianCount();
}

public class VeterinarianRepository : IVeterinarianRepository
{
    private readonly AmbulanceContext _dbContext;

    private readonly string _pepper;
    private readonly int _iteration;

    public VeterinarianRepository(AmbulanceContext dbContext, IConfiguration config)
    {
        _dbContext = dbContext;
        _pepper = config["PasswordHasher:Pepper"];
        _iteration = config.GetValue<int>("PasswordHasher:Iteration");
    }

    public async Task<IEnumerable<Veterinarian>> GetAllAsync()
    {
        return await _dbContext.Veterinarians.Include(a => a.Appointments).ToListAsync();
    }
    public async Task<int> GetVeterinarianCount()
    {
        return await _dbContext.Veterinarians.CountAsync();
    }

    public async Task<Veterinarian> GetByIdAsync(int id)
    {
        return await _dbContext.Veterinarians.FindAsync(id);
    }
    public async Task<Veterinarian> GetVetProfileAsync(string userId)
    {
        return await _dbContext.Veterinarians.FindAsync(userId);
    }
    public async Task<Veterinarian> GetVetFromAmbulanceAsync(int ambulanceId, int veterinarianId)
    {
        return await _dbContext.Veterinarians.
            Where(v => v.AmbulanceId == ambulanceId && v.Id == veterinarianId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Veterinarian>> GetVetsFromAmbulanceAsync(int ambulanceId)
    {
        return await _dbContext.Veterinarians.Where(v => v.AmbulanceId == ambulanceId).ToListAsync();
    }

    public async Task<bool> AmbulanceExistsAsync(int ambulanceId)
    {
        return await _dbContext.Ambulances.AnyAsync(a => a.Id == ambulanceId);
    }

    public async Task AddAsync(int ambulanceId, Veterinarian vet)
    {
        var ambulance = await _dbContext.Ambulances.
            Where(a => a.Id == ambulanceId).Include(v => v.Veterinarinas).FirstOrDefaultAsync();
        if (ambulance != null)
        {
            ambulance.Veterinarinas.Add(vet);
            await _dbContext.Veterinarians.AddAsync(vet);
            await SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(int ambulanceId, Veterinarian vet)
    {
        var ambulance = await _dbContext.Veterinarians.FindAsync(ambulanceId);
        if (ambulance != null)
        {
            _dbContext.Entry(vet).State = EntityState.Modified;
            await SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Veterinarian vet)
    {
        _dbContext.Veterinarians.Remove(vet);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
    public async Task<Veterinarian> GetVetByEmailAsync(string email)
    {
        return await _dbContext.Veterinarians.Where(u => u.Email == email).FirstOrDefaultAsync();
    }
    public async Task<bool> Register(RegistrationModelVet resource)
    {
        var vet = new Veterinarian
        {
            FirstName = resource.FirstName,
            LastName = resource.LastName,
            Email = resource.Email,
            PasswordSalt = PasswordHasher.GenerateSalt(),
            AmbulanceId = resource.ambulanceId
        };
        vet.Password = PasswordHasher.ComputeHash(resource.Password, vet.PasswordSalt, _pepper, _iteration);
        await _dbContext.Veterinarians.AddAsync(vet);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
