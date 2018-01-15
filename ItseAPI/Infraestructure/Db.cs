using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading;
using ConventionalAndWhiteTariffCalculator.Domain;

namespace ConventionalAndWhiteTariffCalculator.Infraestructure
{
    public class Db : DbContext
    {
        #region Tables
        public DbSet<Equipament> Equipament { get; set; }
        public DbSet<Tariff> Tariff { get; set; }
        public DbSet<Concessionary> Concessionary { get; set; }
        #endregion

        public Db(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder m)
        {
            m.Entity<Equipament>().ToTable(nameof(Equipament));
            m.Entity<Tariff>().ToTable(nameof(Tariff));
            m.Entity<Concessionary>().ToTable(nameof(Concessionary));

            m.Entity<Tariff>().HasOne(t => t.Concessionary).WithMany(c => c.Tariffs).HasForeignKey(t => t.ConcessionaryId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
