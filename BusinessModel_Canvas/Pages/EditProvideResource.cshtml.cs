using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditProvideResourceModel : PageModel
    {
        private Guid ResourceID, ServiceID, RelationID;
        private string ServiceName, Reason, ResourceName;
        private readonly Canvas_Context _context;


        public EditProvideResourceModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Service, Guid Resource)
        {
            ResourceID = Resource;
            ServiceID = Service;
            ServiceName = (from servicename in _context.Services where servicename.Id == ServiceID select servicename.Name).FirstOrDefault();
            ResourceName = (from resourcename in _context.Resources where resourcename.Id == ResourceID select resourcename.Name).FirstOrDefault();
            var provides = _context.R_ProvidesResources.Where(s => s.ServiceID == ServiceID && s.ResourceID == ResourceID);
            if (provides.Select(s => s).Count() == 0)
            {
                Response.Redirect("/404");
            }
            Reason = provides.Select(s => s.Description).FirstOrDefault();
            RelationID = provides.Select(s => s.Id).FirstOrDefault();

        }

        public string GetServiceName() { return ServiceName; }
        public string GetResourceName() { return ResourceName; }
        public string GetReason() { return Reason; }
        public Guid GetRelationID() { return RelationID; }

    }
}