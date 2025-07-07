using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkNet.Data;
using ParkNet.Data.Entities;
using ParkNet.Data.Entities.Enum;
using ParkNet.Services;


namespace ParkNet.Pages.BalanceTransactions
{
    public class CreateModel : PageModelBase
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public CreateModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }       

        public IActionResult OnGet()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");

            return Page();
        }

        [BindProperty]
        public BalanceTransaction BalanceTransaction { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}          
            

            BalanceTransaction.Date = DateTime.UtcNow;
            BalanceTransaction.UserId = this.UserId;

            _context.BalanceTransactions.Add(BalanceTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
