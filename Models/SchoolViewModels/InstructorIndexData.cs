using System.Collections.Generic;
using efStart3.Services;
using efStart3.Models.Params;

namespace efStart3.Models.SchoolViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Course> Courses{get;set;}
        public IEnumerable<Enrollment> Enrollments{get;set;}
        public PagedList<Instructor> PagedList{get;set;}   
        public InstructorParam Param{get;set;}
    }
}