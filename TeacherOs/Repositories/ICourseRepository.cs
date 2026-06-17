
using TeacherOs.Models;

namespace TeacherOs.Repositories
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<List<Student>> GetCourseStudentsAsync(int courseId);
        Task<Teacher?> GetCourseTeacherAsync(int courseId);
    }
}
