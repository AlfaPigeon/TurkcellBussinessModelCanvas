using System;

namespace BusinessModel_Canvas.Models
{
    public class ResourceInfo
    {
        public Guid Id { get; set; }
        public Guid? CostumerRelationID { get; set; }
        public Guid? IncomeID { get; set; }
        public Guid? OutcomeID { get; set; }
    }
}
