using System;

namespace BusinessModel_Canvas.Models
{
    public class OutcomeFlow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? GuessedAmount { get; set; }
        public decimal? RealAmount { get; set; }
    }
}
