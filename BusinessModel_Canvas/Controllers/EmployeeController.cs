using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessModel_Canvas.Data;
using BusinessModel_Canvas.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web;

namespace BusinessModel_Canvas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly Canvas_Context _context;

        public EmployeeController(Canvas_Context context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            
            if (employee == null)
            {
                return NotFound();
            }
            
            return employee;
        }
        // GET: api/Employee/5
        [HttpGet("Relation/{id}")]
        public async Task<ActionResult<List<EmployeeInfo>>> GetEmployeeByRelationID(Guid id)
        {
            var relation = await _context.CustomerRelations.FindAsync(id);

            if (relation == null)
            {
                return NotFound();
            }

            List<EmployeeInfo> employees =
                (from rels in _context.R_Relations where rels.RelationID == id join rel in _context.CustomerRelations on rels.RelationID equals rel.Id into table
                 from t in table.DefaultIfEmpty() join  emp in _context.Employees on rels.EmployeeID equals emp.Id into table1
                 from tb in table1.DefaultIfEmpty() join priv in _context.PrivateLifes on tb.PrivateID equals priv.Id into table2
                 from tb2 in table2.DefaultIfEmpty() join com in _context.Communications on tb.CommunicationID equals com.Id into table3
                 from tb3 in table3.DefaultIfEmpty()
                 select new EmployeeInfo()
                 {
                    Id = tb.Id,
                    Name = HttpUtility.HtmlEncode(tb.Name),
                    Position= HttpUtility.HtmlEncode(tb.Position),
                    Description= HttpUtility.HtmlEncode(tb2.ExpectanceDescription),
                    Phone=tb3.OfficeNumber,
                    Support=tb2.SupportLevel

                 }
                 ).ToList();


            return employees;
        }
        // GET: api/Firm/Employee/5
        [HttpGet("Firm/{id}")]
        public ActionResult<List<EmployeeInfo>> GetEmployeesOfFirm(Guid id)
        {
            List<EmployeeInfo> EmployeesOfFirm = (
                    from work in _context.R_Works
                    where work.FirmID == id
                    join emp in _context.Employees on work.EmployeeID equals emp.Id into table1
                    from tb1 in table1.DefaultIfEmpty()
                    join priv in _context.PrivateLifes on tb1.PrivateID equals priv.Id into table2
                    from tb2 in table2.DefaultIfEmpty()
                    join comm in _context.Communications on tb1.CommunicationID equals comm.Id into table3
                    from tb3 in table3.DefaultIfEmpty()
                    select new EmployeeInfo()
                    {
                        Id = tb1.Id,
                        Name = HttpUtility.HtmlEncode(tb1.Name),
                        Description = HttpUtility.HtmlEncode(tb2.ExpectanceDescription),
                        Position = HttpUtility.HtmlEncode(tb1.Position),
                        Phone = tb3.OfficeNumber,
                        Support = tb2.SupportLevel
                    }).ToList();

            return EmployeesOfFirm;
        }


        // GET: api/Service/Employee/5
        [HttpGet("Service/{id}")]
        public ActionResult<List<EmployeeInfo>> GetEmployeesOfService(Guid id)
        {


            var employees = (from prov in _context.Services
                             where prov.Id == id
                             join res in _context.R_ProvidesResources on prov.Id equals res.ServiceID into table
                             from tb in table.DefaultIfEmpty()
                             join res1 in _context.Resources on tb.ResourceID equals res1.Id into table1
                             from tb1 in table1.DefaultIfEmpty()
                             join resinfo in _context.ResourceInfo on tb1.ResourceInfoID equals resinfo.Id into table2
                             from tb2 in table2.DefaultIfEmpty()
                             join customer in _context.CustomerRelations on tb2.CostumerRelationID equals customer.Id into table3
                             from tb3 in table3.DefaultIfEmpty()
                             join rel in _context.R_Relations on tb3.Id equals rel.RelationID into table4
                             from tb4 in table4.DefaultIfEmpty()
                             join emp in _context.Employees on tb4.EmployeeID equals emp.Id
                             select emp);




            List<EmployeeInfo> EmployeesOfFirm = (
                from emp in employees
                join comu in _context.Communications on emp.CommunicationID equals comu.Id into table1
                from comu in table1.DefaultIfEmpty()
                join priv in _context.PrivateLifes on emp.PrivateID equals priv.Id into table2
                from priv in table2.DefaultIfEmpty()
                select new EmployeeInfo()
                {
                    Id = emp.Id,
                    Name = HttpUtility.HtmlEncode(emp.Name),
                    Description = HttpUtility.HtmlEncode(priv.ExpectanceDescription),
                    Position = HttpUtility.HtmlEncode(emp.Position),
                    Phone = comu.OfficeNumber,
                    Support = priv.SupportLevel
                }).OrderBy(s => s.Name).Distinct().ToList();

            return EmployeesOfFirm;
        }
        // GET: api/Employee/5
        [HttpGet("info/{id}")]
        public ActionResult<EmployeeDetailedInfo> GetEmployeeInfo(Guid id)
        {
      

            if (!_context.Employees.Any(s=>s.Id==id))
            {
                return NotFound();
            }

            EmployeeDetailedInfo info =
                (
                 from emp in _context.Employees
                 where emp.Id == id
                 join priv in _context.PrivateLifes on emp.PrivateID equals priv.Id into table2
                 from tb2 in table2.DefaultIfEmpty()
                 join com in _context.Communications on emp.CommunicationID equals com.Id into table3
                 from tb3 in table3.DefaultIfEmpty()
                 select new EmployeeDetailedInfo()
                 {
                     Id = emp.Id,
                     Name = HttpUtility.HtmlEncode(emp.Name),
                     Position = HttpUtility.HtmlEncode(emp.Position),
                     Description = HttpUtility.HtmlEncode(tb2.ExpectanceDescription),
                     School = HttpUtility.HtmlEncode(tb2.SchoolGraduated),
                     BirthDate = tb2.BirthDate,
                     BirthPlace = HttpUtility.HtmlEncode(tb2.BirthPlace),
                     OfficeAddress = HttpUtility.HtmlEncode(tb3.OfficeAddress),
                     OfficePhone = tb3.OfficeNumber,
                     CellPhone = tb3.CellNumber,
                     HomeAddress = HttpUtility.HtmlEncode(tb3.HomeAddress),
                     Significant = HttpUtility.HtmlEncode(tb2.SignificantOtherName),
                     Children = tb2.NameOfChildren,
                     Interests = HttpUtility.HtmlEncode(tb2.Interests),
                     Support = tb2.SupportLevel,
                     IsMarried = tb2.IsMarried
                 }
                 ).FirstOrDefault();


            return info;
        }
       
        public List<string> HtmlEncodeArray(string[] arr)
        {
            List<string> list = new List<string>();
            foreach (string s in arr) list.Add(HttpUtility.HtmlEncode(s));
            return list;
        }



        [HttpPost("Add/ToFirm/{firmID}/{id}")]
        public ActionResult<EmployeeInfo> AddEmployeeToFirm(Guid firmID, Guid id)
        {

            if (!EmployeeExists(id))
            {
                return NotFound("Employee does not exist");
            }
            if (!_context.Firms.Any(e => e.Id == firmID))
            {
                return NotFound("Firm does not exist");
            }

           
            if(_context.R_Works.Any(e => e.FirmID == firmID && e.EmployeeID == id))
            {
                return Conflict();
            }

         

            R_Works works = new R_Works() {
                Id = Guid.NewGuid(),
                FirmID=firmID,
                EmployeeID=id
            };


            _context.R_Works.Add( works);
             _context.SaveChanges();

           
            EmployeeInfo employeeInfo = (from work in _context.R_Works where work.FirmID == firmID
                join emp in _context.Employees on work.EmployeeID equals emp.Id into table1 
                from tb1 in table1.DefaultIfEmpty() where tb1.Id == id
                join priv in _context.PrivateLifes on tb1.PrivateID equals priv.Id into table2
                from tb2 in table2.DefaultIfEmpty()
                join comm in _context.Communications on tb1.CommunicationID equals comm.Id into table3
                from tb3 in table3.DefaultIfEmpty()
                select new EmployeeInfo()
                {
                    Id = tb1.Id,
                    Name = tb1.Name,
                    Description = tb2.ExpectanceDescription,
                    Position = tb1.Position,
                    Phone = tb3.OfficeNumber,
                    Support = tb2.SupportLevel
                }).First();
           
            return employeeInfo;
        }

        [HttpPost("Add/ToRelation/{RelationID}/{id}")]
        public ActionResult<EmployeeInfo> AddEmployeeToRelation(Guid RelationID, Guid id)
        {
            if (!EmployeeExists(id))
            {
                return NotFound("Employee does not exist");
            }
            if (!_context.CustomerRelations.Any(e => e.Id == RelationID))
            {
                return NotFound("Relation does not exist");
            }


            if (_context.R_Relations.Any(e => e.RelationID == RelationID && e.EmployeeID == id))
            {
                return Conflict();
            }

           R_Relations r_relation = new R_Relations()
            {
                Id = Guid.NewGuid(),
                EmployeeID = id,
                RelationID = RelationID
            };
            _context.R_Relations.Add(r_relation);
            _context.SaveChanges();

            EmployeeInfo employeeInfo = (
                                         from emp in _context.Employees 
                                         where emp.Id == id
                                         join priv in _context.PrivateLifes on emp.PrivateID equals priv.Id into table2
                                         from tb2 in table2.DefaultIfEmpty()
                                         join comm in _context.Communications on emp.CommunicationID equals comm.Id into table3
                                         from tb3 in table3.DefaultIfEmpty()
                                         select new EmployeeInfo()
                                         {
                                             Id = emp.Id,
                                             Name = emp.Name,
                                             Description = tb2.ExpectanceDescription,
                                             Position = emp.Position,
                                             Phone = tb3.OfficeNumber,
                                             Support = tb2.SupportLevel
                                         }).First();

            return employeeInfo;
        }



        [HttpPost]
        public async Task<ActionResult<EmployeeDetailedInfo>> PostEmployee([FromForm]EmployeeDetailedInfo info)
        {
            string[] children = null;

            if (info.Children != null) { 
            children = new string[info.Children.Length];
            for (int i = 0; i < info.Children.Length; i++)
            {
                children[i] = RemoveSpecials(info.Children[i]);
            }
                                        }

            PrivateLife privateLife = new PrivateLife()
            {
                Id = Guid.NewGuid(),
                Interests = RemoveSpecials(info.Interests),
                ExpectanceDescription = RemoveSpecials(info.Description),
                BirthDate = info.BirthDate,
                BirthPlace = RemoveSpecials(info.BirthPlace),
                NameOfChildren = children,
                IsMarried = info.IsMarried,
                SignificantOtherName = RemoveSpecials(info.Significant),
                SupportLevel = info.Support%6,
                SchoolGraduated = RemoveSpecials(info.School)
             };

            await _context.PrivateLifes.AddAsync(privateLife);
            Communication communication = new Communication()
            {
                Id = Guid.NewGuid(),
                OfficeAddress = RemoveSpecials(info.OfficeAddress),
                HomeAddress = RemoveSpecials(info.HomeAddress),
                CellNumber = (info.CellPhone != null)? Regex.Replace(info.CellPhone, "[^0-9.]", ""):null,
                OfficeNumber = (info.OfficePhone != null) ? Regex.Replace(info.OfficePhone, "[^0-9.]", ""):null
            };
            await _context.Communications.AddAsync(communication);
            Employee employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = RemoveSpecials(info.Name),
                Position = RemoveSpecials(info.Position),
                CommunicationID = communication.Id,
                PrivateID = privateLife.Id
            };
            await _context.Employees.AddAsync(employee);


            EmployeeDetailedInfo employeeDetailed = new EmployeeDetailedInfo()
            {
                Id = employee.Id,
                Name = employee.Name,
                Position = employee.Position,
                Description = privateLife.ExpectanceDescription,
                School = privateLife.SchoolGraduated,
                BirthDate = privateLife.BirthDate,
                BirthPlace = privateLife.BirthPlace,
                OfficeAddress = communication.OfficeAddress,
                OfficePhone = communication.OfficeNumber,
                CellPhone = communication.CellNumber,
                HomeAddress = communication.HomeAddress,
                Significant = privateLife.SignificantOtherName,
                Children = privateLife.NameOfChildren,
                Interests = privateLife.Interests,
                Support = privateLife.SupportLevel,
                IsMarried = privateLife.IsMarried
            };
            await _context.SaveChangesAsync();


            return employeeDetailed;
        }


        [HttpPost("Edit")]
        public async Task<ActionResult<EmployeeDetailedInfo>> EditEmployee([FromForm]EmployeeDetailedInfo info)
        {
            var name = info.Name;
            if (name == null || name == "") return NotFound();

            var emp = await _context.Employees.FindAsync(info.Id);
            if (emp == null) return NotFound();

            string[] children = null;

            if (info.Children != null)
            {
                children = new string[info.Children.Length];
                for (int i = 0; i < info.Children.Length; i++)
                {
                    children[i] = info.Children[i];
                }
            }
            
            emp.Name = info.Name;
            emp.Position = info.Position;

            var privateLife = await _context.PrivateLifes.FindAsync(emp.PrivateID);
            if(privateLife != null)
            {
                privateLife.Interests = info.Interests;
                privateLife.ExpectanceDescription = info.Description;
                privateLife.BirthDate = info.BirthDate;
                privateLife.BirthPlace = info.BirthPlace;
                privateLife.NameOfChildren = children;
                privateLife.IsMarried = info.IsMarried;
                privateLife.SignificantOtherName = (info.Significant);
                privateLife.SupportLevel = info.Support % 6;
                privateLife.SchoolGraduated = (info.School);
            }

            var communication = await _context.Communications.FindAsync(emp.CommunicationID);
            if (communication != null)
            {
                communication.OfficeAddress = (info.OfficeAddress);
                communication.HomeAddress = (info.HomeAddress);
                communication.CellNumber = (info.CellPhone != null) ? Regex.Replace(info.CellPhone, "[^0-9.]", "") : null;
                communication.OfficeNumber = (info.OfficePhone != null) ? Regex.Replace(info.OfficePhone, "[^0-9.]", "") : null;
            };




            await _context.SaveChangesAsync();

            EmployeeDetailedInfo employeeDetailed = new EmployeeDetailedInfo()
            {
                Id = emp.Id,
                Name = emp.Name,
                Position = emp.Position,
                Description = privateLife.ExpectanceDescription,
                School = privateLife.SchoolGraduated,
                BirthDate = privateLife.BirthDate,
                BirthPlace = privateLife.BirthPlace,
                OfficeAddress = communication.OfficeAddress,
                OfficePhone = communication.OfficeNumber,
                CellPhone = communication.CellNumber,
                HomeAddress = communication.HomeAddress,
                Significant = privateLife.SignificantOtherName,
                Children = privateLife.NameOfChildren,
                Interests = privateLife.Interests,
                Support = privateLife.SupportLevel,
                IsMarried = privateLife.IsMarried
            };
            


            return employeeDetailed;
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            
            List<R_Relations> r_ = (from rel in _context.R_Relations where rel.EmployeeID == id select rel).ToList();
            List<R_Works> r_Works = (from work in _context.R_Works where work.EmployeeID == id select work).ToList();
            List<Communication> communication = (from com in _context.Communications where com.Id == employee.CommunicationID select com).ToList();
            List<PrivateLife> priv = (from pr in _context.PrivateLifes where pr.Id == employee.PrivateID select pr).ToList();


            _context.R_Relations.RemoveRange(r_);
            _context.R_Works.RemoveRange(r_Works);
            _context.Communications.RemoveRange(communication);
            _context.PrivateLifes.RemoveRange(priv);

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }
        // DELETE: api/Employee/Remove/5/4
        [HttpDelete("Remove/{FirmID}/{id}")]
        public  ActionResult<R_Works> DeleteEmployeeFromFirm(Guid FirmID, Guid id)
        {
            R_Works employee = (from i in _context.R_Works where i.EmployeeID == id && i.FirmID == FirmID select i).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }

            _context.Remove(employee);
             _context.SaveChanges();

            return employee;
        }
        

                    // DELETE: api/Employee/Remove/5/4
        [HttpDelete("Remove/FromRelation/{RelationID}/{id}")]
        public ActionResult<R_Relations> DeleteEmployeeFromRelation(Guid RelationID, Guid id)
        {
            R_Relations relation = (from i in _context.R_Relations where i.EmployeeID == id && i.RelationID == RelationID select i).FirstOrDefault();
            if (relation == null)
            {
                return NotFound();
            }

            _context.R_Relations.Remove(relation);
            _context.SaveChanges();

            return relation;
        }
        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        //Inner Classes
        public class EmployeeInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Position { get; set; }
            public string Phone { get; set; }
            public int Support { get; set; }

        }

        public class EmployeeDetailedInfo 
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Position { get; set; }

            public string School { get; set; }

            public string Significant { get; set; }

            public string[] Children { get; set; }
            public string BirthPlace { get; set; }
            public DateTime? BirthDate { get; set; }
            public bool IsMarried { get; set; }
            public string Interests { get; set; }
            public int Support { get; set; }

            public string CellPhone { get; set; }
            public string OfficePhone { get; set; }
            public string OfficeAddress{ get; set; }
            public string HomeAddress { get; set; }
        }

        //utility

        public string RemoveSpecials(string s)
        {
            if (s == null) return s;
            return Regex.Replace(s, @"[\[\]\\\^\\\|\\*\\(\)\{\}><#&\\]", "");
        }

    }
}
