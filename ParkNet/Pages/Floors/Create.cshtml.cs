using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkNet.Data;
using ParkNet.Data.Entities;
using ParkNet.Data.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkNet.Pages.Floors
{
    public class CreateModel : PageModelBase
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public CreateModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            ViewData["ParkingLotId"] = new SelectList(_context.ParkingLots, "Id", "Name");
            ParkingLotList = await _context.ParkingLots
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToListAsync();
            return Page();
        }

        [BindProperty]
        public Floor Floor { get; set; } = default!;
        [BindProperty]
        public int ParkingLotId { get; set; }

        [BindProperty]
        public IFormFile LayoutFile { get; set; }

        public List<SelectListItem> ParkingLotList { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            if (LayoutFile == null || LayoutFile.Length == 0)
            {
                ModelState.AddModelError("", "Ficheiro inválido.");
                return Page();
            }

            var parkingLot = await _context.ParkingLots.FindAsync(ParkingLotId);
            if (parkingLot == null)
            {
                ModelState.AddModelError("", "Parque não encontrado.");
                return Page();
            }

            // Leitura do ficheiro
            using var reader = new StreamReader(LayoutFile.OpenReadStream());
            var lines = new List<string>();
            var allLines = await reader.ReadToEndAsync();

            // separa por piso
            var pisosBrutos = allLines.Split(Environment.NewLine + Environment.NewLine);

            // Percorre cada piso
            for (int pisoIndex = 0; pisoIndex < pisosBrutos.Length; pisoIndex++)
            {
                /* 
                 * Environment.NewLine	A quebra de linha correta para o sistema operativo atual
                 * .Split(Environment.NewLine, ...)    Divide o texto linha a linha
                 *  RemoveEmptyEntries Ignora linhas em branco
                */
                var rawLines = pisosBrutos[pisoIndex]
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                var floor = new Floor
                {
                    Name = $"Piso {pisoIndex}",
                    Order = pisoIndex,
                    ParkingLotId = parkingLot.Id,
                    LayoutRaw = string.Join('\n', rawLines),
                    Spot = new List<ParkingSpot>()
                };

                // Identifica onde esta o 'C' e o 'M'
                for (int row = 0; row < rawLines.Count; row++)
                {
                    var line = rawLines[row];
                    for (int col = 0; col < line.Length; col++)
                    {
                        char c = line[col];
                        if (c == 'C' || c == 'M')
                        {
                            var type = c == 'C' ? VehicleType.Carro : VehicleType.Moto;
                            var code = GerarCodigo(row, col); // ex: A1, B2

                            floor.Spot.Add(new ParkingSpot
                            {
                                Code = code,
                                Type = type,
                                Occupy = false,
                                Reserved = false
                            });
                        }
                    }
                }
                _context.Floors.Add(floor);
            }
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Layout importado com sucesso!";

            return RedirectToPage("./Index");
        }
        private string GerarCodigo(int row, int col)
        {
            // Converte row em letras (A, B, ..., Z, AA, AB...)
            var letra = GetExcelColumnName(row + 1);
            return $"{letra}{col + 1}";
        }

        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";
            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }
            return columnName;
        }

    }
}

