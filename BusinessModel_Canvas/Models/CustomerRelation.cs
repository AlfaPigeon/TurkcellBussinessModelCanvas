using System;

namespace BusinessModel_Canvas.Models
{
    public class CustomerRelation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? IncomeID { get; set; }
        public Guid? OutcomeID { get; set; }
    }
}
