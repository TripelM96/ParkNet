using Microsoft.AspNetCore.Identity;

namespace ParkNet.Data.Entities;

public class Floor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; } // ordenar os pisos
    public string LayoutRaw { get; set; }   
    public ParkingLot ParkingLot{ get; set; }
    public int ParkingLotId { get; set; }   

    public List<ParkingSpot> Spot { get; set; }
}
