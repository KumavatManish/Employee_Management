using Employee_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _employContext;
        public EmployeeController(EmployeeContext employeeContext)
        {
            _employContext = employeeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if (_employContext.Employees == null)
            {
                return NotFound();
            }
            return await _employContext.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_employContext.Employees == null)
            {
                return NotFound();
            }
            var employee = await _employContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _employContext.Employees.Add(employee);
            await _employContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.ID)
            {
                return BadRequest();
            }
            _employContext.Entry(employee).State = EntityState.Modified;
            try
            {
                await _employContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if (_employContext.Employees == null)
            {
                return NotFound();
            }
            var employee = await _employContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _employContext.Employees.Remove(employee);
            await _employContext.SaveChangesAsync();

            return Ok();
        }
    }
}