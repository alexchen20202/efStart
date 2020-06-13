using efStart3.Models;
namespace efStart3.Services
{
    public class StudentPagedService: IStudentPagedService
    {
        public PagedList<Student> PagedList()
        {
            return new PagedList<Student>();
        } 
    }
}