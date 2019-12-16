using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using BusinessModel_Canvas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusinessModel_Canvas.Pages
{
    public class KEYPARTNERSModel : PageModel
    {
        private readonly Canvas_Context _context;

        public KEYPARTNERSModel(Canvas_Context context)
        {
            _context = context;
        }


        public void OnGet()
        {


        }

        public List<Partner> GetPartners()
        {

            
              List<Partner> Partners = (
                from firm in _context.Firms
                join partner in _context.R_Partners on firm.Id equals partner.FirmID
                select new Partner
                {
                 FirmID = firm.Id,
                 FirmName = firm.Name,
                 FirmOrder = firm.Order,
                 FirmRelationshipDesc = firm.RelationshipDescription,
                 Reason = partner.Reason,
                 RelationID= partner.Id

                }).ToList();
            
            return Partners;
        }
        public List<Provider> GetProviders()
        {
            List<Provider> providers = (
              from provider in _context.R_ProvidesServices
              join f in _context.Firms on provider.ProviderID equals f.Id
              where provider.ClientID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")
              select new Provider()
              {
                  FirmID = f.Id,
                  FirmName = f.Name,
                  FirmOrder = f.Order,
                  FirmRelationshipDesc = f.RelationshipDescription

              }
              ).Distinct().ToList();

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

        public class Partner : Provider
        {
            public Guid RelationID { get; set; }
            public string Reason { get; set; }

        }
        public class Provider
        {
            public Guid FirmID { get; set; }
            public string FirmName { get; set; }
            public string FirmOrder { get; set; }
            public string FirmRelationshipDesc { get; set; }

        }



    }

}