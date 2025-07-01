using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data.Entities;

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
}
