using System;

namespace BusinessModel_Canvas.Models
{
    public class R_ProvidesResource
    {
        public Guid Id { get; set; }
        public Guid ServiceID { get; set; }
        public string Description { get; set; }
        public Guid ResourceID { get; set; }

    }
}
