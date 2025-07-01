using Microsoft.AspNetCore.Identity;
using ParkNet.Data.Entities.Enum;

namespace ParkNet.Data.Entities;

public class Subscription
{
    public int Id { get; set; }
    public IdentityUser User { get; set; }
    public string UserId { get; set; }
    public ParkingLot ParkingLot { get; set; }
    public int ParkingLotId { get; set; }  
    public SubscriptionType SubscriptionType { get; set; }
    public DateTime IncialDate { get; set; }
    public DateTime EndTime { get; set; }
    public bool Active { get; set; }
}
