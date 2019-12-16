using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class AddServiceModel : PageModel
    {
        private readonly Canvas_Context _context;

        public AddServiceModel(Canvas_Context context)
        {
            _context = context;
        }
        public void OnGet()
        {

        }

        public List<Tuple<Guid, string>> GetAllFirms()
        {
            List<Tuple<Guid, string>> firms = (
              from firm in _context.Firms where firm.Id != Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")
              select new Tuple<Guid, string>(firm.Id, firm.Name)).ToList();

            return firms;
        }


        public List<Tuple<Guid, string>> GetAllServices()
        {

            List<Tuple<Guid, string>> AllServices = (
              from service in _context.Services
              select new Tuple<Guid, string>(service.Id, service.Name)).ToList();

            return AllServices;
        }
    }
}