using HarzRollerz.MVC.Models.Collections;
using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.Entities;
using HarzRollerz.MVC.Models.Scores;
using HarzRollerz.MVC.Models.SingingFeatures;
using Microsoft.EntityFrameworkCore;

namespace HarzRollerz.MVC.Data
{
    public class DataContext : DbContext
    {
        public DbSet<SingingFeature> SingingFeatures { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<EvaluatedFeature> EvaluatedFeatures { get; set; }
        public DbSet<Breeder> Breeders { get; set; }
        public DbSet<Canary> Canaries { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Cage> Cages { get; set; }
        public DbSet<Score> Scores { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Score>()
                .HasOne(s => s.EvaluatedFeature)
                .WithMany(ef => ef.Scores)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Cage>()
                .HasOne(c => c.Canary)
                .WithMany(c => c.Cages)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
