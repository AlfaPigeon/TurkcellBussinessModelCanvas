using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditPartnerModel : PageModel
    {
        private Guid FirmID;
        private string Name, Reason;
        private readonly Canvas_Context _context;

        public EditPartnerModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Partner)
        {
            FirmID = Partner;
            Name = (from firm in _context.Firms where firm.Id == FirmID select firm.Name).FirstOrDefault();
            Reason = (from p in _context.R_Partners where p.FirmID == FirmID select p.Reason).FirstOrDefault();
        }
        
        public string GetFirmName() { return Name; }
        public string GetPartnerReason() { return Reason; }
        public Guid GetFirmID() { return FirmID; }


    }
}