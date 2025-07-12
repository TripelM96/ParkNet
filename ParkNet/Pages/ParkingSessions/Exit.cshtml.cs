using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
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
        session.ParkingSpot.Occupy = false;

        // Obter subscrição ativa
        var subscription = await _context.Subscriptions
            .Where(s => s.UserId == this.UserId && s.Active)
            .OrderByDescending(s => s.EndTime)
            .FirstOrDefaultAsync();

        var hasValidSubscription = subscription != null &&
                                subscription.IncialDate <= DateTime.UtcNow &&
                                subscription.EndTime >= DateTime.UtcNow;


        if (hasValidSubscription)
        {
            // Subscrição válida: não paga e lugar reservado
            session.ValorPago = 0.0m;
            session.ParkingSpot.Reserved = true;
            session.ParkingSpot.ReservedForUserId = this.UserId;
            
        }        
        else
        {
            var duracao = session.Saida.Value - session.Entrada;
            var precoPorMinuto = 0.05m;

            // para testes 
            //session.Entrada = session.Entrada.AddMinutes(-30);
            session.ValorPago = Math.Round((decimal)duracao.TotalMinutes * precoPorMinuto, 2);


            _context.BalanceTransactions.Add(new BalanceTransaction
            {
                UserId = session.UserId,
                Ammount = -session.ValorPago.Value,
                Date = DateTime.UtcNow,
                Description = $"Estacionamento em {session.ParkingSpot.Code}"
            });


            // Se o lugar estava reservado para este utilizador, mas a subscrição expirou, remover a reserva
            if (session.ParkingSpot.Reserved &&
             session.ParkingSpot.ReservedForUserId == this.UserId)
            {
                session.ParkingSpot.Reserved = false;
                session.ParkingSpot.ReservedForUserId = null;

            }
        }
    

        await _context.SaveChangesAsync();

        TempData["Mensagem"] = $"Sessão terminada. Total: {session.ValorPago:C}";
        return RedirectToPage("./Index");
    }
}