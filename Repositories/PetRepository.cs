using System.Collections;
using eVeterinarskaAmbulanta.DbContexts;
using eVeterinarskaAmbulanta.Entities;
using Microsoft.EntityFrameworkCore;

namespace eVeterinarskaAmbulanta.Repositories;


public interface IPetRepository
{
    Task<IEnumerable<Pet>> GetAllAsync();
    Task<Pet> GetByIdAsync(int id, bool includeAppointments);
    Task<Pet> GetPetFromUserAsync(int userId, int petId);
    Task<IEnumerable<Pet>> GetPetsFromUserAsync(int userId);
    Task<bool> UserExistsAsync(int userId);
    Task AddAsync(int userId, Pet pet);
    Task UpdateAsync(int userId, Pet pet);
    Task DeleteAsync(Pet pet);
    Task SaveChangesAsync();
}

public class PetRepository : IPetRepository
{
    private readonly AmbulanceContext _dbContext;

    public PetRepository(AmbulanceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Pet>> GetAllAsync()
    {
        return await _dbContext.Pets.Include(a => a.Appointments).ToListAsync();
    }

    public async Task<Pet> GetByIdAsync(int id, bool includeAppointments)
    {
        if(includeAppointments) 
        {
            return await _dbContext.Pets.Include(p => p.Appointments).Where(p => p.Id == id).FirstOrDefaultAsync();
        } 

        return await _dbContext.Pets.FindAsync(id);
    }

    public async Task<Pet> GetPetFromUserAsync(int userId, int petId)
    {
        return await _dbContext.Pets.Where(p => p.UserId == userId && p.Id == petId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Pet>> GetPetsFromUserAsync(int userId)
    {
        return await _dbContext.Pets.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _dbContext.Users.AnyAsync(u => u.Id == userId);
    }


    public async Task AddAsync(int userId, Pet pet)
    {
        var user = await _dbContext.Users.
            Where(u => u.Id == userId).Include(p => p.Pets).FirstOrDefaultAsync();
        if (user != null) 
        {
            //user.Pets.Add(pet);
            await _dbContext.Pets.AddAsync(pet);
            await SaveChangesAsync();
        }    
    }

    public async Task UpdateAsync(int userId, Pet pet)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user != null)
        {
            _dbContext.Entry(pet).State = EntityState.Modified;
            await SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Pet pet)
    {
        _dbContext.Pets.Remove(pet);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
