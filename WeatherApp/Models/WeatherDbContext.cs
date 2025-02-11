using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    internal class WeatherDbContext : DbContext
    {
        public DbSet<WeatherData> TempData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=WeatherApp; Trusted_Connection=True; TrustServerCertificate=True;");
        }
    }
}
