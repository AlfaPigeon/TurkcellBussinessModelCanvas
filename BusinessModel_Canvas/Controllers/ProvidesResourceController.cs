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
using Microsoft.EntityFrameworkCore.Internal;
using System.Text.RegularExpressions;
using System.Web;

namespace BusinessModel_Canvas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidesResourceController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public ProvidesResourceController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/ProvidesResource
        [HttpGet]
        public async Task<ActionResult<IEnumerable<R_ProvidesResource>>> GetR_ProvidesResources()
        {
            return await _context.R_ProvidesResources.ToListAsync();
        }

        // GET: api/ProvidesResource/5
        [HttpGet("{id}")]
        public async Task<ActionResult<R_ProvidesResource>> GetR_ProvidesResource(Guid id)
        {
            var r_ProvidesResource = await _context.R_ProvidesResources.FindAsync(id);

            if (r_ProvidesResource == null)
            {
                return NotFound();
            }

            return new R_ProvidesResource() { 
            Id = r_ProvidesResource.Id,
            ResourceID = r_ProvidesResource.ResourceID,
            ServiceID = r_ProvidesResource.ServiceID,
            Description = HttpUtility.HtmlEncode(r_ProvidesResource.Description)
            };
        }


        // GET: api/ProvidesResource/Resources/{service Id}
        [HttpGet("Resources/{id}")]
        public ActionResult<List<ProvidedResource>> GetProvidedResources(Guid id)
        {

            List<ProvidedResource> Services = (
             from provider in _context.R_ProvidesResources
             where provider.ServiceID == id
             join resource in _context.Resources on provider.ResourceID equals resource.Id
             select new ProvidedResource() { Id = resource.Id, Name = HttpUtility.HtmlEncode(resource.Name), Description = HttpUtility.HtmlEncode(resource.Description), ProvideDescription = HttpUtility.HtmlEncode(provider.Description),Type=resource.IsEvent?"Event":"Resource" }).ToList();

            return Ok(Services);
        }


        // POST: api/ProvidesResource
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<R_ProvidesResource>> PostR_ProvidesResource(R_ProvidesResource r_ProvidesResource)
        {
            r_ProvidesResource.Description = (r_ProvidesResource.Description);
            _context.R_ProvidesResources.Add(r_ProvidesResource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetR_ProvidesResource", new { id = r_ProvidesResource.Id }, r_ProvidesResource);
        }

        // Post : api/ProvidesResource/Add/5/2/desc
        [HttpPost("Add/{sid}/{rid}/{desc}")]
        public async Task<ActionResult<ProvidedResource>> AddResourceToService(Guid sid, Guid rid, string desc)
        {
       

            //is valid 
            Service service = await _context.Services.FindAsync(sid);
            if (service == null)
            {
                return NotFound();
            }
           
            Resource resource =  await _context.Resources.FindAsync(rid);
            if (resource == null)
            {
                return NotFound();
            }
          



            //get provider id

            R_ProvidesResource existant = (from i in _context.R_ProvidesResources where i.ResourceID == rid && i.ServiceID == sid select i).FirstOrDefault();

            

            if (existant != null)
            {
                return Conflict();
            }

            
            //craft entity
            R_ProvidesResource provided = new R_ProvidesResource()
            {
                Id = Guid.NewGuid(),
                ServiceID = sid,
                ResourceID = rid,
                Description = (desc)
            };
         



            //add
            _context.R_ProvidesResources.Add(provided);
            await _context.SaveChangesAsync();
            
            ProvidedResource provided_resource = new ProvidedResource()
            {
                Id = resource.Id,
                Name = (resource.Name),
                Description = (resource.Description),
                ProvideDescription = (provided.Description),
                Type = resource.IsEvent?"Event":"Resource"


            };
           
            return provided_resource;
        }

        [HttpPost("Edit")]
        public async Task<ActionResult<R_ProvidesResource>> EditServiceProvide([FromForm]ResourceGroup resource)
        {

            var rel = await _context.R_ProvidesResources.FindAsync(resource.Id);
            if (rel == null)
            {
                return NotFound();
            }

            rel.Description = (resource.Description);

            await _context.SaveChangesAsync();

            return rel;
        }


        // DELETE: api/ProvidesResource/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<R_ProvidesResource>> DeleteR_ProvidesResource(Guid id)
        {
            var r_ProvidesResource = await _context.R_ProvidesResources.FindAsync(id);
            if (r_ProvidesResource == null)
            {
                return NotFound();
            }

            _context.R_ProvidesResources.Remove(r_ProvidesResource);
            await _context.SaveChangesAsync();

            return r_ProvidesResource;
        }

        // DELETE: api/Resource/5
        [HttpDelete("Provided/{id}")]
        public async Task<ActionResult<Resource>> DeleteProvided(Guid id)
        {

            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }


            List<R_ProvidesResource> tab = (from res in _context.Resources where res.Id == id
                                  join provide in _context.R_ProvidesResources on res.Id equals provide.ResourceID into table1
                                  from tb1 in table1.DefaultIfEmpty()
                                  join serv in _context.R_ProvidesServices on tb1.ServiceID equals serv.ServiceID
                                  where serv.ClientID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")

                                  select tb1).Distinct().ToList();

            if (tab != null)
            {
                _context.R_ProvidesResources.RemoveRange(tab);
            }

            await _context.SaveChangesAsync();

            return resource;
        }
        // DELETE: api/Resource/5
        [HttpDelete("Providing/{id}")]
        public async Task<ActionResult<Resource>> DeleteProviding(Guid id)
        {

            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }


            List<R_ProvidesResource> tab = (from res in _context.Resources
                                            where res.Id == id
                                            join provide in _context.R_ProvidesResources on res.Id equals provide.ResourceID into table1
                                            from tb1 in table1.DefaultIfEmpty()
                                            join serv in _context.R_ProvidesServices on tb1.ServiceID equals serv.ServiceID
                                            where serv.ProviderID == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")

                                            select tb1).Distinct().ToList();

            if (tab != null)
            {
                _context.R_ProvidesResources.RemoveRange(tab);
            }

            await _context.SaveChangesAsync();

            return resource;
        }


        // DELETE: api/Employee/Remove/5/4
        [HttpDelete("Remove/{ServiceID}/{id}")]
        public ActionResult<R_ProvidesResource> RemoveResourceFromService(Guid ServiceID, Guid id)
        {
            R_ProvidesResource resource = (from i in _context.R_ProvidesResources where i.ServiceID == ServiceID && i.ResourceID == id select i).FirstOrDefault();
            if (resource == null)
            {
                return NotFound();
            }

            _context.Remove(resource);
            _context.SaveChanges();

            return resource;
        }
        private bool R_ProvidesResourceExists(Guid id)
        {
            return _context.R_ProvidesResources.Any(e => e.Id == id);
        }

        //utils

        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }

        //Inner Classes
        public class ProvidedResource
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string ProvideDescription { get; set; }
            public string Type { get; set; }


        }

        public class ResourceGroup
        {
            public Guid Id { get; set; }
            public string Description { get; set; }
        }

    }
}
