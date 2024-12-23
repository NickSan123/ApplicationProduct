using ApplicationProduct.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationProduct.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(
                    "Data Source=app.db",
                    b => b.MigrationsAssembly("ApplicationProduct.WebApi"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(256);

                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasMany(u => u.Sessions)
                      .WithOne(s => s.User)
                      .HasForeignKey(s => s.UserId);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Token).IsRequired().HasMaxLength(500);
                entity.Property(s => s.CreatedAt).IsRequired();
                entity.Property(s => s.ExpiresAt).IsRequired();
            });
        }
    }

}
