using efStart3.Models;
using efStart3.Services;

namespace efStart3.Services
{
    public class InstructorPagedService: IInstructorPagedService
    {
        public PagedList<Instructor> PagedList()
        {
            return new PagedList<Instructor>();
        } 
    }
}