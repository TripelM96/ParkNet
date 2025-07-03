using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ParkNet.Services;

namespace ParkNet.Pages.BalanceTransactions;

[Authorize]
public class IndexModel : PageModelBase
{
    private readonly ParkNetDbContext _context;
    private readonly BalanceTransactionsServices _balanceServices;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ParkNetDbContext context, BalanceTransactionsServices balanceServices, ILogger<IndexModel> logger)
    {
        _context = context;
        _balanceServices = balanceServices;
        _logger = logger;
    }

    public IList<BalanceTransaction> BalanceTransaction { get;set; } = default!;

    public decimal Balance { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            BalanceTransaction = await _balanceServices.GetAllMovementsAsync(this.UserId);
            Balance = await _balanceServices.GetBalanceAsync(this.UserId);

            _logger.LogInformation("Movements and balance retrieved successfully for user {UserId}", this.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving movements or balance for user {UserId}", this.UserId);
        }
    }
}
