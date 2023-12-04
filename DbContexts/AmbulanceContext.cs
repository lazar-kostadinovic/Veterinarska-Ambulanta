using eVeterinarskaAmbulanta.Entities;
using eVeterinarskaAmbulanta.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace eVeterinarskaAmbulanta.DbContexts;

public class AmbulanceContext : DbContext
{
    public DbSet<Ambulance> Ambulances { get; set; }
    public DbSet<Veterinarian> Veterinarians { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Review> Reviews { get; set; }

    private readonly IConfiguration _configuration;

    public AmbulanceContext(DbContextOptions<AmbulanceContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedAmbulance(modelBuilder);
        SeedUser(modelBuilder);
        SeedPet(modelBuilder);
        SeedVeterinarian(modelBuilder);

        modelBuilder.Entity<Appointment>().HasKey(a => a.Id);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Veterinarian)
            .WithMany(v => v.Appointments)
            .HasForeignKey(a => a.VeterinarianId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Pet)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PetId);
        ////

        modelBuilder.Entity<Review>().HasKey(a => a.Id);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Veterinarian)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.VeterinarianId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.UserId);
        ///

        base.OnModelCreating(modelBuilder);
    }

    private void SeedAmbulance(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ambulance>()
            .HasData(
            new Ambulance()
            {
                Id = 1,
                Name = "Vet Medic",
                Adress = "Bulevar Nemanjica",
                Phone = "0647061254"
            },
            new Ambulance()
            {
                Id = 2,
                Name = "Pet Lovers",
                Adress = "Bulevar Zorana Djindjica",
                Phone = "0647061251"
            });
    }

    private void SeedUser(ModelBuilder modelBuilder)
    {

        var salt1 = PasswordHasher.GenerateSalt();
        var salt2 = PasswordHasher.GenerateSalt();

        modelBuilder.Entity<User>()
            .HasData(
            new User()
            {
                Id = 1,
                FirstName = "Dusan",
                LastName = "Djordjevic",
                Email = "dusandjordjevic@gmail.com",
                Password = PasswordHasher.ComputeHash("root", salt1, _configuration["PasswordHasher:Pepper"], _configuration.GetValue<int>("PasswordHasher:Iteration")),
                Role = UserRole.Admin,
                PasswordSalt = salt1


            }
          );
    }

    private void SeedPet(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>()
            .HasData(
            new Pet()
            {
                Id = 1,
                Name = "Slavko",
                Species = "dog",
                Age = 3,
                UserId = 1
            },
            new Pet()
            {
                Id = 2,
                Name = "Mirko",
                Species = "dog",
                Age = 7,
                UserId = 1
            },
            new Pet()
            {
                Id = 3,
                Name = "Luna",
                Species = "turtle",
                Age = 10,
                UserId = 2

            },
            new Pet()
            {
                Id = 4,
                Name = "Maks",
                Species = "cat",
                Age = 4,
                UserId = 2
            });


    }

    private void SeedVeterinarian(ModelBuilder modelBuilder)
    {

        var salt1 = PasswordHasher.GenerateSalt();

        modelBuilder.Entity<Veterinarian>()
            .HasData(
            new Veterinarian()
            {
                Id = 2,
                AmbulanceId = 1,
                Email = "proba@gmail.com",
                FirstName = "proba",
                LastName = "proba",
                Password = PasswordHasher.ComputeHash("vet123", salt1, _configuration["PasswordHasher:Pepper"], _configuration.GetValue<int>("PasswordHasher:Iteration")),
                PasswordSalt = salt1
            });
    }


    //private void SeedVeterinarians(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Veterinarian>()
    //        .HasData(
    //        new Veterinarian()
    //        {
    //            Id = 1,
    //            FirstName = "Lazar",
    //            LastName = "Jovic",
    //            AmbulanceId = 1
    //        },
    //        new Veterinarian()
    //        {
    //            Id = 2,
    //            FirstName = "Nikola",
    //            LastName = "Jocic",
    //            AmbulanceId = 1
    //        },
    //        new Veterinarian()
    //        {
    //            Id = 3,
    //            FirstName = "Stefan",
    //            LastName = "Milicevic",
    //            AmbulanceId = 2
    //        });
    //}
}









