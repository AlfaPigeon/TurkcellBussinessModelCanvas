using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditIncomeModel : PageModel
    {
        private readonly Canvas_Context _context;
        private Guid IncomeID;
        private string Name, Description;
        private decimal? Real, Guessed;
        public EditIncomeModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Income)
        {
            IncomeID = Income;
            var incomeflow = _context.IncomeFlows.Where(s => s.Id == IncomeID).Select(s => s).FirstOrDefault();
            if (incomeflow == null)
            {
                Response.Redirect("/404");
            }
            Description = incomeflow.Description;
            Name = incomeflow.Name;
            Guessed = incomeflow.GuessedAmount;
            Real = incomeflow.RealAmount;
        }



        public Guid GetIncome() { return IncomeID; }
        public string GetName() { return Name; }
        public string GetDescription() { return Description; }
        public Decimal? GetGuessed() { return Guessed; }
        public Decimal? GetReal() { return Real; }
    }
}