using EFCoreWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreWebApp
{
    public class EFCoreSampleDbContext : DbContext
    {
        public virtual DbSet<Code> Codes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public EFCoreSampleDbContext()
        {
        }

        public EFCoreSampleDbContext(DbContextOptions<EFCoreSampleDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Code>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Codes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Codes_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });
        }
    }
}
