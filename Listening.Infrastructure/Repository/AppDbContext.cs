using Infrastructure.SharedKernel;
using Listening.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure.Repository
{
    public class AppDbContext : BaseDbContext
    {
        // 不能直接用DbContextOptions options

        public AppDbContext(DbContextOptions<BaseDbContext> options )
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Kind> Kinds { get; set; }
        public DbSet<Album> Albums { get; set; }
    }
}
