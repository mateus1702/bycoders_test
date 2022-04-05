using lojaService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace lojaService
{
    public class LojaContext : DbContext
    {
        public DbSet<Loja> Lojas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = $"Server=mssql-server, 1433;Initial Catalog=bycoders;User ID=SA;Password=senhadificil.123";

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Loja>().HasKey(x => x.Id);
        }
    }
}
