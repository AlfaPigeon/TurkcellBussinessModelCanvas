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
    public class ResourceController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public ResourceController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Resource
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            return await _context.Resources.ToListAsync();
        }

        // GET: api/Resource/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(Guid id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound();
            }

            return resource;
        }


        // POST: api/Resource
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Resource>> PostResource(Resource resource)
        {
            resource.Name = HttpUtility.HtmlEncode(resource.Name);
            resource.Description = HttpUtility.HtmlEncode(resource.Description);
            
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.Id }, resource);
        }



        // POST: api/Resource
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Create")]
        public async Task<ActionResult<Resource>> CreateResource([FromForm]DetailedResource resource)
        {

            ResourceInfo info = new ResourceInfo
            {
                Id = Guid.NewGuid(),
                IncomeID = resource.IncomeID,
                OutcomeID = resource.OutcomeID,
                CostumerRelationID = resource.RelationID
            };

            Resource newRes = new Resource()
            {
                Id = Guid.NewGuid(),
                Name = HttpUtility.HtmlEncode(resource.Name),
                Description = HttpUtility.HtmlEncode(resource.Description),
                IsEvent = (resource.Type == "Activity"),
                ResourceInfoID = info.Id
            };

            await _context.ResourceInfo.AddAsync(info);
            await _context.Resources.AddAsync(newRes);
            await _context.SaveChangesAsync();

            return newRes;
        }

        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Edit")]
        public async Task<ActionResult<Resource>> EditResource([FromForm]DetailedResource resource)
        {
            if (resource.Name == null || resource.Name == "") return BadRequest();
            var res = await _context.Resources.FindAsync(resource.Id);

            if(res == null)
            {
                return NotFound();
            }

            var info = await _context.ResourceInfo.FindAsync(res.ResourceInfoID);

            if(info == null)
            {
                info = new ResourceInfo()
                {
                    Id = Guid.NewGuid()
                };
                res.ResourceInfoID = info.Id;
                await _context.ResourceInfo.AddAsync(info);
            }

            res.Name = HttpUtility.HtmlEncode(resource.Name);
            res.Description = HttpUtility.HtmlEncode(resource.Description);
            
            if(_context.CustomerRelations.Any(s => s.Id == resource.RelationID))info.CostumerRelationID = resource.RelationID;
            if (_context.IncomeFlows.Any(s => s.Id == resource.IncomeID)) info.IncomeID = resource.IncomeID;
            if (_context.OutcomeFlows.Any(s => s.Id == resource.OutcomeID)) info.OutcomeID = resource.OutcomeID;

            await _context.SaveChangesAsync();

            return res;
        }
        // DELETE: api/Resource/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Resource>> DeleteResource(Guid id)
        {

            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            var info = _context.ResourceInfo.Where(s => s.Id == resource.ResourceInfoID).FirstOrDefault();

            if (info != null)
            {
                _context.ResourceInfo.Remove(info);
            }

            var provide = _context.R_ProvidesResources.Where(s => s.ResourceID == id).ToList();

            if (provide != null)
            {
                _context.R_ProvidesResources.RemoveRange(provide);
            }

            var channel = _context.R_Channeled.Where(s => s.ResourceID == id).ToList();

            if (channel != null)
            {
                _context.R_Channeled.RemoveRange(channel);
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return resource;
        }


    //inner classes
        public class DetailedResource
        {
            public Guid Id { get; set; }
            public Guid? IncomeID { get; set; }
            public Guid? OutcomeID { get; set; }
            public Guid? RelationID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
        }
        //utils
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
        private bool ResourceExists(Guid id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }
    }
}
