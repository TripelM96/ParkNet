using System.ComponentModel.DataAnnotations.Schema;

namespace ParkNet.Data.Entities;

public class ParkingRate
{
    public int Id { get; set; }
    public ParkingLot ParkingLot { get; set; }
    public int ParkingLotId { get; set; }
    public int MinutesGap { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }
}
