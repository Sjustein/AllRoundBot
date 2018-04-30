using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AllRound.Resources.Database
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Stone> Stones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            string DbLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.0", @"Data\");
            Options.UseSqlite($"Data Source={DbLocation}Database.sqlite");
        }
    }
}
