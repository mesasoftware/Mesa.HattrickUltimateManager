namespace Mesa.HUM.Infrastructure.Persistence
{
    using Mesa.HUM.Domain.Profiles;
    using Microsoft.EntityFrameworkCore;

    internal class AppDbContext : DbContext
    {
        public AppDbContext ( DbContextOptions options ) : base ( options )
        {
        }

        protected AppDbContext ( )
        {
        }

        internal DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            base.OnModelCreating ( modelBuilder );

            modelBuilder.ApplyConfigurationsFromAssembly ( typeof ( AppDbContext ).Assembly );
        }
    }
}