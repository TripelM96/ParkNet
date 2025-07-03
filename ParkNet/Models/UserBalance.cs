namespace ParkNet.Services;

public partial class BalanceTransactionsServices
{
    public class UserBalance
    {
        public string UserId { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
