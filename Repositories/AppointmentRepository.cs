using eVeterinarskaAmbulanta.DbContexts;
using eVeterinarskaAmbulanta.Entities;
using Microsoft.EntityFrameworkCore;

namespace eVeterinarskaAmbulanta.Repositories;


public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAllAsync();
    Task<Appointment> GetByIdAsync(int id);
    //
    Task<Appointment> GetAppFromPetAsync(int petId, int appointmentId);
    Task<IEnumerable<Appointment>> GetAppsFromPetAsync(int petId);
    //
    Task<Appointment> GetAppFromVetAsync(int vetId, int appointmentId);
    Task<IEnumerable<Appointment>> GetAppsFromVetAsync(int vetId);
    //
    Task<bool> VeterinarianExistsAsync(int vetId);
    Task<bool> PetExistsAsync(int petId);

    Task AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
    Task DeleteAsync(Appointment appointment);

}

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AmbulanceContext _dbContext;

    public AppointmentRepository(AmbulanceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _dbContext.Appointments.Include(v => v.Veterinarian)
                                            .Include(p => p.Pet)
                                            .ToListAsync();
    }
    public async Task<Appointment> GetByIdAsync(int id)
    {
        return await _dbContext.Appointments.FindAsync(id);
    }
    public async Task<Appointment> GetAppFromPetAsync(int petId, int appointmentId)
    {
        return await _dbContext.Appointments
            .Where(a => a.Id == appointmentId && a.PetId == petId)
            .FirstOrDefaultAsync();
    }
    public async Task<Appointment> GetAppFromVetAsync(int vetId, int appointmentId)
    {
        return await _dbContext.Appointments
                .Where(a => a.Id == appointmentId && a.VeterinarianId == vetId)
                .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppsFromPetAsync(int petId)
    {
        return await _dbContext.Appointments
            .Where(a => a.PetId == petId).ToListAsync();
    }

   public async Task<IEnumerable<Appointment>> GetAppsFromVetAsync(int vetId)
    {
        return await _dbContext.Appointments
            .Where(a => a.VeterinarianId == vetId).ToListAsync();
    }

    public async Task<bool> VeterinarianExistsAsync(int vetId)
    {
        return await _dbContext.Veterinarians.AnyAsync(v => v.Id == vetId);
    }

    public async Task<bool> PetExistsAsync(int petId)
    {
        return await _dbContext.Pets.AnyAsync(p => p.Id == petId);
    }


    public async Task AddAsync(Appointment appointment)
    {
        await _dbContext.Appointments.AddAsync(appointment);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _dbContext.Entry(appointment).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Appointment appointment)
    {
        _dbContext.Appointments.Remove(appointment);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
