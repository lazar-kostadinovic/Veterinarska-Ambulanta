using eVeterinarskaAmbulanta.DbContexts;
using eVeterinarskaAmbulanta.Entities;
using Microsoft.EntityFrameworkCore;

namespace eVeterinarskaAmbulanta.Repositories;


public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAllAsync();
    Task<Review> GetByIdAsync(int id);
    Task<IEnumerable<Review>> GetReviewsFromVetAsync(int vetId);
    Task<bool> VeterinarianExistsAsync(int vetId);
    Task<bool> UserExistsAsync(int userId);
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
}

public class ReviewRepository : IReviewRepository
{
    private readonly AmbulanceContext _dbContext;

    public ReviewRepository(AmbulanceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _dbContext.Reviews.Include(v => v.Veterinarian)
                                            .Include(u => u.User)
                                            .ToListAsync();
    }

    public async Task<Review> GetByIdAsync(int id)
    {
        return await _dbContext.Reviews.FindAsync(id);
    }

    public async Task<IEnumerable<Review>> GetReviewsFromVetAsync(int vetId)
    {
        return await _dbContext.Reviews
           .Where(r => r.VeterinarianId == vetId).ToListAsync();
    }

    public async Task<bool> VeterinarianExistsAsync(int vetId)
    {
        return await _dbContext.Veterinarians.AnyAsync(v => v.Id == vetId);
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _dbContext.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task AddAsync(Review review)
    {
        await _dbContext.Reviews.AddAsync(review);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Review review)
    {
        _dbContext.Entry(review).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Review review)
    {
        _dbContext.Reviews.Remove(review);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
