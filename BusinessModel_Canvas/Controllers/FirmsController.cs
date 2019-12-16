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
    public class FirmsController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public FirmsController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Firms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Firm>>> GetFirms()
        {
            return await _context.Firms.ToListAsync();
        }

        // GET: api/Firms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Firm>> GetFirm(Guid id)
        {
            var firm = await _context.Firms.FindAsync(id);

            if (firm == null)
            {
                return NotFound();
            }
            firm.Name = HttpUtility.HtmlEncode(firm.Name);
            firm.Description = HttpUtility.HtmlEncode(firm.Description);
            firm.RelationshipDescription = HttpUtility.HtmlEncode(firm.RelationshipDescription);
            firm.Order = HttpUtility.HtmlEncode(firm.Order);
            return firm;
        }


        // POST: api/Firms
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Firm>> PostFirm([FromForm]Firm firm)
        {
            firm.Id = Guid.NewGuid();
            firm.Name = (firm.Name);
            firm.Description = (firm.Description);
            firm.Order = (firm.Order);
            firm.RelationshipDescription = (firm.RelationshipDescription);
           
            _context.Firms.Add(firm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFirm", new { id = firm.Id }, firm);
        }

        [HttpPost("Edit")]
        public async Task<ActionResult<Firm>> EditFirm([FromForm]Firm firm)
        {
            if (firm.Name == null || firm.Name == "") return BadRequest();
            var Nfirm = await _context.Firms.FindAsync(firm.Id);
            if (Nfirm == null)
                return NotFound();

            Nfirm.Name = (firm.Name);
            Nfirm.Description = (firm.Description);
            Nfirm.RelationshipDescription = (firm.RelationshipDescription);
            Nfirm.Order = (firm.Order);

            await _context.SaveChangesAsync();

            return Nfirm;
        }



        // DELETE: api/Firms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Firm>> DeleteFirm(Guid id)
        {
            if(id == Guid.Parse("0f2e1299-0f25-48be-a9b1-1c971422d60d")) { return Forbid(); }
            var firm = await _context.Firms.FindAsync(id);
            if (firm == null)
            {
                return NotFound();
            }
            List < R_Works > list= (from work in _context.R_Works where work.FirmID == id select work).ToList();
            List<R_Partners> partners = (from partner in _context.R_Partners where partner.FirmID == id select partner).ToList();
            List<R_ProvidesServices> provides = (from prov in _context.R_ProvidesServices where prov.ProviderID == id || prov.ClientID == id select prov).ToList();

            _context.R_Works.RemoveRange(list);
            _context.R_Partners.RemoveRange(partners);
            _context.R_ProvidesServices.RemoveRange(provides);
            _context.Firms.Remove(firm);
            await _context.SaveChangesAsync();

            return firm;
        }
        //utils
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
        
        private bool FirmExists(Guid id)
        {
            return _context.Firms.Any(e => e.Id == id);
        }
        public List<string> HtmlEncodeArray(string[] arr)
        {
            List<string> list = new List<string>();
            foreach (string s in arr) list.Add(HttpUtility.HtmlEncode(s));
            return list;
        }
    }
}
