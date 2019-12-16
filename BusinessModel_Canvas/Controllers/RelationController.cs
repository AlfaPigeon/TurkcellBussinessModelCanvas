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
    public class RelationController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public RelationController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Relation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerRelation>>> GetCustomerRelations()
        {
            return await _context.CustomerRelations.ToListAsync();
        }

        // GET: api/Relation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerRelation>> GetCustomerRelation(Guid id)
        {
            var customerRelation = await _context.CustomerRelations.FindAsync(id);

            if (customerRelation == null)
            {
                return NotFound();
            }

            return new CustomerRelation() {
                Id = customerRelation.Id,
                Name = HttpUtility.HtmlEncode(customerRelation.Name),
                Description = HttpUtility.HtmlEncode(customerRelation.Description),
                IncomeID = customerRelation.IncomeID,
                OutcomeID = customerRelation.OutcomeID
            };
        }




        [HttpGet("Resource/{id}")]
        public async Task<ActionResult<CustomerRelation>> GetRelationByResourceID(Guid id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound();
            }
            CustomerRelation relation = (
                from res in _context.Resources where res.Id == id join info in _context.ResourceInfo on res.ResourceInfoID equals info.Id
                into table1
                from tb1 in table1.DefaultIfEmpty()
                join rel in _context.CustomerRelations on tb1.CostumerRelationID equals rel.Id
                select new CustomerRelation()
                {
                    Id=rel.Id,
                    Name= HttpUtility.HtmlEncode(rel.Name),
                    Description= HttpUtility.HtmlEncode(rel.Description)
                }
                
                ).FirstOrDefault();


            if (relation == null)
            {
                return NotFound();
            }

            

            return relation;
        }


        // POST: api/Relation
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CustomerRelation>> PostCustomerRelation([FromForm]CustomerRelation customerRelation)
        {

            

            if (customerRelation.IncomeID != null && !_context.IncomeFlows.Any(s => s.Id == customerRelation.IncomeID))
            {
                return NotFound();
            }
            if (customerRelation.OutcomeID != null && !_context.OutcomeFlows.Any(s => s.Id == customerRelation.OutcomeID))
            {
                return NotFound();
            }
            CustomerRelation relation = new CustomerRelation()
            {
                Id = Guid.NewGuid(),
                Name = (customerRelation.Name),
                Description = (customerRelation.Description),
                IncomeID = customerRelation.IncomeID,
                OutcomeID = customerRelation.OutcomeID
            };

            _context.CustomerRelations.Add(relation);
            await _context.SaveChangesAsync();

            return relation;
        }
        [HttpPost("Edit")]
        public async Task<ActionResult<CustomerRelation>> EditCustomerRelation([FromForm]CustomerRelation customerRelation)
        {
            var name = (customerRelation.Name);
            if (name == null || name == "") return BadRequest();

            var custom = await _context.CustomerRelations.FindAsync(customerRelation.Id);
            if (custom == null) return NotFound();


            if (customerRelation.IncomeID != null && !_context.IncomeFlows.Any(s => s.Id == customerRelation.IncomeID))
            {
                return NotFound();
            }
            if (customerRelation.OutcomeID != null && !_context.OutcomeFlows.Any(s => s.Id == customerRelation.OutcomeID))
            {
                return NotFound();
            }


            custom.Name = name;
            custom.Description = (customerRelation.Description);
            custom.IncomeID = customerRelation.IncomeID;
            custom.OutcomeID = customerRelation.OutcomeID;

            await _context.SaveChangesAsync();

            return custom;
        }

        // DELETE: api/Relation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerRelation>> DeleteCustomerRelation(Guid id)
        {
            var customerRelation = await _context.CustomerRelations.FindAsync(id);
            if (customerRelation == null)
            {
                return NotFound();
            }
            List<R_Relations> r_ = (from rel in _context.R_Relations where rel.RelationID == id select rel).ToList();
            _context.R_Relations.RemoveRange(r_);
            _context.CustomerRelations.Remove(customerRelation);
            await _context.SaveChangesAsync();

            return customerRelation;
        }

        private bool CustomerRelationExists(Guid id)
        {
            return _context.CustomerRelations.Any(e => e.Id == id);
        }
        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }
    }
}
