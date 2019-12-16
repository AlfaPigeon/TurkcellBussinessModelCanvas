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
    public class PartnersController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public PartnersController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Partners
        [HttpGet]
        public async Task<ActionResult<IEnumerable<R_Partners>>> GetR_Partners()
        {
            return await _context.R_Partners.ToListAsync();
        }

        // GET: api/Partners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<R_Partners>> GetR_Partners(Guid id)
        {
            var r_Partners = await _context.R_Partners.FindAsync(id);

            if (r_Partners == null)
            {
                return NotFound();
            }

            return new R_Partners()
            {
                Id = r_Partners.Id,
                FirmID = r_Partners.FirmID,
                Reason = HttpUtility.HtmlEncode(r_Partners.Reason)

            };
        }


        // POST: api/Partners
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<R_Partners>> PostR_Partners([FromForm]Partner partner)
        {
            R_Partners r_Partners = new R_Partners()
            {
                Id = Guid.NewGuid(),
                FirmID = partner.Id,
                Reason = (partner.Reason)
            };

            _context.R_Partners.Add(r_Partners);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetR_Partners", new { id = r_Partners.Id }, r_Partners);
        }


        [HttpPost("Edit")]
        public async Task<ActionResult<R_Partners>> EditPartner([FromForm]Partner partner)
        {

            var firm = await _context.Firms.FindAsync(partner.Id);
            if(firm == null)
            {
                return NotFound();
            }
            var rel = _context.R_Partners.Where(s => s.FirmID == firm.Id).FirstOrDefault();
            if(rel == null)
            {
                return NotFound();
            }
            
            rel.Reason = (partner.Reason);
            
            await _context.SaveChangesAsync();

            return rel;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Partner>> PostPartner([FromForm]Partner partner)
        {
            Firm firm = new Firm
            {
                Id = Guid.NewGuid(),
                Name = (partner.Name),
                Order = (partner.Order),
                RelationshipDescription = (partner.RelationshipDescription),
                Description= (partner.Description)
            };

           
            _context.Firms.Add(firm);

            R_Partners r_ = new R_Partners
            {
                Id = Guid.NewGuid(),
                FirmID = firm.Id,
                Reason = (partner.Reason)
            };

            _context.R_Partners.Add(r_);

            await _context.SaveChangesAsync();

            return partner;
        }

        // DELETE: api/Partners/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<R_Partners>> DeleteR_Partners(Guid id)
        {

            if(!_context.Firms.Any(s => s.Id == id))
            {
                return NotFound();
            }
            
            var r_Partners = await _context.R_Partners.FindAsync(_context.R_Partners.Where(s => s.FirmID == id).Select(s => s.Id).FirstOrDefault());
            if (r_Partners == null)
            {
                return NotFound();
            }

            _context.R_Partners.Remove(r_Partners);
            await _context.SaveChangesAsync();

            return r_Partners;
        }

        private bool R_PartnersExists(Guid id)
        {
            return _context.R_Partners.Any(e => e.Id == id);
        }
        public class Partner : Firm
        {
            public string Reason { get; set; }
        }
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
    }
}
