using bus_tickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bus_tickets
{
    internal class Database : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Flight> Flights { get; set; } = null!;

        public DbSet<Ticket> Tickets { get; set; } = null!;

        public Database(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        { }

        //Определение конфигурации моделей базы данных
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                string? tableName = entity.GetTableName() ?? "default";
                entity.SetTableName(tableName[..1].ToLower() + tableName[1..]);

                foreach (IMutableProperty property in entity.GetProperties())
                {
                    string propertyName = property.Name[..1].ToLower() + property.Name[1..];
                    property.SetColumnName(propertyName);
                }
            }

            _ = modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                Login = "admin",
                Password = "$2a$11$aXJvnMfl8ZOMDmV.blRHR.zJ4xQAUb0fa1jN5XE/KwqfR.xSjFp0y",
                IsAdmin = true,
                IsActive = true
            });
        }
    }
}
