using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkNet.Services;

namespace ParkNet.Pages.BalanceTransactions
{
    
    public class WithdrawModel : PageModelBase
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;
        private readonly BalanceTransactionsServices _balanceServices;
        public WithdrawModel(ParkNetDbContext context, BalanceTransactionsServices balanceServices)
        {
            _context = context;
            _balanceServices = balanceServices;
        }

        public IActionResult OnGet()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");

            return Page();
        }

        [BindProperty]
        public BalanceTransaction BalanceTransaction { get; set; }
        public decimal Balance { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var withdraw = BalanceTransaction.Ammount;

            // Verifica saldo atual
            var currentBalance = await _balanceServices.GetBalanceAsync(this.UserId);
            if (withdraw > currentBalance)
            {
                ModelState.AddModelError(string.Empty, "Saldo insuficiente.");
                return Page();
            }

            // Verifica se o carro está estacionado
            var hasCarParked = await _context.ParkingSessions
            .AnyAsync(s => s.UserId == this.UserId && s.Saida == null);

            var hasActiveSubscription = await _context.Subscriptions
            .AnyAsync(s => s.UserId == this.UserId);

            if (hasCarParked == true && hasActiveSubscription == false)
            {
                ModelState.AddModelError(string.Empty, "A viatura encontra se estacionada. Por favor proceda à saida para efetuar o levantamento");
                return Page();
            }
           
            
            var transaction = (new BalanceTransaction
            {
                UserId = this.UserId,
                Ammount = -withdraw,
                Date = DateTime.UtcNow,
                Description = "Withdraw"
            });

            _context.BalanceTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            Balance = await _balanceServices.GetBalanceAsync(this.UserId);

            return RedirectToPage("./Index");
        }
    }
}

