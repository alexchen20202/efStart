using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using efStart3.DAL;
using efStart3.Models;
using efStart3.Models.SchoolViewModels;
using efStart3.Services;
using efStart3.Models.Params;

namespace efStart3.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;
        private readonly IInstructorPagedService _pagedList;

        public InstructorsController(SchoolContext context,
        IInstructorPagedService pagedList)
        {
            _context = context;
            _pagedList = pagedList;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(
            int? InstructorID, int? CourseID, int pageIndex = 1, string searchString = "")
        {
            InstructorIndexData viewModel = new InstructorIndexData();
            InstructorParam param = new InstructorParam();
            param.PageIndex = pageIndex;
            param.SearchString = searchString;
            viewModel.Param = param;
            
             IQueryable<Instructor> instructors = _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
            .ThenInclude(i => i.Course)
            .ThenInclude(i => i.Department)
            .Include(i => i.CourseAssignments)
            .ThenInclude(i => i.Course)
            .ThenInclude(i => i.Enrollments)
            .ThenInclude(i => i.Student)
            .AsNoTracking()
            .OrderBy(i => i.InstructorID)
            .AsQueryable();
            
            if(!String.IsNullOrEmpty(searchString))
            {
                instructors = instructors.Where(
                    i => i.FirstMidName.Contains(searchString) 
                    || i.LastName.Contains(searchString)
                );
            }

            if(InstructorID != null)
            {

                viewModel.Param.InstructID = InstructorID;
                Instructor instructor = instructors
                .Where(i => i.InstructorID == InstructorID).Single();
                viewModel.Courses = instructor.CourseAssignments
                .Select(c => c.Course);
            }

            if(CourseID != null)
            {
                viewModel.Param.CourseID = CourseID;
                Course course = viewModel.Courses
                .Where(c => c.CourseID == CourseID).Single();
                viewModel.Enrollments = course.Enrollments;
            }
            
            int pageSize = 10;
            PagedList<Instructor> list = _pagedList.PagedList();
            viewModel.PagedList = list.Paging(await instructors.ToListAsync(), pageIndex, pageSize);;

            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.InstructorID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>(){};
            PopulateAssignedCourseData(instructor);
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorID,LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            if(selectedCourses != null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>(){};
                foreach( string courseID in selectedCourses )
                {
                    instructor.CourseAssignments.Add(new CourseAssignment{
                        InstructorID = instructor.InstructorID,
                        CourseID = int.Parse(courseID)
                    });
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor instructor = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
            .ThenInclude(i => i.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.InstructorID == id);

            if (instructor == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(instructor);
            ViewBag.SelectedCourses = new string[]{};
            return View(instructor);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        {

            if (id == null){ return NotFound(); }

            Instructor instructorToUpdate = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
            .ThenInclude(i => i.Course)
            .FirstOrDefaultAsync(s => s.InstructorID == id);

            // if instructor info is complete, update and go to Index
            if (await TryUpdateModelAsync<Instructor>(
                instructorToUpdate, "", i => i.FirstMidName, 
                i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
            {
                // Office can be null
                if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }

                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            // else return to the same view again
            else
            {
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                PopulateAssignedCourseData(instructorToUpdate);
            }
            return View(instructorToUpdate);
    }

        
        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.InstructorID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Instructor instructor = await _context.Instructors
            .Include(i => i.CourseAssignments)
            .SingleAsync(i => i.InstructorID == id);

            List<Department> departments = await _context.Departments
            .Where(d => d.InstructorID == id).ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

            _context.Instructors.Remove(instructor);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorID == id);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            DbSet<Course> allCourse = _context.Courses;
            HashSet<int> instructorCourses = new HashSet<int>(
                instructor.CourseAssignments.Select(c => c.CourseID));
            List<AssignedCourseData> courseData = new List<AssignedCourseData>();
            foreach(var item in allCourse)
            {
                courseData.Add(new AssignedCourseData()
                {
                    CourseID = item.CourseID,
                    CourseTitle = item.Title,
                    Assigned = instructorCourses.Contains(item.CourseID)
                });      
            }
            ViewBag.Courses = courseData;
        }
    
        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructor)
        {
            if(selectedCourses == null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            HashSet<string> selectedCoursesHS = new HashSet<string>(selectedCourses);
            HashSet<int> instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.Course.CourseID));

            foreach (Course course in _context.Courses)
            {
                // if this course has not be assigned by the instructor
                // add this course to CourseAssignments data
                if (selectedCoursesHS.Contains(course.CourseID.ToString()) && !instructorCourses.Contains(course.CourseID))
                {
                    instructor.CourseAssignments.Add(new CourseAssignment { 
                        InstructorID = instructor.InstructorID, 
                        CourseID = course.CourseID });
                }
                // if instructor has assigned this course but cancel now 
                // remove this CourseAssignment data
                if ( !selectedCoursesHS.Contains(course.CourseID.ToString()) && instructorCourses.Contains(course.CourseID))
                {
                        CourseAssignment courseToRemove = instructor.CourseAssignments
                            .FirstOrDefault(i => i.CourseID == course.CourseID);
                        _context.Remove(courseToRemove);
                }
            }

        }

    }
}
