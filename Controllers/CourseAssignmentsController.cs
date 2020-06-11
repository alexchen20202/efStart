using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using efStart3.DAL;
using efStart3.Models;

namespace efStart3.Controllers
{
    public class CourseAssignmentsController : Controller
    {
        private readonly SchoolContext _context;

        public CourseAssignmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: CourseAssignments
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.CourseAssignments
            .Include(c => c.Course)
            .Include(c => c.Instructor);
            return View(await schoolContext.ToListAsync());
        }

        // GET: CourseAssignments/Details/5
        public async Task<IActionResult> Details(int? InstructorID, int? CourseID)
        {
            if (InstructorID == null || CourseID == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignments
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(
                    m => m.CourseID == CourseID 
                    && m.InstructorID == InstructorID);
            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // GET: CourseAssignments/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title");
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "InstructorID", "FullName");
            return View();
        }

        // POST: CourseAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorID,CourseID")] CourseAssignment courseAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "InstructorID", "FullName", courseAssignment.InstructorID);
            return View(courseAssignment);
        }

        // GET: CourseAssignments/Edit/5
        public async Task<IActionResult> Edit(int? InstructorID, int? CourseID)
        {
            if (InstructorID == null || CourseID == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignments
            .FirstOrDefaultAsync(
                c => c.InstructorID == InstructorID && c.CourseID == CourseID);
            if (courseAssignment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "InstructorID", "FullName", courseAssignment.InstructorID);
            return View(courseAssignment);
        }

        // POST: CourseAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorID,CourseID")] CourseAssignment courseAssignment)
        {
            if (id != courseAssignment.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseAssignmentExists(courseAssignment.CourseID))
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
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID", courseAssignment.CourseID);
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "InstructorID", "FullName", courseAssignment.InstructorID);
            return View(courseAssignment);
        }

        // GET: CourseAssignments/Delete/5
        public async Task<IActionResult> Delete(int? InstructorID, int? CourseID)
        {
            if (InstructorID == null || CourseID == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignments
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.CourseID == CourseID 
                && c.InstructorID == InstructorID);
            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // POST: CourseAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? InstructorID, int? CourseID)
        {
            var courseAssignment = await _context.CourseAssignments
            .FirstOrDefaultAsync(c => c.InstructorID == InstructorID 
                && c.CourseID == CourseID);
            _context.CourseAssignments.Remove(courseAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseAssignmentExists(int id)
        {
            return _context.CourseAssignments.Any(e => e.CourseID == id);
        }
    }
}
