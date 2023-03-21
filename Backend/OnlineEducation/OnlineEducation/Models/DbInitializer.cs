using Microsoft.EntityFrameworkCore;

namespace OnlineEducation.Models
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            var adminId = Guid.NewGuid();
            modelBuilder.Entity<Account>().HasData(
                   new Account()
                   {
                       Id = adminId,
                       CreationTime = DateTime.Now,
                       CreatorId = adminId,
                       IsDelete = false,
                       UserName = "admin",
                       Password = Account.HashPashword("1q2w3E*"),
                       Role = "Admin",
                       Email = "admin@gmail.com",
                       IsActive = true,
                       IsVerification = true,
                       LastLogOnDate = DateTime.Now,
                       FirstName = "Chau",
                       LastName = "Toan",
                       PhoneNumber = "1234567890",
                       Gender = Shared.Gender.Male
                   }
            );

            modelBuilder.Entity<Role>().HasData(
                  new Role()
                  {
                      Id = Guid.NewGuid(),
                      CreationTime = DateTime.Now,
                      CreatorId = adminId,
                      IsDelete = false,
                      RoleName = "admin",
                      Description = "Admintrative Function"
                  },
                  new Role()
                  {
                      Id = Guid.NewGuid(),
                      CreationTime = DateTime.Now,
                      CreatorId = adminId,
                      IsDelete = false,
                      RoleName = "user",
                      Description = "Normal User"
                  }
             );
        }
    }
}
