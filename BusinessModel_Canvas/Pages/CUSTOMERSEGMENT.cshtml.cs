using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class CUSTOMERSEGMENTModel : PageModel
    {
        private readonly Canvas_Context _context;

        public CUSTOMERSEGMENTModel(Canvas_Context context)
        {
            _context = context;
        }


        public void OnGet()
        {


        }


        public List<Client> GetClients()
        {
            List<Client> providers = (
              from provider in _context.R_ProvidesServices
              join f in _context.Firms on provider.ClientID equals f.Id
              where provider.ProviderID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")
              select new Client()
              {
                  FirmID = f.Id,
                  FirmName = f.Name,
                  FirmOrder = f.Order,
                  FirmRelationshipDesc = f.RelationshipDescription

              }
              ).Distinct().OrderBy(s => s.FirmName).ToList();

            return providers;
        }
        public List<Tuple<Guid, string>> GetAllServices()
        {

            List<Tuple<Guid, string>> AllServices = _context.Services.OrderBy(s => s.Name).Select(c => new Tuple<Guid, string>(c.Id, c.Name)).ToList();

            return AllServices;
        }
        public List<Tuple<Guid, string>> GetAllResources()
        {

            List<Tuple<Guid, string>> AllResources = _context.Resources.OrderBy(s => s.Name).Select(c => new Tuple<Guid, string>(c.Id, c.Name)).ToList();

            return AllResources;
        }

        public List<Tuple<Guid, string>> GetAllEmployees()
        {

            List<Tuple<Guid, string>> AllEmployees = _context.Employees.OrderBy(s => s.Name).Select(c => new Tuple<Guid, string>(c.Id, c.Name)).ToList();

            return AllEmployees;
        }


        public class Client
        {
            public Guid FirmID { get; set; }
            public string FirmName { get; set; }
            public string FirmOrder { get; set; }
            public string FirmRelationshipDesc { get; set; }

        }

    }
}