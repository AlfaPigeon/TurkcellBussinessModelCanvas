using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditResourceModel : PageModel
    {
        private readonly Canvas_Context _context;
        private Guid ResourceID;
        private Guid? IncomeID, OutcomeID, RelationID;
        private string Name, Description;
        public EditResourceModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Resource)
        {
            ResourceID = Resource;
            var resource = _context.Resources.Where(s => s.Id == ResourceID && s.IsEvent == false).FirstOrDefault();
            if (resource == null)
                Response.Redirect("/404");
            Name = resource.Name;
            Description = resource.Description;
            var info = _context.ResourceInfo.Where(s => s.Id == resource.ResourceInfoID).FirstOrDefault();
            if (info == null)
            {
                return;
            }
            IncomeID = _context.IncomeFlows.Where(s => info.IncomeID == s.Id).Select(s => s.Id).FirstOrDefault();
            OutcomeID = _context.OutcomeFlows.Where(s => info.OutcomeID == s.Id).Select(s => s.Id).FirstOrDefault();
            RelationID = _context.CustomerRelations.Where(s => info.CostumerRelationID == s.Id).Select(s => s.Id).FirstOrDefault();
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

        public List<Tuple<Guid, string>> GetAllRelations()
        {

            List<Tuple<Guid, string>> relation = (
              from o in _context.CustomerRelations
              select new Tuple<Guid, string>(o.Id, o.Name)).ToList();

            return relation;
        }

        public string GetResourceName()
        {
            return Name;
        }

        public string GetResourceDescription()
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
        public Guid? GetSelectedRelation()
        {
            return RelationID;
        }
        public Guid GetResourceID()
        {
            return ResourceID;
        }
    }
}