using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeManagement.Server.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext( DbContextOptions options ) : base( options ) { }

    protected override void OnConfiguring( DbContextOptionsBuilder options )
    {
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating( ModelBuilder builder )
    {
        base.OnModelCreating( builder );
    
        EntityTypeBuilder<Family> family = builder.Entity<Family>();
        EntityTypeBuilder<User> user = builder.Entity<User>();
        
        family.HasMany( x => x.Users );
    
        user.HasOne( x => x.Family );
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Family> Families { get; set; }
}