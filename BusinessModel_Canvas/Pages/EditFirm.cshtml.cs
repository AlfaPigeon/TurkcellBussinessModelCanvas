using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditFirmModel : PageModel
    {
        private Guid FirmID;
        private string Name, Description,RelationDescription,Order;
        private readonly Canvas_Context _context;

        public EditFirmModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Firm)
        {
            FirmID = Firm;
            var firm = _context.Firms.Where(s => s.Id == FirmID).Select(s => s).FirstOrDefault();
            if(firm == null)Response.Redirect("/404");

            Name = firm.Name;
            Description = firm.Description;
            Order = firm.Order;
            RelationDescription = firm.RelationshipDescription;
        }

        public string GetFirmName() { return Name; }
        public string GetFirmDescription() { return Description; }
        public string GetFirmRelationDescription() { return RelationDescription; }
        public string GetFirmOrder() { return Order; }
        public Guid GetFirmID() { return FirmID; }
    }
}