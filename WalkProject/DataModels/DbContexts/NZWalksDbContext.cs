using Microsoft.EntityFrameworkCore;
using WalkProject.DataModels.Entities;

namespace WalkProject.DataModels.DbContexts
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<WalkCategory> WalkCategories { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many relationship between Walk and Category
            modelBuilder.Entity<WalkCategory>()
                .HasKey(wt => new { wt.WalkId, wt.CategoryId });
            modelBuilder.Entity<WalkCategory>()
                .HasOne(wt => wt.Walk)
                .WithMany(w => w.WalkCategories)
                .HasForeignKey(wt => wt.WalkId);
            modelBuilder.Entity<WalkCategory>()
                .HasOne(wt => wt.Category)
                .WithMany(t => t.WalkCategories)
                .HasForeignKey(wt => wt.CategoryId);

            // One to One relationship between Walk and Difficulty
            modelBuilder.Entity<Walk>()
                .HasOne(w => w.Difficulty);

            // One to One relationship between Walk and Region
            modelBuilder.Entity<Walk>()
                .HasOne(w => w.Region);


            modelBuilder.Entity<Role>().HasData(
                new { Id = Guid.Parse("9048e7ee-b63b-44ee-a93a-79cf1a904d86"), Name = "admin", Description = "root admin", CreatedAt = new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc) },
                new { Id = Guid.Parse("222715a2-dbe8-47ba-85d3-b2948be8a55c"), Name = "staff", Description = "nhân viên", CreatedAt = new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc) },
                new { Id = Guid.Parse("bd8e2129-6568-4c38-b15a-b9fbdb64dc31"), Name = "user", Description = "chủ cửa hàng", CreatedAt = new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }


}
