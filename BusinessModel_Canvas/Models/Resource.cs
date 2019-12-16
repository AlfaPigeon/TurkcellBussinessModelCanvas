using System;

namespace BusinessModel_Canvas.Models
{
    public class Resource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ResourceInfoID { get; set; }
        public bool IsEvent { get; set; }
    }
}
