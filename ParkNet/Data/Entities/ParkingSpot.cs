using ParkNet.Data.Entities.Enum;

namespace ParkNet.Data.Entities;

public class ParkingSpot
{
    public int Id { get; set; }
    public Floor Floor { get; set; }
    public int FloorId { get; set; }
    public VehicleType Type { get; set; }
    public string Code { get; set; }
    public bool Occupy { get; set; }
    public bool Reserved { get; set; }
    public string ReservedForUserId { get; set; }

}
