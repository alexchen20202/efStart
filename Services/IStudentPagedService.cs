using efStart3.Models;

namespace efStart3.Services
{
    public interface IStudentPagedService
    {
        PagedList<Student> PagedList();
    }
}
