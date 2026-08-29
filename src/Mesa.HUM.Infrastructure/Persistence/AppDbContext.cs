namespace Mesa.HUM.Infrastructure.Persistence
{
    using System.Diagnostics.CodeAnalysis;
    using Mesa.HUM.Domain.Profiles;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
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