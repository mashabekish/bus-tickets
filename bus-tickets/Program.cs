using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using static bus_tickets.Constants;

namespace bus_tickets
{
    internal class Program : IDesignTimeDbContextFactory<Database>
    {
        private static void Main()
        {
            try
            {
                Database database = new Program().CreateDbContext();
                database.Database.Migrate();

                Handler handler = new(database);
                handler.Authorize();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    ERROR: " + ex.Message);
                Console.ResetColor();
                Environment.Exit(1);
            }
        }

        //Создание базы данных и добавление миграций
        public Database CreateDbContext(string[]? args = null)
        {
            string connetionString = $"Server={host};Port={port};UserId={user};Password={password};Database={database};charset=utf8;";

            DbContextOptionsBuilder<Database> optionsBuilder = new();

            _ = optionsBuilder.UseMySql(connetionString,
                new MySqlServerVersion(new Version(8, 0, 28)),
                options =>
                {
                    _ = options.EnableStringComparisonTranslations();
                    _ = options.MigrationsHistoryTable("migrations");
                    _ = options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(1), null);
                });

            return new Database(optionsBuilder.Options);
        }
    }
}
