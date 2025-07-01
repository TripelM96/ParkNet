using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ParkNet.Data.Entities;

public class ParkingLot
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Column(TypeName = "varchar(128)")]
    public string Location { get; set; }
    
  

}
