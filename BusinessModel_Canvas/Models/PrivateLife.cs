using System;

namespace BusinessModel_Canvas.Models
{
    public class PrivateLife
    {
        public Guid Id { get; set; }
        public string Interests { get; set; }
        public string BirthPlace { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SchoolGraduated { get; set; }
        public bool IsMarried { get; set; }
        public string SignificantOtherName { get; set; }
        public string[] NameOfChildren { get; set; }
        public int SupportLevel { get; set; }//Does he like us?What is the level of his support
        public string ExpectanceDescription { get; set; }//What do we expect from this client in a brief description
    }
}
