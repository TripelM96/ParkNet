using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.ParkingSessions;

public class ExitModel : PageModelBase
{
    private readonly ParkNetDbContext _context;

    public ExitModel(ParkNetDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public ParkingSession ParkingSession { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ParkingSession = await _context.ParkingSessions
            .Include(p => p.ParkingSpot)
            .FirstOrDefaultAsync(p =>
                p.Id == id &&
                p.UserId == this.UserId &&
                p.Saida == null);

        if (ParkingSession == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var session = await _context.ParkingSessions
            .Include(s => s.ParkingSpot)
            .FirstOrDefaultAsync(s =>
                s.Id == id &&
                s.UserId == this.UserId &&
                s.Saida == null);

        if (session == null)
            return NotFound();

        session.Saida = DateTime.UtcNow;
        var duracao = session.Saida.Value - session.Entrada;
        var precoPorMinuto = 0.05m;

        // para testes 
        //session.Entrada = session.Entrada.AddMinutes(-90);
        session.ValorPago = Math.Round((decimal)duracao.TotalMinutes * precoPorMinuto, 2);

        session.ParkingSpot.Occupy = false;

        _context.BalanceTransactions.Add(new BalanceTransaction
        {
            UserId = session.UserId,
            Ammount = -session.ValorPago.Value,
            Date = DateTime.UtcNow,
            Description = $"Estacionamento em {session.ParkingSpot.Code}"
        });

        await _context.SaveChangesAsync();

        TempData["Mensagem"] = $"Sessão terminada. Total: {session.ValorPago:C}";
        return RedirectToPage("./Index");
    }
}
