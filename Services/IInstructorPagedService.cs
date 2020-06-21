using efStart3.Models;

namespace efStart3.Services
{
    public interface IInstructorPagedService
    {
        PagedList<Instructor> PagedList();
    }
}