using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFWorkforce.Data;
using EFWorkforce.Models;

namespace EFWorkforce.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Employee.Include(e => e.Computer).Include(e => e.Department); 
            // never ever have applicationDbContext variable name!
            var employees = _context.Employee.Include(e => e.Department);
            return View(await employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Computer)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id); // equivalent WHERE return employee or null
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee); // passing data to view
        }

        // GET: Employees/Create
        public IActionResult Create()
        {

            var computers = _context.Computer.Where(c => c.DecomissionDate == null && c.Employee == null );

            //ViewData["ComputerList"] = new SelectList(_context.Computer, "Id", "Make"); // to pass extra stuff to view
            ViewData["ComputerList"] = new SelectList( computers, "Id", "Make");
            ViewData["DepartmentList"] = new SelectList(_context.Department, "Id", "Name"); //  to set list of departments in view ViewBag in view - extra data for view
            ViewData["Anything"] = "Man, tacos are goood!"; // ViewData is Dictionary
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,IsSupervisor,DepartmentId,ComputerId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Make", employee.ComputerId);
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["ComputerList"] = new SelectList(_context.Computer, "Id", "Make", employee.ComputerId);
            ViewData["DepartmentList"] = new SelectList(_context.Department, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind forses to have only indicated names in query to prevent insertion hacking - to prevent overposting attack
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,IsSupervisor,DepartmentId,ComputerId")] Employee employee)
        {
            if (id != employee.Id) // employee data comes from body of request
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Make", employee.ComputerId);
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", employee.DepartmentId);
            ViewData["ErrorMessage"] = "You are goofed!";
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Computer)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
