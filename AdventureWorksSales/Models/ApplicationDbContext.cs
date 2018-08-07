using AdventureWorksSales.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksSales.Models
{
    public class ApplicationDbContext : AdventureWorks2014Context
    {
        private readonly string ConnectionString;

        public ApplicationDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}
