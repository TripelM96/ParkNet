
namespace ParkNet.Services;

public partial class BalanceTransactionsServices
{
    private readonly ParkNetDbContext _ctx;

    public BalanceTransactionsServices(ParkNetDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<BalanceTransaction>> GetAllMovementsAsync(string userId)
    {
        var movements = await _ctx.BalanceTransactions
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.Date)
            .ToListAsync();

        return movements;
    }
    public async Task<List<UserBalance>> GetBalancesForAllUsersAsync()
    {
       

        var balances = _ctx.BalanceTransactions
             .GroupBy(m => m.UserId)
             .Select(g => new UserBalance { UserId = g.Key, TotalBalance = g.Sum(m => m.Ammount) });

        return balances.ToList();
    }
    public async Task<decimal> GetBalanceAsync(string userId)
    {
        return await _ctx.BalanceTransactions
            .Where(m => m.UserId == userId)
            .SumAsync(m => m.Ammount);
    }
    public async Task Add(List<BalanceTransaction> movements)
    {
        _ctx.BalanceTransactions.AddRange(movements);
        await _ctx.SaveChangesAsync();
    }

}