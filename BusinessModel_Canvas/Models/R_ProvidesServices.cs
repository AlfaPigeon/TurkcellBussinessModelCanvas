using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessModel_Canvas.Models
{
    public class R_ProvidesServices
    {
        public Guid Id { get; set; }
        public Guid ProviderID { get; set; }
        public Guid ClientID { get; set; }
        public string Reason { get; set; }
        public Guid ServiceID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
