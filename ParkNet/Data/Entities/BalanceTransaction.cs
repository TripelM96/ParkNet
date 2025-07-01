using Microsoft.AspNetCore.Identity;
using ParkNet.Data.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace ParkNet.Data.Entities;

public class BalanceTransaction
{
    public int Id { get; set; }

    public IdentityUser User { get; set; }
    public string UserId { get; set; }

    [Column(TypeName = "Datetime2(0)")]
    public DateTime Date { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Ammount { get; set; }
    public TransactionType TransactionType { get; set; }
    public string Description { get; set; }

}