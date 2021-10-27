using Commander.Models;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander
{
    public class CommandContext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> opt) : base(opt)
        {

        }

        //dit mapt de model Command aan een database tabel Commands
        public DbSet<Command> Commands { get; set; }
        //en de Platforms
        public DbSet<Platform> Platforms { get; set; }

        //in principe is bovenstaande voldoende, EF regelt de relaties
        //Les Jackson neemt echter het zekere voor het onzekere en
        //declared hieronder de boel nog even expliciet.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Platform>()
                .HasMany(p => p.Commands)
                .WithOne(p => p.Platform!)
                .HasForeignKey(p => p.PlatformId);

            modelBuilder
            .Entity<Command>()
            .HasOne(p => p.Platform)
            .WithMany(p => p.Commands)
            .HasForeignKey(p => p.PlatformId);
        }



    }
}
