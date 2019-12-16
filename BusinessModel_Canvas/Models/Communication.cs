using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessModel_Canvas.Models
{
    public class Communication
    {
        public Guid Id { get; set; }
        public string CellNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string HomeAddress { get; set; }
        public string OfficeAddress { get; set; }
    }
}
