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
    public class IncomeController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public IncomeController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Income
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeFlow>>> GetIncomeFlows()
        {
            return await _context.IncomeFlows.ToListAsync();
        }

        // GET: api/Income/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeFlow>> GetIncomeFlow(Guid id)
        {
            var incomeFlow = await _context.IncomeFlows.FindAsync(id);

            if (incomeFlow == null)
            {
                return NotFound();
            }

            return new IncomeFlow()
            {
                Id = incomeFlow.Id,
                GuessedAmount = incomeFlow.GuessedAmount,
                RealAmount = incomeFlow.RealAmount,
                Name = HttpUtility.HtmlEncode(incomeFlow.Name),
                Description = HttpUtility.HtmlEncode(incomeFlow.Description)
            }; ;
        }


        // GET: api/Income/5
        [HttpGet("Resource/{id}")]
        public ActionResult<IncomeFlow> GetIncomeFlowByResourceID(Guid id)
        {
            if (!_context.Resources.Any(s => s.Id == id))
            {
                return NotFound();
            }

            IncomeFlow flow = (from res in _context.Resources where res.Id == id join info in _context.ResourceInfo on res.ResourceInfoID equals info.Id into table1
                               from tb1 in table1.DefaultIfEmpty() join income in _context.IncomeFlows on tb1.IncomeID equals income.Id
                               select new IncomeFlow() {               
                               Id= income.Id,
                               Name= HttpUtility.HtmlEncode(income.Name),
                               Description= HttpUtility.HtmlEncode(income.Description),
                               GuessedAmount = income.GuessedAmount,
                               RealAmount = income.RealAmount  
                               }
                                 ).FirstOrDefault();

            return flow;
        }



        // GET: api/Income/5
        [HttpGet("Channel/{id}")]
        public ActionResult<IncomeFlow> GetIncomeFlowByChannelID(Guid id)
        {
            if (!_context.ShippingChannels.Any(s => s.Id == id))
            {
                return NotFound();
            }

            IncomeFlow flow = (from ch in _context.ShippingChannels
                               where ch.Id == id
                               join income in _context.IncomeFlows on ch.IncomeID equals income.Id
                               select new IncomeFlow()
                               {
                                   Id = income.Id,
                                   Name = HttpUtility.HtmlEncode(income.Name),
                                   Description = HttpUtility.HtmlEncode(income.Description),
                                   GuessedAmount = income.GuessedAmount,
                                   RealAmount = income.RealAmount
                               }
                                 ).FirstOrDefault();

            return flow;
        }




        // GET: api/Income/5
        [HttpGet("Relation/{id}")]
        public ActionResult<IncomeFlow> GetIncomeFlowByRelationID(Guid id)
        {
            if (!_context.CustomerRelations.Any(s => s.Id == id))
            {
                return NotFound();
            }

            IncomeFlow flow = (from cus in _context.CustomerRelations
                               where cus.Id == id 
                               join income in _context.IncomeFlows on cus.IncomeID equals income.Id
                               select new IncomeFlow()
                               {
                                   Id = income.Id,
                                   Name = HttpUtility.HtmlEncode(income.Name),
                                   Description = HttpUtility.HtmlEncode(income.Description),
                                   GuessedAmount = income.GuessedAmount,
                                   RealAmount = income.RealAmount
                               }
                                 ).FirstOrDefault();

            return flow;
        }


        // POST: api/Income
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<IncomeFlow>> PostIncomeFlow([FromForm]IncomeFlow incomeFlow)
        {

            _context.IncomeFlows.Add(incomeFlow);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncomeFlow", new { id = incomeFlow.Id }, incomeFlow);
        }
        [HttpPost("Edit")]
        public async Task<ActionResult<IncomeFlow>> EditIncomeFlow([FromForm]IncomeFlow incomeFlow)
        {
            var name = (incomeFlow.Name);
            if (name == null || name == "") return BadRequest();

            var income = await _context.IncomeFlows.FindAsync(incomeFlow.Id);
            if (income == null) return NotFound();

            income.Name = name;
            income.Description = (incomeFlow.Description);
            income.GuessedAmount = incomeFlow.GuessedAmount;
            income.RealAmount = incomeFlow.RealAmount;

            await _context.SaveChangesAsync();

            return income;
        }
        // DELETE: api/Income/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IncomeFlow>> DeleteIncomeFlow(Guid id)
        {
            var incomeFlow = await _context.IncomeFlows.FindAsync(id);
            if (incomeFlow == null)
            {
                return NotFound();
            }

            _context.IncomeFlows.Remove(incomeFlow);
            await _context.SaveChangesAsync();

            return incomeFlow;
        }
        //utils
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
        private bool IncomeFlowExists(Guid id)
        {
            return _context.IncomeFlows.Any(e => e.Id == id);
        }
    }
}
