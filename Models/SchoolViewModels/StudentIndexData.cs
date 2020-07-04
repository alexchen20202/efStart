using System.Collections.Generic;
using efStart3.Services;
using efStart3.Models.Params;

namespace efStart3.Models.SchoolViewModels
{
    public class StudentIndexData
    {
        public IEnumerable<Course> Courses{get;set;}
        public IEnumerable<CourseAssignment> CourseAssignments{get;set;}
        public PagedList<Student> PagedList{get;set;}
        public StudentParam Param{get;set;}
    }
}