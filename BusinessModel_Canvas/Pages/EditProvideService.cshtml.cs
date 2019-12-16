using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditProvideServiceModel : PageModel
    {

        private Guid FirmID, ServiceID, RelationID;
        private string FirmName, ServiceName, Reason;
        private readonly Canvas_Context _context;
        private DateTime? Start, End;

        public EditProvideServiceModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Firm, Guid Service, bool IsClient)
        {
            FirmID = Firm;
            ServiceID = Service;
            FirmName = (from firm in _context.Firms where firm.Id == FirmID select firm.Name).FirstOrDefault();
            ServiceName = (from service in _context.Services where service.Id == ServiceID select service.Name).FirstOrDefault();



            var provides = _context.R_ProvidesServices.Where(s => s.ServiceID == ServiceID && s.ProviderID == FirmID);
            if (IsClient)
            {
                provides = _context.R_ProvidesServices.Where(s => s.ServiceID == ServiceID && s.ClientID == FirmID);
            }

            if (provides.Select(s => s).Count() == 0)
            {
                Response.Redirect("/404");
            }
            Reason = provides.Select(s => s.Reason).FirstOrDefault();
            Start = provides.Select(s => s.StartDate).FirstOrDefault();
            End = provides.Select(s => s.EndDate).FirstOrDefault();
            RelationID = provides.Select(s => s.Id).FirstOrDefault();

        }

        public string GetFirmName() { return FirmName; }
        public string GetServiceName() { return ServiceName; }
        public string GetReason() { return Reason; }
        public Guid GetRelationID() { return RelationID; }
        public DateTime? GetStartDate() { return Start; }
        public DateTime? GetEndDate() { return End; }

    }
}