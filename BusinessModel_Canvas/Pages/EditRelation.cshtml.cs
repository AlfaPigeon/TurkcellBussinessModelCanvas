using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditRelationModel : PageModel
    {
        private readonly Canvas_Context _context;
        private Guid RelationID;
        private Guid? IncomeID, OutcomeID;
        private string Name, Description;
        public EditRelationModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Relation)
        {
            RelationID = Relation;
            var relation = _context.CustomerRelations.Where(s => s.Id == RelationID).FirstOrDefault();
            if (relation == null)
                Response.Redirect("/404");
            Name = relation.Name;
            Description = relation.Description;

            IncomeID = _context.IncomeFlows.Where(s => relation.IncomeID == s.Id).Select(s => s.Id).FirstOrDefault();
            OutcomeID = _context.OutcomeFlows.Where(s => relation.OutcomeID == s.Id).Select(s => s.Id).FirstOrDefault();

        }
        public List<Tuple<Guid, string>> GetAllIncome()
        {

            List<Tuple<Guid, string>> income = (
              from i in _context.IncomeFlows
              select new Tuple<Guid, string>(i.Id, i.Name)).ToList();

            return income;
        }

        public List<Tuple<Guid, string>> GetAllOutcome()
        {

            List<Tuple<Guid, string>> outcome = (
              from o in _context.OutcomeFlows
              select new Tuple<Guid, string>(o.Id, o.Name)).ToList();

            return outcome;
        }


        public string GetRelationName()
        {
            return Name;
        }
        public string GetRelationDescription()
        {
            return Description;
        }
        public Guid? GetSelectedIncome()
        {
            return IncomeID;
        }
        public Guid? GetSelectedOutcome()
        {
            return OutcomeID;
        }

        public Guid GetRelationID()
        {
            return RelationID;
        }
    }
}