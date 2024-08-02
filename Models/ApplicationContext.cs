using Microsoft.EntityFrameworkCore;

namespace TestWebApp_Autopiter.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Filial> Filials { get; set; } = null!;
        public DbSet<PrintingDevice> PrintingDevices { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<PrintingDeviceInFilial> PrintingDevicesInFilials { get; set; } = null!;
        public DbSet<ConnectionType> ConnectionTypes { get; set; } = null!;
        public DbSet<PrintJob> PrintJobs { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Filial>().HasData(
                    new Filial { Id = 1, Name = "Тридевятое царство", Location = "Тридевятое царство" },
                    new Filial { Id = 2, Name = "Дремучий лес", Location = "Дремучий лес" },
                    new Filial { Id = 3, Name = "Луна", Location = "Луна" }
            );
            modelBuilder.Entity<Employee>().HasData(
                    new Employee { Id = 1, Name = "Царь", FilialId = 1 },
                    new Employee { Id = 2, Name = "Яга", FilialId = 2 },
                    new Employee { Id = 3, Name = "Копатыч", FilialId = 3 },
                    new Employee { Id = 4, Name = "Добрыня", FilialId = 1 },
                    new Employee { Id = 5, Name = "Кощей", FilialId = 3 },
                    new Employee { Id = 6, Name = "Лосяш", FilialId = 3 }
            );
            modelBuilder.Entity<ConnectionType>().HasData(
                    new ConnectionType { Id = 1, Name = "Локальное" },
                    new ConnectionType { Id = 2, Name = "Сетевое" }
            );
            modelBuilder.Entity<PrintingDevice>().HasData(
                    new PrintingDevice { Id = 1, Name = "Папирус", ConnectionTypeId = 1 },
                    new PrintingDevice { Id = 2, Name = "Бумага", ConnectionTypeId = 1 },
                    new PrintingDevice { Id = 3, Name = "Камень", ConnectionTypeId = 2 }
            );
            modelBuilder.Entity<PrintingDeviceInFilial>().HasData(
                    new PrintingDeviceInFilial { Id = 1, Name = "Дворец", FilialId = 1, NumberInFilial = 1, IsDefault = true, PrintingDeviceId = 1 },
                    new PrintingDeviceInFilial { Id = 2, Name = "Конюшни", FilialId = 1, NumberInFilial = 2, IsDefault = false, PrintingDeviceId = 2 },
                    new PrintingDeviceInFilial { Id = 3, Name = "Оружейная", FilialId = 1, NumberInFilial = 3, IsDefault = false, PrintingDeviceId = 2 },
                    new PrintingDeviceInFilial { Id = 4, Name = "Кратер", FilialId = 3, NumberInFilial = 1, IsDefault = true, PrintingDeviceId = 3 },
                    new PrintingDeviceInFilial { Id = 5, Name = "Избушка", FilialId = 2, NumberInFilial = 3, IsDefault = false, PrintingDeviceId = 2 },
                    new PrintingDeviceInFilial { Id = 6, Name = "Топи", FilialId = 2, NumberInFilial = 2, IsDefault = true, PrintingDeviceId = 1 }
            );
        }
    }
}