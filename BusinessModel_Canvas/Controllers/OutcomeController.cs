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
    public class OutcomeController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public OutcomeController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Outcome
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutcomeFlow>>> GetOutcomeFlows()
        {
            return await _context.OutcomeFlows.ToListAsync();
        }

        // GET: api/Outcome/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OutcomeFlow>> GetOutcomeFlow(Guid id)
        {
            var outcomeFlow = await _context.OutcomeFlows.FindAsync(id);

            if (outcomeFlow == null)
            {
                return NotFound();
            }


            return new OutcomeFlow() {
                Id = outcomeFlow.Id,
                GuessedAmount = outcomeFlow.GuessedAmount,
                RealAmount = outcomeFlow.RealAmount,
                Name = HttpUtility.HtmlEncode(outcomeFlow.Name),
                Description = HttpUtility.HtmlEncode(outcomeFlow.Description)
            };
        }


        [HttpGet("Resource/{id}")]
        public ActionResult<OutcomeFlow> GetIncomeFlowByResourceID(Guid id)
        {

            if(!_context.Resources.Any(s => s.Id == id))
            {
                return NotFound();
            }
            
            OutcomeFlow flow = (from res in _context.Resources
                                where res.Id == id
                                join info in _context.ResourceInfo on res.ResourceInfoID equals info.Id into table1
                                from tb1 in table1.DefaultIfEmpty()
                                join outcome in _context.OutcomeFlows on tb1.OutcomeID equals outcome.Id 
                               select new OutcomeFlow()
                               {
                                   Id = outcome.Id,
                                   Name = HttpUtility.HtmlEncode(outcome.Name),
                                   Description = HttpUtility.HtmlEncode(outcome.Description),
                                   GuessedAmount = outcome.GuessedAmount,
                                   RealAmount = outcome.RealAmount
                               }
                                 ).FirstOrDefault();

            return flow;
        }
        // GET: api/Income/5
        [HttpGet("Channel/{id}")]
        public ActionResult<OutcomeFlow> GetOutcomeFlowByChannelID(Guid id)
        {
            if (!_context.ShippingChannels.Any(s => s.Id == id))
            {
                return NotFound();
            }

            OutcomeFlow flow = (from ch in _context.ShippingChannels
                               where ch.Id == id
                               join outcome in _context.OutcomeFlows on ch.OutcomeID equals outcome.Id
                               select new OutcomeFlow()
                               {
                                   Id = outcome.Id,
                                   Name = HttpUtility.HtmlEncode(outcome.Name),
                                   Description = HttpUtility.HtmlEncode(outcome.Description),
                                   GuessedAmount = outcome.GuessedAmount,
                                   RealAmount = outcome.RealAmount
                               }
                                 ).FirstOrDefault();

            return flow;
        }
        // GET: api/Income/5
        [HttpGet("Relation/{id}")]
        public ActionResult<IncomeFlow> GetOutcomeFlowByRelationID(Guid id)
        {
            if (!_context.CustomerRelations.Any(s => s.Id == id))
            {
                return NotFound();
            }

            IncomeFlow flow = (from cus in _context.CustomerRelations
                               where cus.Id == id
                               join outcome in _context.OutcomeFlows on cus.OutcomeID equals outcome.Id
                               select new IncomeFlow()
                               {
                                   Id = outcome.Id,
                                   Name = HttpUtility.HtmlEncode(outcome.Name),
                                   Description = HttpUtility.HtmlEncode(outcome.Description),
                                   GuessedAmount = outcome.GuessedAmount,
                                   RealAmount = outcome.RealAmount
                               }
                                 ).FirstOrDefault();

            return flow;
        }


        // POST: api/Outcome
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<OutcomeFlow>> PostOutcomeFlow([FromForm]OutcomeFlow outcomeFlow)
        {
            outcomeFlow.Name = (outcomeFlow.Name);
            outcomeFlow.Description = (outcomeFlow.Description);
            _context.OutcomeFlows.Add(outcomeFlow);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOutcomeFlow", new { id = outcomeFlow.Id }, outcomeFlow);
        }

        [HttpPost("Edit")]
        public async Task<ActionResult<OutcomeFlow>> EditOutcomeFlow([FromForm]OutcomeFlow outcomeFlow)
        {
            var name = (outcomeFlow.Name);
            if (name == null || name == "") return BadRequest();

            var outcome = await _context.OutcomeFlows.FindAsync(outcomeFlow.Id);
            if (outcome == null) return NotFound();

            outcome.Name = name;
            outcome.Description = (outcomeFlow.Description);
            outcome.GuessedAmount = outcomeFlow.GuessedAmount;
            outcome.RealAmount = outcomeFlow.RealAmount;

            await _context.SaveChangesAsync();

            return outcome;
        }

        // DELETE: api/Outcome/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OutcomeFlow>> DeleteOutcomeFlow(Guid id)
        {
            var outcomeFlow = await _context.OutcomeFlows.FindAsync(id);
            if (outcomeFlow == null)
            {
                return NotFound();
            }

            _context.OutcomeFlows.Remove(outcomeFlow);
            await _context.SaveChangesAsync();

            return outcomeFlow;
        }
        //utils
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
        private bool OutcomeFlowExists(Guid id)
        {
            return _context.OutcomeFlows.Any(e => e.Id == id);
        }
    }
}
