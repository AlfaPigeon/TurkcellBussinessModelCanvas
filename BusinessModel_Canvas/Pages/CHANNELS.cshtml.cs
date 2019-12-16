using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class CHANNELSModel : PageModel
    {
        private readonly Canvas_Context _context;

        public CHANNELSModel(Canvas_Context context)
        {
            _context = context;
        }


        public void OnGet()
        {

        }

        public List<Activity> GetOurProvidingActivities()
        {


            List<Activity> tab = (from act in _context.Resources
                                  where act.IsEvent == true
                                  join provide in _context.R_ProvidesResources on act.Id equals provide.ResourceID into table1
                                  from tb1 in table1.DefaultIfEmpty()
                                  join serv in _context.R_ProvidesServices on tb1.ServiceID equals serv.ServiceID
                                  where serv.ProviderID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")

                                  select new Activity()
                                  {
                                      Id = act.Id,
                                      Name = act.Name,
                                      Description = act.Description
                                  }).Distinct().OrderBy(s => s.Name).ToList();




            return tab;
        }
        public List<Activity> GetTheirProvidingActivities()
        {
            List<Activity> tab = (from act in _context.Resources
                                  where act.IsEvent == true
                                  join provide in _context.R_ProvidesResources on act.Id equals provide.ResourceID into table1
                                  from tb1 in table1.DefaultIfEmpty()
                                  join serv in _context.R_ProvidesServices on tb1.ServiceID equals serv.ServiceID
                                  where serv.ClientID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")

                                  select new Activity()
                                  {
                                      Id = act.Id,
                                      Name = act.Name,
                                      Description = act.Description
                                  }).Distinct().OrderBy(s => s.Name).ToList();




            return tab;
        }

        public List<Activity> GetAllActivities()
        {
            List<Activity> tab = (
                                  from act in _context.Resources
                                  where act.IsEvent == true
                                  select new Activity()
                                  {
                                      Id = act.Id,
                                      Name = act.Name,
                                      Description = act.Description
                                  }).Distinct().OrderBy(s => s.Name).ToList();




            return tab;
        }

        public List<Tuple<Guid, string>> GetAllEmployees()
        {

            List<Tuple<Guid, string>> AllEmployees = (
              from person in _context.Employees
              select new Tuple<Guid, string>(person.Id, person.Name)).ToList();

            return AllEmployees;
        }

        public List<Tuple<Guid, string>> GetAllChannels()
        {
            List<Tuple<Guid, string>> AllChannels = (
              from ch in _context.ShippingChannels
              select new Tuple<Guid, string>(ch.Id, ch.Name)).ToList();

            return AllChannels;
        }

        public class Activity
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public String Description { get; set; }

        }
    }
}