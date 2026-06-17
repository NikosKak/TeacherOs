using TeacherOs.Core;
using TeacherOs.Data;
using TeacherOs.Models;
using System.Linq.Expressions;

namespace TeacherOs.Repositories
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<List<Course>> GetStudentCoursesAsync(int studentId);
        Task<Student?> GetByAm(string? am);
        Task<Student?> GetByUserIdAsync(int userId);
        Task<PaginatedResult<User>> GetPaginatedUsersStudentsAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<Student>> GetPaginatedUsersStudentsFilteredAsync(int pageNumber, int pageSize,
            List<Expression<Func<Student, bool>>> predicates);
    }
}
