using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkNet.Data;
using ParkNet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkNet.Data.Entities.Enum;
using ParkNet.Services;


namespace ParkNet.Pages.Subscriptions
{
    public class CreateModel : PageModelBase
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;
        private readonly BalanceTransactionsServices _balance;

        public CreateModel(ParkNet.Data.ParkNetDbContext context, BalanceTransactionsServices balance)
        {
            _context = context;
            _balance = balance;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ParkingLotList = await _context.ParkingLots
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToListAsync();

            SubscriptionTypeList = Enum.GetValues(typeof(SubscriptionType))
            .Cast<SubscriptionType>()
            .Select(t => new SelectListItem
            {
                Value = t.ToString(),
                Text = t.ToString()
            }).ToList();

            return Page();
        }     

        [BindProperty]
        public Subscription Subscription { get; set; }
        
        public List<SelectListItem> ParkingLotList { get; set; }
        public List<SelectListItem> SubscriptionTypeList { get; set; }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {


            var userId = this.UserId;

            //Valores subscription
            var values = new Dictionary<SubscriptionType, decimal>
            {
                { SubscriptionType.Monthly,     30.00m },
                { SubscriptionType.Quarterly, 85.00m },
                { SubscriptionType.Semesterly,  160.00m },
                { SubscriptionType.Annualy,      300.00m }
            };

           
            var cost = values[Subscription.SubscriptionType];


            //TODO: Acrescentar envio de email para utilizador 
            //var currentBalance = await _balance.GetBalanceAsync(userId);
            //if (CurrentBalance < cost)
            //{
            //    ModelState.AddModelError("", "Saldo a negativo. Efetue um carregamento para efetuar a compra");
            //    return Page();
            //}

            _context.BalanceTransactions.Add(new BalanceTransaction
            {
                UserId = userId,
                Ammount = -cost,
                Date = DateTime.UtcNow,
                Description = $"Compra de avença {Subscription.SubscriptionType}"
            });

            Subscription.UserId = userId;           
            Subscription.EndTime = Subscription.SubscriptionType switch
            {
                SubscriptionType.Monthly => Subscription.IncialDate.AddMonths(1),
                SubscriptionType.Quarterly => Subscription.IncialDate.AddMonths(3),
                SubscriptionType.Semesterly => Subscription.IncialDate.AddMonths(6),
                SubscriptionType.Annualy => Subscription.IncialDate.AddYears(1),
                _ => Subscription.IncialDate.AddMonths(1)
            };
            Subscription.Active = true;

            _context.Subscriptions.Add(Subscription);

            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Avença comprada com sucesso!";
            return RedirectToPage("./Index");

        }
    }
}

