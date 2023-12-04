﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eVeterinarskaAmbulanta.DbContexts;

#nullable disable

namespace eVeterinarskaAmbulanta.Migrations
{
    [DbContext(typeof(AmbulanceContext))]
    [Migration("20230602160110_VetReg")]
    partial class VetReg
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.16");

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Ambulance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Ambulances");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Adress = "Bulevar Nemanjica",
                            Name = "Vet Medic",
                            Phone = "0647061254"
                        },
                        new
                        {
                            Id = 2,
                            Adress = "Bulevar Zorana Djindjica",
                            Name = "Pet Lovers",
                            Phone = "0647061251"
                        });
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("PetId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Symptom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("TEXT");

                    b.Property<int>("VeterinarianId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PetId");

                    b.HasIndex("VeterinarianId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Pet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Species")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Pets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 3,
                            Name = "Slavko",
                            Species = "dog",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Age = 7,
                            Name = "Mirko",
                            Species = "dog",
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            Age = 10,
                            Name = "Luna",
                            Species = "turtle",
                            UserId = 2
                        },
                        new
                        {
                            Id = 4,
                            Age = 4,
                            Name = "Maks",
                            Species = "cat",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VeterinarianId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VeterinarianId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasMaxLength(10)
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "dusandjordjevic@gmail.com",
                            FirstName = "Dusan",
                            LastName = "Djordjevic",
                            Password = "1SgsoW3IvJf68saYjUFjmd3jYALkvLW+x3/AQtxNQuE=",
                            PasswordSalt = "fiMxiMH6EKNSgHxY5lQsqQ==",
                            Role = 2
                        },
                        new
                        {
                            Id = 2,
                            Email = "markomarkovic@gmail.com",
                            FirstName = "Marko",
                            LastName = "Markovic",
                            Password = "cIVi6y+xFV8YnrMpXhq0BDPvFc3DRGNle04/0dqPAts=",
                            PasswordSalt = "CSLv9PTrZhDjhsBGCucw/Q==",
                            Role = 1
                        });
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Veterinarian", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AmbulanceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AmbulanceId");

                    b.ToTable("Veterinarians");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            AmbulanceId = 1,
                            Email = "proba@gmail.com",
                            FirstName = "proba",
                            LastName = "proba",
                            Password = "NsyNPLJ5oKBZOdaOEqXqloBHoYIsXf3veUMwO3mzpt0=",
                            PasswordSalt = "o1qnffIQon7UxVSJgCG2oA=="
                        });
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Appointment", b =>
                {
                    b.HasOne("eVeterinarskaAmbulanta.Entities.Pet", "Pet")
                        .WithMany("Appointments")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eVeterinarskaAmbulanta.Entities.Veterinarian", "Veterinarian")
                        .WithMany("Appointments")
                        .HasForeignKey("VeterinarianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pet");

                    b.Navigation("Veterinarian");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Pet", b =>
                {
                    b.HasOne("eVeterinarskaAmbulanta.Entities.User", "User")
                        .WithMany("Pets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Review", b =>
                {
                    b.HasOne("eVeterinarskaAmbulanta.Entities.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eVeterinarskaAmbulanta.Entities.Veterinarian", "Veterinarian")
                        .WithMany("Reviews")
                        .HasForeignKey("VeterinarianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Veterinarian");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Veterinarian", b =>
                {
                    b.HasOne("eVeterinarskaAmbulanta.Entities.Ambulance", "Ambulance")
                        .WithMany("Veterinarinas")
                        .HasForeignKey("AmbulanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ambulance");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Ambulance", b =>
                {
                    b.Navigation("Veterinarinas");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Pet", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.User", b =>
                {
                    b.Navigation("Pets");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("eVeterinarskaAmbulanta.Entities.Veterinarian", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
