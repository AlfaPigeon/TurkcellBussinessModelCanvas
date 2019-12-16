using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class AddPartnerModel : PageModel
    {
        private readonly Canvas_Context _context;

        public AddPartnerModel(Canvas_Context context)
        {
            _context = context;
        }
        public void OnGet()
        {

        }

        public List<Tuple<Guid, string>> GetAllFirms()
        {
            var a = (from firm in _context.Firms where firm.Id != Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d") select firm);
            var b = (from firm in _context.Firms
                     where firm.Id != Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")
                     join
                     part in _context.R_Partners on firm.Id equals part.FirmID
                     select firm);
            List<Tuple<Guid, string>> firms = (
              from firm in a.Except(b)
              select new Tuple<Guid, string>(firm.Id, firm.Name)).ToList();

            return firms;
        }
    }
}