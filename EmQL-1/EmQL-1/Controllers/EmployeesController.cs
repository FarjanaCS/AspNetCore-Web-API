using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmQL_1.Models;

namespace EmQL_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeDbCOntext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeesController(EmployeeDbCOntext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }
        // ////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("Qualification/Include")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeeswithQualifications()
        {
            return await _context.Employees.Include(x=>x.Qualifications).ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }
        // //////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("Qualification/Include/{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeswithQualifications(int id)
        {
            var employee = await _context.Employees.Include(x=> x.Qualifications).FirstOrDefaultAsync(x=> x.EmployeeId==id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }
            // //////////////////////////////////////////////////////////////////////////////////////////
            var eq= await _context.Employees.Include(x => x.Qualifications).FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (eq== null)
            {
               return NotFound();
            }
            eq.Name= employee.Name;
            eq.Gender= employee.Gender;
            eq.JoiningDate= employee.JoiningDate;
            eq.Address= employee.Address;   
            eq.IsaCurrentEmployee= employee.IsaCurrentEmployee;
            eq.Salary= employee.Salary;
            eq.Picture= employee.Picture;
            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Qualifications WHERE EmployeeId={id}");
            foreach(var q in employee.Qualifications)
            {
                _context.Qualifications.Add(new Qualification { PassingYear=q.PassingYear, Degree=q.Degree , EmployeeId=id});
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }
        // //////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost("image/Upload")]
        public async Task<ActionResult<string>> Upload(IFormFile pic)
        {
            string ext = Path.GetExtension(pic.FileName);
            string f= Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+ext;
            string savepath = Path.Combine(_env.WebRootPath, "Pictures", f);
            FileStream fs= new FileStream(savepath, FileMode.Create);
            await pic.CopyToAsync(fs);
            fs.Close();
            return f;
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
