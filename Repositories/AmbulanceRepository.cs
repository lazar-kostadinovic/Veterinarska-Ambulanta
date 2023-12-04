using eVeterinarskaAmbulanta.DbContexts;
using eVeterinarskaAmbulanta.Entities;
using Microsoft.EntityFrameworkCore;

namespace eVeterinarskaAmbulanta.Repositories;


public interface IAmbulanceRepository
{
    Task<IEnumerable<Ambulance>> GetAllAsync();
    Task<Ambulance> GetByIdAsync(int id);
    Task AddAsync(Ambulance ambulance);
    Task UpdateAsync(Ambulance ambulance);
    Task DeleteAsync(Ambulance ambulance);
}

public class AmbulanceRepository : IAmbulanceRepository
{
    private readonly AmbulanceContext _dbContext;

    public AmbulanceRepository(AmbulanceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Ambulance>> GetAllAsync()
    {
        return await _dbContext.Ambulances.Include(a => a.Veterinarinas).ToListAsync();
    }
  
    public async Task<Ambulance> GetByIdAsync(int id)
    {
        return await _dbContext.Ambulances.FindAsync(id);
    }

    public async Task AddAsync(Ambulance ambulance)
    {
        await _dbContext.Ambulances.AddAsync(ambulance);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Ambulance ambulance)
    {
        _dbContext.Entry(ambulance).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Ambulance ambulance)
    {
        _dbContext.Ambulances.Remove(ambulance);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
