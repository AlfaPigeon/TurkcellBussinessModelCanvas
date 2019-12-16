using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class CreateEmployeeModel : PageModel
    {
        private readonly Canvas_Context _context;

        public CreateEmployeeModel(Canvas_Context context)
        {
            _context = context;
        }
        public void OnGet()
        {

        }

        public List<Tuple<Guid, string>> GetAllFirms()
        {
            List<Tuple<Guid, string>> firms = (
              from firm in _context.Firms
              select new Tuple<Guid, string>(firm.Id, firm.Name)).ToList();

            return firms;
        }
    }
}