using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkNet.Data.Entities;

public class ParkingSession
{
    public int Id { get; set; }

    public IdentityUser User { get; set; }
    public string UserId { get; set; }
    public ParkingSpot ParkingSpot { get; set; }
    public int ParkingSpotId { get; set; }

    [Column(TypeName = "Datetime2(0)")]
    public DateTime Entrada { get; set; }

    [Column(TypeName = "Datetime2(0)")]
    public DateTime? Saida { get; set; }

    [NotMapped]
    public TimeSpan? Duracao => Saida.HasValue ? Saida - Entrada : null;

    [Column(TypeName = "decimal(10,2)")]
    public decimal? ValorPago { get; set; }
}
