using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessModel_Canvas.Data;
using BusinessModel_Canvas.Models;
using System.Text.RegularExpressions;
using System.Web;

namespace BusinessModel_Canvas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public ServiceController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Service
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        // GET: api/Service/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(Guid id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return new Service() { 
            Id = service.Id,
            Name = HttpUtility.HtmlEncode(service.Name),
            Description = HttpUtility.HtmlEncode(service.Description)

            };
        }


        // POST: api/Service
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Service>> PostService([FromForm]Service service)
        {
            Service serv = new Service()
            {
                Id=Guid.NewGuid(),
                Name = (service.Name),
                Description = (service.Description)
            };
            _context.Services.Add(serv);
            await _context.SaveChangesAsync();

            return serv;
        }

        [HttpPost("Edit")]
        public async Task<ActionResult<Service>> EditService([FromForm]Service service)
        {
            if (service.Name == null || service.Name == "") return BadRequest();

            var serv = await _context.Services.FindAsync(service.Id);
            if(serv == null)
            {
                return NotFound();
            }

            serv.Name = (service.Name);
            serv.Description = (service.Description);

            await _context.SaveChangesAsync();

            return serv;
        }

        // DELETE: api/Service/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Service>> DeleteService(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            var p_service = _context.R_ProvidesServices.Where(s => s.ServiceID == id).ToList();

            if(p_service != null)
            {
                _context.R_ProvidesServices.RemoveRange(p_service);
            }

            var p_resource = _context.R_ProvidesResources.Where(s => s.ServiceID == id).ToList();

            if (p_resource != null)
            {
                _context.R_ProvidesResources.RemoveRange(p_resource);
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return service;
        }


        // DELETE: api/Service/Providing/5
        /** removes service from all our providing relations*/

        [HttpDelete("Providing/{id}")]
        public async Task<ActionResult<List<R_ProvidesServices>>> DeleteProvidingService(Guid id)
        {

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            List<R_ProvidesServices> p_service = (from act in _context.Services
                                            join serv in _context.R_ProvidesServices on act.Id equals serv.ServiceID
                                            where serv.ProviderID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d") && serv.ServiceID == id

                                            select serv).ToList();



            if (p_service != null)
            {
                _context.R_ProvidesServices.RemoveRange(p_service);
            }




            await _context.SaveChangesAsync();

            return p_service;
        }

        // DELETE: api/Service/5
        [HttpDelete("Provided/{id}")]
        public async Task<ActionResult<List<R_ProvidesServices>>> DeleteProvidedService(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            List<R_ProvidesServices> p_service = (from act in _context.Services
                                                  join serv in _context.R_ProvidesServices on act.Id equals serv.ServiceID
                                                  where serv.ClientID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d") && serv.ServiceID == id

                                                  select serv).ToList();



            if (p_service != null)
            {
                _context.R_ProvidesServices.RemoveRange(p_service);
            }

            await _context.SaveChangesAsync();

            return p_service;
        }


        [HttpDelete("RemoveFrom/{id}")]
        public async Task<ActionResult<List<R_ProvidesServices>>> RemoveServicesFromClient(Guid id)
        {

            var client = await _context.Firms.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            List<R_ProvidesServices> p_service = (from act in _context.Services
                                                  join serv in _context.R_ProvidesServices on act.Id equals serv.ServiceID
                                                  where serv.ProviderID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d") &&
                                                  serv.ClientID == id
                                                  select serv).ToList();



            if (p_service != null)
            {
                _context.R_ProvidesServices.RemoveRange(p_service);
            }




            await _context.SaveChangesAsync();

            return p_service;
        }        
        //utils
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
        private bool ServiceExists(Guid id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
