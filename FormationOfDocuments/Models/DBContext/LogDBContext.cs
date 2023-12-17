using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FormationOfDocuments.Models.DBContext
{
    public class LogDBContext : DbContext
    {
        public DbSet<LogEvent> ParseOtherResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var hostSQL = ConfigurationManager.AppSettings["hostLogDB"];
            var userSQL = ConfigurationManager.AppSettings["userSQL"];
            var passSQL = ConfigurationManager.AppSettings["passSQL"];
            var database = ConfigurationManager.AppSettings["databaseLogDB"];

            if (!optionsBuilder.IsConfigured)
            {
                //Подключаемся к базе данных
                optionsBuilder.UseSqlServer($"Server={hostSQL};Database={database};Trusted_connection=True;User ID={userSQL};Password={passSQL}");
            }
        }
    }
}
