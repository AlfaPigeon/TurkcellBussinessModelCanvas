using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditOutcomeModel : PageModel
    {
        private readonly Canvas_Context _context;
        private Guid OutcomeID;
        private string Name, Description;
        private decimal? Real, Guessed; 
        public EditOutcomeModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Outcome)
        {
            OutcomeID = Outcome;
            var outcomeflow = _context.OutcomeFlows.Where(s => s.Id == OutcomeID).Select(s => s).FirstOrDefault();
            if (outcomeflow == null)
            {
                Response.Redirect("/404");
            }
            Description = outcomeflow.Description;
            Name = outcomeflow.Name;
            Guessed = outcomeflow.GuessedAmount;
            Real = outcomeflow.RealAmount;
        }



        public Guid GetOutcome() { return OutcomeID; }
        public string GetName() { return Name; }
        public string GetDescription() { return Description; }
        public Decimal? GetGuessed() { return Guessed; }
        public Decimal? GetReal() { return Real; }
    }
}