using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efStart3.Services;

namespace efStart3.Models.SchoolViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Course> Courses{get;set;}
        public IEnumerable<Enrollment> Enrollments{get;set;}
        public PagedList<Instructor> PagedList{get;set;}   
    }
}