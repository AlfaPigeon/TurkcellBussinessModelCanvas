using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusinessModel_Canvas.Pages
{
    public class EditEmployeeModel : PageModel
    {
        private readonly Canvas_Context _context;
        private Guid EmployeeID;
        private Guid? FirmID;
        private string Name, Position, Interests, School, BirthPlace, Significant, Description, CellPhone, OfficePhone, HomeAddress, OfficeAddress;
        private DateTime? BirthDate;
        private string[] Children;
        private bool IsMarried = false;
        private int Support = 0;


        public EditEmployeeModel(Canvas_Context context)
        {
            _context = context;
        }
        public void OnGet(Guid Employee)
        {
            EmployeeID = Employee;
            var emp = _context.Employees.Where(s => s.Id == EmployeeID).Select(s => s).FirstOrDefault();
            if (emp == null) Response.Redirect("/404");
            //normal
            Name = emp.Name;
            Position = emp.Position;
            //employee info
            var info = _context.PrivateLifes.Where(s => s.Id == emp.PrivateID).Select(s => s).FirstOrDefault();
            if(info != null)
            {
                Description = info.ExpectanceDescription;
                Interests = info.Interests;
                Children = info.NameOfChildren;
                School = info.SchoolGraduated;
                BirthDate = info.BirthDate;
                BirthPlace = info.BirthPlace;
                Significant = info.SignificantOtherName;
                IsMarried = info.IsMarried;
                Support = info.SupportLevel;
            }
            //comunication
            var com = _context.Communications.Where(s => s.Id == emp.CommunicationID).Select(s => s).FirstOrDefault();
            if (com != null)
            {
                OfficeAddress = com.OfficeAddress;
                HomeAddress = com.HomeAddress;
                CellPhone = com.CellNumber;
                OfficePhone = com.OfficeNumber;
            }
            //works
            var work = _context.R_Works.Where(s => s.EmployeeID == emp.Id).Select(s => s).FirstOrDefault();
            if(work != null)
            {
                FirmID = work.FirmID;
            }
        }

        public List<Tuple<Guid, string>> GetAllFirms()
        {
            List<Tuple<Guid, string>> firms = (
              from firm in _context.Firms
              select new Tuple<Guid, string>(firm.Id, firm.Name)).ToList();

            return firms;
        }

        public string GetName() { return Name; }
        public string GetPosition() { return Position; }
        public string GetInterests() { return Interests; }
        public string GetSchool() { return School; }
        public string GetBirthPlace() { return BirthPlace; }
        public string GetSignificant() { return Significant; }
        public string GetDescription() { return Description; }
        public string GetCellPhone() { return CellPhone; }
        public string GetOfficePhone() { return OfficePhone; }
        public string GetHomeAddress() { return HomeAddress; }
        public string GetOfficeAddress() { return OfficeAddress; }
        public Guid? GetFirmID() { return FirmID; }
        public DateTime? GetBirthDate() { return BirthDate; }
        public string[] GetChildren() { return Children; }
        public bool GetIsMarried() { return IsMarried; }
        public int GetSupport() { return Support; }
        public Guid GetEmployeeID() { return EmployeeID; }

    }
}