using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class REVENUESTREAMSModel : PageModel
    {
        private readonly Canvas_Context _context;

        public REVENUESTREAMSModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }


        public List<Tuple<Guid, string>> GetAllIncome()
        {

            List<Tuple<Guid, string>> income = (
              from i in _context.IncomeFlows
              select new Tuple<Guid, string>(i.Id, i.Name)).ToList();

            return income;
        }
    }
}