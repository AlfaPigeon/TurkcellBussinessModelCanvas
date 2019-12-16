using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditChannelModel : PageModel
    {
        private readonly Canvas_Context _context;
        private Guid ChannelID;
        private string Name, Description;
        private Guid? IncomeID,OutcomeID;
        public EditChannelModel(Canvas_Context context)
        {
            _context = context;
        }

        public void OnGet(Guid Channel)
        {
            ChannelID = Channel;
            var channel = _context.ShippingChannels.Where(s => s.Id == ChannelID).Select(s => s).FirstOrDefault();
            if(channel == null)
            {
                Response.Redirect("/404");
            }
            Description = channel.Description;
            Name = channel.Name;
            IncomeID = channel.IncomeID;
            OutcomeID = channel.OutcomeID;

        }
        public List<Tuple<Guid, string>> GetAllIncome()
        {
            List<Tuple<Guid, string>> income = _context.IncomeFlows.OrderBy(s => s.Name).Select(s => new Tuple<Guid, string>(s.Id, s.Name)).ToList();
            return income;
        }

        public List<Tuple<Guid, string>> GetAllOutcome()
        {
            List<Tuple<Guid, string>> outcome = _context.OutcomeFlows.OrderBy(s => s.Name).Select(s => new Tuple<Guid, string>(s.Id, s.Name)).ToList();
            return outcome;
        }

        public Guid? GetIncome() { return IncomeID; }
        public Guid? GetOutcome() { return OutcomeID; }
        public string GetName() { return Name; }
        public string GetDescription() { return Description; }
        public Guid GetChannelID() { return ChannelID; }
    }
}