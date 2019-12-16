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
    public class ChannelController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public ChannelController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Channel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<R_Channeled>>> GetR_Channeled()
        {
            return await _context.R_Channeled.ToListAsync();
        }

        // GET: api/Channel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<R_Channeled>> GetR_Channeled(Guid id)
        {
            var r_Channeled = await _context.R_Channeled.FindAsync(id);

            if (r_Channeled == null)
            {
                return NotFound();
            }
   
            return r_Channeled;
        }


        [HttpGet("Ch/{id}")]
        public async Task<ActionResult<Channel>> GetChannel(Guid id)
        {
            var channel = await _context.ShippingChannels.FindAsync(id);

            if (channel == null)
            {
                return NotFound();
            }
            
            return new Channel() {

                Id = channel.Id,
                Name = HttpUtility.HtmlEncode(channel.Name),
                Description = HttpUtility.HtmlEncode(channel.Description),
                IncomeID = channel.IncomeID,
                OutcomeID = channel.OutcomeID
            };
        }

        // GET: api/Channel/Resource/5
        [HttpGet("Resource/{id}")]
        public async Task<ActionResult<List<Channel>>> GetChannelsByResourceID(Guid id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound();
            }

            List<Channel> channels =
                (from channeled in _context.R_Channeled where channeled.ResourceID == id
                 join channel in _context.ShippingChannels on channeled.ChannelID equals channel.Id
                 select new Channel()
                 {
                     Id=channel.Id,
                     Name= HttpUtility.HtmlEncode(channel.Name),
                     Description= HttpUtility.HtmlEncode(channel.Description)
                 }
                ).ToList();


            return channels;
        }



        // POST: api/Channel
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Channel>> PostR_Channeled([FromForm]Channel channel)
        {
            if (channel.Name == null || channel.Name == "") return BadRequest();
            Channel ch = new Channel()
            {
                Id = Guid.NewGuid(),
                Name = channel.Name,
                Description = channel.Description,
                IncomeID = channel.IncomeID,
                OutcomeID = channel.OutcomeID
            };
            
            _context.ShippingChannels.Add(ch);
            await _context.SaveChangesAsync();

            return channel;
        }
        [HttpPost("Edit")]
        public async Task<ActionResult<Channel>> EditChannel([FromForm]Channel channel)
        {
            var name = channel.Name;
            if(name == null || name == "")
            {
                return BadRequest();
            }

            var cha = await _context.ShippingChannels.FindAsync(channel.Id);
            if(cha == null)
            {
                return NotFound();
            }
            if (channel.IncomeID != null && !_context.IncomeFlows.Any(s => s.Id == channel.IncomeID)) return NotFound();
            if (channel.OutcomeID != null && !_context.OutcomeFlows.Any(s => s.Id == channel.OutcomeID)) return NotFound();
            cha.Name = channel.Name;
            cha.Description = channel.Description;
            cha.IncomeID = channel.IncomeID;
            cha.OutcomeID = channel.OutcomeID;

            

            await _context.SaveChangesAsync();

            return cha;
        }

        [HttpPost("Add/{ResourceID}/{id}")]
        public async Task<ActionResult<Channel>> AddChanneloResource(Guid ResourceID, Guid id)
        {
            var resource = await _context.Resources.FindAsync(ResourceID);
            if(resource == null)
            {
                return NotFound();
            }
            var channel = await _context.ShippingChannels.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }
            R_Channeled channeled = (from ch in _context.R_Channeled where ch.ChannelID == id && ch.ResourceID == ResourceID select ch).FirstOrDefault();

            if (channeled != null)
            {
                return Conflict("Already added!");
            }

            channeled = new R_Channeled()
            {
                Id = Guid.NewGuid(),
                ResourceID = ResourceID,
                ChannelID = id

            };
           
            _context.R_Channeled.Add(channeled);
            await _context.SaveChangesAsync();

            return channel;
        }
        // DELETE: api/Channel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Channel>> DeleteR_Channeled(Guid id)
        {
            var channel = await _context.ShippingChannels.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }
            var channeled = _context.R_Channeled.Where(s => s.ChannelID == id).ToList();

            if (channeled != null)
            {
                _context.R_Channeled.RemoveRange(channeled);
            }
            _context.ShippingChannels.Remove(channel);
            await _context.SaveChangesAsync();

            return channel;
        }
        // DELETE: api/Employee/Remove/5/4
        [HttpDelete("Remove/{ResourceID}/{id}")]
        public ActionResult<R_Channeled> RemoveChannelFromResource(Guid ResourceID, Guid id)
        {
            R_Channeled channeled = (from i in _context.R_Channeled where i.ResourceID == ResourceID && i.ChannelID == id select i).FirstOrDefault();
            if (channeled == null)
            {
                return NotFound();
            }

            _context.Remove(channeled);
            _context.SaveChanges();

            return channeled;
        }
        //utils
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
        private bool R_ChanneledExists(Guid id)
        {
            return _context.R_Channeled.Any(e => e.Id == id);
        }
    }
}
