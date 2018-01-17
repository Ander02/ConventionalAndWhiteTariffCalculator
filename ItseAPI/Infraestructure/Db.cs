using Microsoft.EntityFrameworkCore;
using ConventionalAndWhiteTariffCalculator.Domain;

namespace ConventionalAndWhiteTariffCalculator.Infraestructure
{
    public class Db : DbContext
    {
        #region Tables
        public DbSet<Equipment> Equipament { get; set; }
        public DbSet<Tariff> Tariff { get; set; }
        public DbSet<PowerDistribuitor> PowerDistribuitor { get; set; }
        #endregion

        public Db(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder m)
        {
            m.Entity<Equipment>().ToTable(nameof(Equipment));
            m.Entity<Tariff>().ToTable(nameof(Tariff));
            m.Entity<PowerDistribuitor>().ToTable(nameof(PowerDistribuitor));

            m.Entity<Tariff>().HasOne(t => t.PowerDistribuitor).WithMany(c => c.Tariffs).HasForeignKey(t => t.PowerDistribuitorId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
