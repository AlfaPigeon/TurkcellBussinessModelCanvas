using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class COSTUMERRELATIONSHIPSModel : PageModel
    {
        private readonly Canvas_Context _context;

        public COSTUMERRELATIONSHIPSModel(Canvas_Context context)
        {
            _context = context;
        }


        public void OnGet()
        {

        }


        public List<RelationDescription> GetAllRelations()
        {
            List<RelationDescription> tab = (
                                  from rel in _context.CustomerRelations
                                  select new RelationDescription()
                                  {
                                      Id = rel.Id,
                                      Name = rel.Name,
                                      Description = rel.Description
                                  }).Distinct().OrderBy(s => s.Name).ToList();
            return tab;
        }

        public List<EmployeeDescription> GetAllEmployees()
        {

            List<EmployeeDescription> AllEmployees = (
              from person in _context.Employees
              select new EmployeeDescription() { 
              Id=person.Id,
              Name=person.Name,
              Position=person.Position
              }
              ).ToList();

            return AllEmployees;
        }

        public List<Tuple<Guid, string>> GetAllChannels()
        {
            List<Tuple<Guid, string>> AllChannels = (
              from ch in _context.ShippingChannels
              select new Tuple<Guid, string>(ch.Id, ch.Name)).ToList();

            return AllChannels;
        }
        public List<Tuple<Guid, string>> GetAllFirms()
        {
            List<Tuple<Guid, string>> firms = (
              from firm in _context.Firms where firm.Id != Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")
              select new Tuple<Guid, string>(firm.Id, firm.Name)).ToList();

            return firms;
        }

        public class RelationDescription
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public String Description { get; set; }

        }

        public class EmployeeDescription
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public String Position { get; set; }

        }
    }
}