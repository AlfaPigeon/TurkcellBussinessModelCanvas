using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessModel_Canvas.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public Guid PrivateID { get; set; }
        public Guid CommunicationID { get; set; }
    }
}
