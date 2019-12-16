using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessModel_Canvas.Data;
using BusinessModel_Canvas.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

namespace BusinessModel_Canvas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidesServiceController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public ProvidesServiceController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Provides
        [HttpGet]
        public async Task<ActionResult<IEnumerable<R_ProvidesServices>>> GetR_ProvidesServices()
        {
            return await _context.R_ProvidesServices.ToListAsync();
        }

        // GET: api/Provides/5
        [HttpGet("{id}")]
        public async Task<ActionResult<R_ProvidesServices>> GetR_ProvidesServices(Guid id)
        {
            var r_ProvidesServices = await _context.R_ProvidesServices.FindAsync(id);

            if (r_ProvidesServices == null)
            {
                return NotFound();
            }

            return new R_ProvidesServices()
            {
                Id = r_ProvidesServices.Id,
                ProviderID = r_ProvidesServices.ProviderID,
                ServiceID = r_ProvidesServices.ServiceID,
                ClientID = r_ProvidesServices.ClientID,
                Reason = HttpUtility.HtmlEncode(r_ProvidesServices.Reason)
                
            };
        }
      
        // GET: api/ProvidesService/Services/5
        [HttpGet("Services/{id}")]
        public ActionResult<List<ProvidedService>> GetProvidedServices(Guid id)
        {
            
            List<ProvidedService> Services = (
             from provider in _context.R_ProvidesServices
             where provider.ProviderID == id
             join service in _context.Services on provider.ServiceID equals service.Id
             select new ProvidedService() {Id=service.Id,Name= HttpUtility.HtmlEncode(service.Name) ,Description= HttpUtility.HtmlEncode(service.Description),Reason= HttpUtility.HtmlEncode(provider.Reason)}).ToList();

            return  Ok(Services);
        }
        // GET: api/ProvidesService/Services/5
        [HttpGet("OurProvidedServices/{id}")]
        public ActionResult<List<ProvidedService>> GetOurProvidingServices(Guid id)
        {

            List<ProvidedService> Services = (
             from provider in _context.R_ProvidesServices
             where provider.ProviderID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d") && provider.ClientID == id
             join service in _context.Services on provider.ServiceID equals service.Id
             select new ProvidedService() { Id = service.Id, Name = HttpUtility.HtmlEncode(service.Name), Description = HttpUtility.HtmlEncode(service.Description), Reason = HttpUtility.HtmlEncode(provider.Reason) }).ToList();

            return Services;
        }


        // POST: api/Provides
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<R_ProvidesServices>> PostR_ProvidesServices(R_ProvidesServices r_ProvidesServices)
        {
            r_ProvidesServices.Reason = (r_ProvidesServices.Reason);
            _context.R_ProvidesServices.Add(r_ProvidesServices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetR_ProvidesServices", new { id = r_ProvidesServices.Id }, r_ProvidesServices);
        }
        [HttpPost("Edit")]
        public async Task<ActionResult<R_ProvidesServices>> EditServiceProvide([FromForm]ServiceGroup service)
        {
            
            var rel = await _context.R_ProvidesServices.FindAsync(service.Id);
            if (rel == null)
            {
                return NotFound();
            }

            rel.StartDate = service.Start;
            rel.EndDate = service.End;
            rel.Reason = (service.Reason);

            await _context.SaveChangesAsync();

            return rel;
        }
        // Post : api/Add/5/12-5-2012/12-7-2012/reason 
        [HttpPost("Add")]
        public async Task<ActionResult<ProvidedService>> AddServiceToPartner([FromForm] ServiceGroup serviceGroup)
        {


            //is valid 
            var service = await _context.Services.FindAsync(serviceGroup.Id);
            if (service == null)
            {
                return NotFound();
            }

            //get provider id
            if (Request.Cookies["firm-id"] == null)
            {
                return BadRequest("Coudn'd find cookie!");
            }
            Guid value = Guid.Empty;
            try
            {
                value = Guid.Parse(Request.Cookies["firm-id"]);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            var check = (from provide in _context.R_ProvidesServices where provide.ServiceID == serviceGroup.Id && value == provide.ProviderID select provide).Count(); 
            if(check != 0 )
            {
                return BadRequest(check);
            }
            //craft entity

            
    
            R_ProvidesServices provided = new R_ProvidesServices()
            {
                Id = Guid.NewGuid(),
                ClientID = Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d"),
                ProviderID = value,
                Reason = (serviceGroup.Reason),
                StartDate = serviceGroup.Start,
                EndDate = serviceGroup.End,
                ServiceID = serviceGroup.Id
            };

            

            //add
            await _context.R_ProvidesServices.AddAsync(provided);
            await _context.SaveChangesAsync();
            ProvidedService provided_service = new ProvidedService()
            {
                Id = service.Id,
                Name = (service.Name),
                Description= (service.Description),
                Reason = (provided.Reason)

            };
            return Ok(provided_service);
        }

        [HttpPost("AddService/{id}")]
        public async Task<ActionResult<ProvidedService>> RecieveServiceFrom(Guid id,[FromForm] ServiceGroup serviceGroup)
        {


            //is valid 
            var service = await _context.Services.FindAsync(serviceGroup.Id);
            if (service == null)
            {
                return NotFound();
            }

            //get provider id
            var value = id;
            var check = (from provide in _context.R_ProvidesServices where provide.ServiceID == serviceGroup.Id && value == provide.ProviderID select provide).Count();
            if (check != 0)
            {
                return BadRequest(check);
            }
            //craft entity



            R_ProvidesServices provided = new R_ProvidesServices()
            {
                Id = Guid.NewGuid(),
                ClientID = Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d"),
                ProviderID = value,
                Reason = (serviceGroup.Reason),
                StartDate = serviceGroup.Start,
                EndDate = serviceGroup.End,
                ServiceID = serviceGroup.Id
            };




            //add
            await _context.R_ProvidesServices.AddAsync(provided);
            await _context.SaveChangesAsync();
            ProvidedService provided_service = new ProvidedService()
            {
                Id = service.Id,
                Name = (service.Name),
                Description = (service.Description),
                Reason = (provided.Reason)

            };

            return Ok(provided_service);
        }
        [HttpPost("AddToClient")]
        public async Task<ActionResult<ProvidedService>> AddServiceToClient([FromForm]ServiceGroup serviceGroup)
        {

            //is valid 
            var service = await _context.Services.FindAsync(serviceGroup.Id);
            if (service == null)
            {
                return NotFound();
            }

            //get provider id
            if (Request.Cookies["firm-id"] == null)
            {
                return BadRequest("Coudn'd find cookie!");
            }
            Guid value = Guid.Empty;
            try
            {
                value = Guid.Parse(Request.Cookies["firm-id"]);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            var check = (from provide in _context.R_ProvidesServices where provide.ServiceID == serviceGroup.Id && value == provide.ClientID select provide).Count();
            if (check != 0)
            {
                return BadRequest(check);
            }
            //craft entity
            R_ProvidesServices provided = new R_ProvidesServices()
            {
                Id = Guid.NewGuid(),
                ClientID = value,
                ProviderID = Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d"),
                Reason = (serviceGroup.Reason),
                StartDate = serviceGroup.Start,
                EndDate = serviceGroup.End,
                ServiceID = serviceGroup.Id
            };




            //add
            _context.R_ProvidesServices.Add(provided);
            await _context.SaveChangesAsync();

            ProvidedService provided_service = new ProvidedService()
            {
                Id = service.Id,
                Name = (service.Name),
                Description = (service.Description),
                Reason = (provided.Reason)

            };

            return Ok(provided_service);
        }

        [HttpPost("AddToClient/{id}")]
        public async Task<ActionResult<ProvidedService>> GiveServiceTo(Guid id,[FromForm]ServiceGroup serviceGroup)
        {

            //is valid 
            var service = await _context.Services.FindAsync(serviceGroup.Id);
            if (service == null)
            {
                return NotFound();
            }

            //get provider id
            var value = id;
            var check = (from provide in _context.R_ProvidesServices where provide.ServiceID == serviceGroup.Id && value == provide.ClientID select provide).Count();
            if (check != 0)
            {
                return BadRequest(check);
            }
            //craft entity
            R_ProvidesServices provided = new R_ProvidesServices()
            {
                Id = Guid.NewGuid(),
                ClientID = value,
                ProviderID = Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d"),
                Reason = (serviceGroup.Reason),
                StartDate = serviceGroup.Start,
                EndDate = serviceGroup.End,
                ServiceID = serviceGroup.Id
            };




            //add
            _context.R_ProvidesServices.Add(provided);
            await _context.SaveChangesAsync();

            ProvidedService provided_service = new ProvidedService()
            {
                Id = service.Id,
                Name = (service.Name),
                Description = (service.Description),
                Reason = (provided.Reason)

            };

            return Ok(provided_service);
        }

        // DELETE: api/Provides/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<R_ProvidesServices>>> DeleteAllServicesFromProvider(Guid id)
        {

            if(!_context.Firms.Any(s=>s.Id == id))
            {
                return NotFound();
            }


            var list = _context.R_ProvidesServices.Where(s => s.ProviderID == id).Select(s => s).ToList();
            _context.R_ProvidesServices.RemoveRange(list);
            await _context.SaveChangesAsync();

            return list;
        }

        // DELETE: api/Employee/Remove/5/4
        [HttpDelete("Remove/{FirmID}/{id}")]
        public ActionResult<List<ProvidedService>> RemoveServiceFromFirm(Guid FirmID, Guid id)
        {
            R_ProvidesServices service = (from i in _context.R_ProvidesServices where i.ProviderID == FirmID && i.ServiceID == id select i).FirstOrDefault();
            if (service == null)
            {
                return NotFound();
            }

            _context.Remove(service);
            _context.SaveChanges();

            return GetProvidedServices(id);
        }

        
        // DELETE: api/Employee/Remove/5/4
        [HttpDelete("RemoveFromClient/{FirmID}/{id}")]
        public ActionResult<List<ProvidedService>> RemoveServiceFromClient(Guid FirmID, Guid id)
        {
            R_ProvidesServices service = (from i in _context.R_ProvidesServices where i.ClientID == FirmID && i.ServiceID == id select i).FirstOrDefault();
            if (service == null)
            {
                return NotFound();
            }

            _context.Remove(service);
            _context.SaveChanges();
            
            return GetOurProvidingServices(id);
        }



        private bool R_ProvidesServicesExists(Guid id)
        {
            return _context.R_ProvidesServices.Any(e => e.Id == id);
        }

        //Inner Classes
        public class ProvidedService : Service
        {
            public string Reason { get; set; }
        }

        /** ServiceGroup - wraps added services*/
        public class ServiceGroup
        {
            public Guid Id { get; set; }
            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }
            public string Reason { get; set; }
        }
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
    }
}
