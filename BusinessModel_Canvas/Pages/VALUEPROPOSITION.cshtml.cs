using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class VALUEPROPOSITIONModel : PageModel
    {
        private readonly Canvas_Context _context;

        public VALUEPROPOSITIONModel(Canvas_Context context)
        {
            _context = context;
        }


        public void OnGet()
        {

        }

        public List<ServiceDescription> GetOurProvidingServices()
        {


            List<ServiceDescription> tab = (from act in _context.Services
                                              join serv in _context.R_ProvidesServices on act.Id equals serv.ServiceID
                                              where serv.ProviderID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")

                                              select new ServiceDescription()
                                              {
                                                  Id = act.Id,
                                                  Name = act.Name,
                                                  Description = act.Description
                                              }).Distinct().OrderBy(s => s.Name).ToList();
            return tab;
        }
        public List<ServiceDescription> GetTheirProvidingServices()
        {
            List<ServiceDescription> tab = (from act in _context.Services
                                            join serv in _context.R_ProvidesServices on act.Id equals serv.ServiceID
                                            where serv.ClientID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")

                                            select new ServiceDescription()
                                              {
                                                  Id = act.Id,
                                                  Name = act.Name,
                                                  Description = act.Description
                                              }).Distinct().OrderBy(s => s.Name).ToList();
            return tab;
        }

        public List<ServiceDescription> GetAllServices()
        {
            List<ServiceDescription> tab = (
                                      from act in _context.Services
                                      select new ServiceDescription()
                                      {
                                      Id = act.Id,
                                      Name = act.Name,
                                      Description = act.Description
                                      }).Distinct().OrderBy(s => s.Name).ToList();
            return tab;
        }

        public List<Tuple<Guid, string>> GetAllEmployees()
        {

            List<Tuple<Guid, string>> AllEmployees = _context.Employees.OrderBy(s => s.Name).Select(c => new Tuple<Guid, string>(c.Id, c.Name)).ToList();

            return AllEmployees;
        }

        public List<Tuple<Guid, string>> GetAllResources()
        {

            List<Tuple<Guid, string>> AllResources = _context.Resources.OrderBy(s => s.Name).Select(c => new Tuple<Guid, string>(c.Id, c.Name)).ToList();

            return AllResources;
        }

        public class ServiceDescription
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public String Description { get; set; }

        }
    }
}