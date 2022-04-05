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

        public DbSet<TipoDeTransacao> TiposDeTransacao { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = $"Server=mssql-server, 1433;Initial Catalog=bycoders;User ID=SA;Password=senhadificil.123";

            optionsBuilder.UseSqlServer(connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transacao>().HasOne(x => x.TipoDeTransacao).WithMany().IsRequired();
            modelBuilder.Entity<Transacao>().HasOne(x => x.Cliente).WithMany().IsRequired();
            modelBuilder.Entity<Transacao>().HasOne(x => x.Loja).WithMany().IsRequired();


            modelBuilder.Entity<TipoDeTransacao>().HasData(
               new TipoDeTransacao
               {
                   Id = 1,
                   Codigo = 1,
                   Descricao = "Débito",
                   Entrada = true,
               }, new TipoDeTransacao
               {
                   Id = 2,
                   Codigo = 2,
                   Descricao = "Boleto",
                   Entrada = false,
               }, new TipoDeTransacao
               {
                   Id = 3,
                   Codigo = 3,
                   Descricao = "Financiamento",
                   Entrada = false,
               }, new TipoDeTransacao
               {
                   Id = 4,
                   Codigo = 4,
                   Descricao = "Crédito",
                   Entrada = true,
               }, new TipoDeTransacao
               {
                   Id = 5,
                   Codigo = 5,
                   Descricao = "Recebimento Empréstimo",
                   Entrada = true,
               }, new TipoDeTransacao
               {
                   Id = 6,
                   Codigo = 6,
                   Descricao = "Vendas",
                   Entrada = true,
               }, new TipoDeTransacao
               {
                   Id = 7,
                   Codigo = 7,
                   Descricao = "Recebimento TED",
                   Entrada = true,
               }, new TipoDeTransacao
               {
                   Id = 8,
                   Codigo = 8,
                   Descricao = "Recebimento DOC",
                   Entrada = true,
               }, new TipoDeTransacao
               {
                   Id = 9,
                   Codigo = 9,
                   Descricao = "Aluguel",
                   Entrada = false,
               }
           );
        }
    }
}
