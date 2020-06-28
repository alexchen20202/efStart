using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using efStart3.DAL;
using efStart3.Models;
using efStart3.Models.SchoolViewModels;
using efStart3.Services;

namespace efStart3.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;
        private readonly IStudentPagedService _pagedList;
        
        public StudentsController(SchoolContext context, IStudentPagedService pagedList)
        {
            _context = context;
            _pagedList = pagedList;
        }

        // GET: Students
        public async Task<IActionResult> Index(
            int? StudentID, 
            int? CourseID,
            string sortString = "", 
            string searchString = "", 
            int pageIndex = 1)
        {
            ViewBag.SearchString = searchString;
            ViewBag.SortString = sortString;
            ViewBag.SortLastName = (sortString == "lastName_desc") ? "lastName" : "lastName_desc";        
            ViewBag.SortID = (sortString == "id_desc") ? "id" : "id_desc";
            ViewBag.SortEnrollDate = (sortString == "enrollDate_desc") ? "enrollDate" : "enrollDate_desc";
            var viewModel = new StudentIndexData();

            IQueryable<Student> students = _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(s => s.Course)
            .ThenInclude(s => s.Department)
            .Include(s => s.Enrollments)
            .ThenInclude(s => s.Course)            
            .ThenInclude(s => s.CourseAssignments)
            .ThenInclude(s => s.Instructor)
            .AsNoTracking()
            .AsQueryable();

            if(!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(
                    s => s.FirstName.Contains(searchString)
                    || s.LastName.Contains(searchString)
                );
            }

            if(StudentID != null)
            {
                ViewData["StudentID"] = StudentID;
                Student student = students
                .Where(i => i.StudentId == StudentID).Single();
                viewModel.Courses = student.Enrollments
                .Select(c => c.Course);
            }

            if(CourseID != null)
            {
                ViewData["CourseID"] = CourseID;
                Course course = viewModel.Courses
                .Where(c => c.CourseID == CourseID).FirstOrDefault();
                viewModel.CourseAssignments = course.CourseAssignments;
            }

            switch(sortString)
            {
                case "lastName_desc":
                students = students.OrderByDescending(s => s.LastName);
                break;
                case "lastName":
                students = students.OrderBy(s => s.LastName);
                break;
                case "id_desc":
                students = students.OrderByDescending(s => s.StudentId);
                break;
                case "id":
                students = students.OrderBy(s => s.StudentId);
                break;
                case "enrollDate_desc":
                students = students.OrderByDescending(s => s.EnrollmentDate);
                break;
                case "enrollDate":
                students = students.OrderBy(s => s.EnrollmentDate);
                break;
                default :
                students = students.OrderBy(s => s.StudentId);
                break;
            }
            
            int pageSize = 10;
            PagedList<Student> list = _pagedList.PagedList();
            viewModel.PagedList = list.Paging(await students.ToListAsync(), pageIndex, pageSize);
            
            return View(viewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,EnrollmentDate")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
