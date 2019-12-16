using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditServiceModel : PageModel
    {
        private readonly Canvas_Context _context;
        private Guid ServiceID;
        private string Name, Description;
        public EditServiceModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Service)
        {
            ServiceID = Service;
            var service = _context.Services.Where(s => s.Id == ServiceID).FirstOrDefault();
            if (service == null)
                Response.Redirect("/404");
            Name = service.Name;
            Description = service.Description;
        }


        public string GetServiceName()
        {
            return Name;
        }

        public string GetServiceDescription()
        {
            return Description;
        }
     
        public Guid GetServiceID()
        {
            return ServiceID;
        }
    }
}