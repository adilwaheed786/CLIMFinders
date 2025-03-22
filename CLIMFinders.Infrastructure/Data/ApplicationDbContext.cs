using CLIMFinders.Application.Enums;
using CLIMFinders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CLIMFinders.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserAddress> Businesses { get; set; }
        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<Searches> Searches { get; set; }
        public virtual DbSet<Vehicles> Vehicles { get; set; }
        public virtual DbSet<VehiclesNotFound> VehiclesNotFound { get; set; }
        public virtual DbSet<VehicleMake> VehicleMakes { get; set; }
        public virtual DbSet<VehicleModel> VehicleModels { get; set; }
        public virtual DbSet<VehicleColor> VehicleColors { get; set; }
        public virtual DbSet<SubscriptionPlans> SubscriptionPlans { get; set; }
        public virtual DbSet<PlanServices> PlanServices { get; set; }
        public virtual DbSet<SubRoles> SubRoles { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasData(              
                new Roles { Id = (int)RoleEnum.Users, RoleNanme = RoleEnum.Users.ToString() },
                new Roles { Id = (int)RoleEnum.Business, RoleNanme = RoleEnum.Business.ToString() },
                new Roles { Id = (int)RoleEnum.SuperAdmin, RoleNanme = RoleEnum.SuperAdmin.ToString() }
            );
            modelBuilder.Entity<SubRoles>().HasData(
               new SubRoles { Id = (int)SubRoleEnum.Tow, RoleNanme = SubRoleEnum.Tow.ToString() },
               new SubRoles { Id = (int)SubRoleEnum.Impound, RoleNanme = SubRoleEnum.Impound.ToString() } 
           );
            modelBuilder.Entity<SubscriptionPlans>().HasData(
           new SubscriptionPlans { Id = 1, Tier = "User Registration (Car Owners)", Amount = 10, Duration = 1 },
           new SubscriptionPlans { Id = 2, Tier = "Business Registration (Tow or Impound)", Amount = 10, Duration = 1 }
           );

            modelBuilder.Entity<PlanServices>().HasData(
                new PlanServices { Id = 1, Name = "User Registration.", PlanId = 1 },
                new PlanServices { Id = 2, Name = "Vehicle Search by VIN.", PlanId = 1 },
                new PlanServices { Id = 3, Name = "Access the search results.", PlanId = 1 },
                new PlanServices { Id = 4, Name = "Business registration", PlanId = 2 },
                new PlanServices { Id = 5, Name = "Vehicle Management", PlanId = 2 },
                new PlanServices { Id = 6, Name = "Notification System", PlanId = 2 }
                );

            string[] hash = new string[2];
            string password = "Password@!@$1";
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            // Salt
            hash[0] = Convert.ToBase64String(saltBytes);


            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + hash[0]);
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);
                hash[1] = Convert.ToBase64String(hashBytes);
            }
            // 1:1 Relationship between User and SubRole
            modelBuilder.Entity<User>()
                .HasOne(u => u.SubRoles)
                .WithOne()
                .HasForeignKey<User>(u => u.SubRoleId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascading delete issues

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    //Password = "MDAwMA==",
                    Email = "admin@admin.com",
                    ConfirmedOn = DateTime.Now,
                    FullName = "Super Admin",
                    IsConfirmed = true,
                    PasswordHash = hash[1],
                    PasswordSalt = hash[0],
                    RoleId = (int)RoleEnum.SuperAdmin,
                    SubRoleId = null,
                    AddedById = 1,
                    ModifiedById = 1
                });
             

            modelBuilder.Entity<VehicleColor>().HasData(
            new VehicleColor { Id = 1, Name = "Black" },
            new VehicleColor { Id = 2, Name = "White" },
            new VehicleColor { Id = 3, Name = "Gray" },
            new VehicleColor { Id = 4, Name = "Silver" },
            new VehicleColor { Id = 5, Name = "Red" },
            new VehicleColor { Id = 6, Name = "Blue" },
            new VehicleColor { Id = 7, Name = "Green" },
            new VehicleColor { Id = 8, Name = "Yellow" },
            new VehicleColor { Id = 9, Name = "Orange" },
            new VehicleColor { Id = 10, Name = "Brown" },
            new VehicleColor { Id = 11, Name = "Gold" },
            new VehicleColor { Id = 12, Name = "Beige" },
            new VehicleColor { Id = 13, Name = "Purple" },
            new VehicleColor { Id = 14, Name = "Pink" },
            new VehicleColor { Id = 15, Name = "Turquoise" }
            );
            modelBuilder.Entity<VehicleMake>().HasData(
                new VehicleMake { Id = 1, Name = "Acura" },
                new VehicleMake { Id = 2, Name = "Alfa Romeo" },
                new VehicleMake { Id = 3, Name = "Aston Martin" },
                new VehicleMake { Id = 4, Name = "Audi" },
                new VehicleMake { Id = 5, Name = "Bentley" },
                new VehicleMake { Id = 6, Name = "BMW" },
                new VehicleMake { Id = 7, Name = "Bugatti" },
                new VehicleMake { Id = 8, Name = "Buick" },
                new VehicleMake { Id = 9, Name = "Cadillac" },
                new VehicleMake { Id = 10, Name = "Chevrolet" },
                new VehicleMake { Id = 11, Name = "Chrysler" },
                new VehicleMake { Id = 12, Name = "Dodge" },
                new VehicleMake { Id = 13, Name = "Ferrari" },
                new VehicleMake { Id = 14, Name = "Fiat" },
                new VehicleMake { Id = 15, Name = "Ford" },
                new VehicleMake { Id = 16, Name = "Genesis" },
                new VehicleMake { Id = 17, Name = "GMC" },
                new VehicleMake { Id = 18, Name = "Honda" },
                new VehicleMake { Id = 19, Name = "Hyundai" },
                new VehicleMake { Id = 20, Name = "Infiniti" },
                new VehicleMake { Id = 21, Name = "Jaguar" },
                new VehicleMake { Id = 22, Name = "Jeep" },
                new VehicleMake { Id = 23, Name = "Kia" },
                new VehicleMake { Id = 24, Name = "Lamborghini" },
                new VehicleMake { Id = 25, Name = "Land Rover" },
                new VehicleMake { Id = 26, Name = "Lexus" },
                new VehicleMake { Id = 27, Name = "Lincoln" },
                new VehicleMake { Id = 28, Name = "Lotus" },
                new VehicleMake { Id = 29, Name = "Maserati" },
                new VehicleMake { Id = 30, Name = "Mazda" },
                new VehicleMake { Id = 31, Name = "McLaren" },
                new VehicleMake { Id = 32, Name = "Mercedes-Benz" },
                new VehicleMake { Id = 33, Name = "Mini" },
                new VehicleMake { Id = 34, Name = "Mitsubishi" },
                new VehicleMake { Id = 35, Name = "Nissan" },
                new VehicleMake { Id = 36, Name = "Peugeot" },
                new VehicleMake { Id = 37, Name = "Porsche" },
                new VehicleMake { Id = 38, Name = "Ram" },
                new VehicleMake { Id = 39, Name = "Renault" },
                new VehicleMake { Id = 40, Name = "Rolls-Royce" },
                new VehicleMake { Id = 41, Name = "Saab" },
                new VehicleMake { Id = 42, Name = "Subaru" },
                new VehicleMake { Id = 43, Name = "Suzuki" },
                new VehicleMake { Id = 44, Name = "Tesla" },
                new VehicleMake { Id = 45, Name = "Toyota" },
                new VehicleMake { Id = 46, Name = "Volkswagen" },
                new VehicleMake { Id = 47, Name = "Volvo" }
            );

            modelBuilder.Entity<VehicleModel>().HasData(
            // Acura
            new VehicleModel { Id = 1, Name = "MDX", MakeId = 1 },
            new VehicleModel { Id = 2, Name = "RDX", MakeId = 1 },
            new VehicleModel { Id = 3, Name = "TLX", MakeId = 1 },

            // Alfa Romeo
            new VehicleModel { Id = 4, Name = "Giulia", MakeId = 2 },
            new VehicleModel { Id = 5, Name = "Stelvio", MakeId = 2 },

            // Aston Martin
            new VehicleModel { Id = 6, Name = "DB11", MakeId = 3 },
            new VehicleModel { Id = 7, Name = "Vantage", MakeId = 3 },

            // Audi
            new VehicleModel { Id = 8, Name = "A3", MakeId = 4 },
            new VehicleModel { Id = 9, Name = "A4", MakeId = 4 },
            new VehicleModel { Id = 10, Name = "Q5", MakeId = 4 },
            new VehicleModel { Id = 11, Name = "Q7", MakeId = 4 },

            // Bentley
            new VehicleModel { Id = 12, Name = "Continental GT", MakeId = 5 },
            new VehicleModel { Id = 13, Name = "Bentayga", MakeId = 5 },

            // BMW
            new VehicleModel { Id = 14, Name = "3 Series", MakeId = 6 },
            new VehicleModel { Id = 15, Name = "5 Series", MakeId = 6 },
            new VehicleModel { Id = 16, Name = "X5", MakeId = 6 },
            new VehicleModel { Id = 17, Name = "X7", MakeId = 6 },

            // Bugatti
            new VehicleModel { Id = 18, Name = "Chiron", MakeId = 7 },
            new VehicleModel { Id = 19, Name = "Veyron", MakeId = 7 },

            // Buick
            new VehicleModel { Id = 20, Name = "Enclave", MakeId = 8 },
            new VehicleModel { Id = 21, Name = "Encore", MakeId = 8 },

            // Cadillac
            new VehicleModel { Id = 22, Name = "Escalade", MakeId = 9 },
            new VehicleModel { Id = 23, Name = "XT5", MakeId = 9 },

            // Chevrolet
            new VehicleModel { Id = 24, Name = "Silverado", MakeId = 10 },
            new VehicleModel { Id = 25, Name = "Malibu", MakeId = 10 },
            new VehicleModel { Id = 26, Name = "Camaro", MakeId = 10 },

            // Chrysler
            new VehicleModel { Id = 27, Name = "300", MakeId = 11 },
            new VehicleModel { Id = 28, Name = "Pacifica", MakeId = 11 },

            // Dodge
            new VehicleModel { Id = 29, Name = "Charger", MakeId = 12 },
            new VehicleModel { Id = 30, Name = "Challenger", MakeId = 12 },

            // Ferrari
            new VehicleModel { Id = 31, Name = "488", MakeId = 13 },
            new VehicleModel { Id = 32, Name = "Roma", MakeId = 13 },

            // Fiat
            new VehicleModel { Id = 33, Name = "500", MakeId = 14 },
            new VehicleModel { Id = 34, Name = "Panda", MakeId = 14 },

            // Ford
            new VehicleModel { Id = 35, Name = "F-150", MakeId = 15 },
            new VehicleModel { Id = 36, Name = "Mustang", MakeId = 15 },

            // Genesis
            new VehicleModel { Id = 37, Name = "G70", MakeId = 16 },
            new VehicleModel { Id = 38, Name = "G90", MakeId = 16 },

            // GMC
            new VehicleModel { Id = 39, Name = "Sierra", MakeId = 17 },
            new VehicleModel { Id = 40, Name = "Yukon", MakeId = 17 },

            // Honda
            new VehicleModel { Id = 41, Name = "Civic", MakeId = 18 },
            new VehicleModel { Id = 42, Name = "Accord", MakeId = 18 },

            // Hyundai
            new VehicleModel { Id = 43, Name = "Elantra", MakeId = 19 },
            new VehicleModel { Id = 44, Name = "Santa Fe", MakeId = 19 },

            // Infiniti
            new VehicleModel { Id = 45, Name = "Q50", MakeId = 20 },
            new VehicleModel { Id = 46, Name = "QX80", MakeId = 20 },

            // Jaguar
            new VehicleModel { Id = 47, Name = "F-PACE", MakeId = 21 },
            new VehicleModel { Id = 48, Name = "XE", MakeId = 21 },

            // Jeep
            new VehicleModel { Id = 49, Name = "Wrangler", MakeId = 22 },
            new VehicleModel { Id = 50, Name = "Grand Cherokee", MakeId = 22 },

            // Tesla
            new VehicleModel { Id = 51, Name = "Model S", MakeId = 44 },
            new VehicleModel { Id = 52, Name = "Model 3", MakeId = 44 },
            new VehicleModel { Id = 53, Name = "Model X", MakeId = 44 },
            new VehicleModel { Id = 54, Name = "Model Y", MakeId = 44 },

            // Toyota
            new VehicleModel { Id = 55, Name = "Corolla", MakeId = 45 },
            new VehicleModel { Id = 56, Name = "Camry", MakeId = 45 },
            new VehicleModel { Id = 57, Name = "RAV4", MakeId = 45 },

            // Volkswagen
            new VehicleModel { Id = 58, Name = "Golf", MakeId = 46 },
            new VehicleModel { Id = 59, Name = "Passat", MakeId = 46 },

            // Volvo
            new VehicleModel { Id = 60, Name = "XC90", MakeId = 47 },
            new VehicleModel { Id = 61, Name = "S60", MakeId = 47 }
            );



        }
    }
}
