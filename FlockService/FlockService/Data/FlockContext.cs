using FlockService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Data
{
    public class FlockContext : DbContext
    {
        public FlockContext(DbContextOptions<FlockContext> opt) : base(opt)
        {

        }

        //dit mapt de model Command aan een database tabel Commands
        public DbSet<Flock> Flocks { get; set; }
        //en de Platforms
        public DbSet<EggType> EggTypes { get; set; }

        //in principe is bovenstaande voldoende, EF regelt de relaties
        //Les Jackson neemt echter het zekere voor het onzekere en
        //declared hieronder de boel nog even expliciet.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<EggType>()
                .HasMany(p => p.Flocks)
                .WithOne(p => p.EggType!)
                .HasForeignKey(p => p.EggTypeId);

            modelBuilder
            .Entity<Flock>()
            .HasOne(p => p.EggType)
            .WithMany(p => p.Flocks)
            .HasForeignKey(p => p.EggTypeId);
        }



    }
}
