using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data.Entities;
using System.Reflection.Emit;

namespace ParkNet.Data;

public class ParkNetDbContext : IdentityDbContext
{
    public DbSet<ParkingLot> ParkingLots { get; set; }
    public DbSet<Floor> Floors { get; set; }
    public DbSet<ParkingSpot> ParkingSpots  { get; set; }
    public DbSet<Subscription> Subscriptions  { get; set; }
    public DbSet<BalanceTransaction> BalanceTransactions { get; set; }
    public DbSet<ParkingRate> ParkingRates { get; set; }
    public DbSet<ParkingSession> ParkingSessions { get; set; }
    public ParkNetDbContext(DbContextOptions<ParkNetDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Add addtional EF configurations using Fluent API

        builder.Entity<ParkingSpot>()
     .HasOne<IdentityUser>(p => p.ReservedForUser)
     .WithMany()
     .HasForeignKey(p => p.ReservedForUserId)
     .OnDelete(DeleteBehavior.SetNull);
    }
}
