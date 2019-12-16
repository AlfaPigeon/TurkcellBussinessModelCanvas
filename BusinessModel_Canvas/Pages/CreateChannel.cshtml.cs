using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class CreateChannelModel : PageModel
    {
        private readonly Canvas_Context _context;

        public CreateChannelModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }
        public List<Tuple<Guid, string>> GetAllIncome()
        {
            List<Tuple<Guid, string>> income = _context.IncomeFlows.OrderBy(s => s.Name).Select(s => new Tuple<Guid,string>(s.Id,s.Name)).ToList();
            return income;
        }

        public List<Tuple<Guid, string>> GetAllOutcome()
        {
            List<Tuple<Guid, string>> outcome = _context.OutcomeFlows.OrderBy(s => s.Name).Select(s => new Tuple<Guid, string>(s.Id, s.Name)).ToList();
            return outcome;
        }
    }
}