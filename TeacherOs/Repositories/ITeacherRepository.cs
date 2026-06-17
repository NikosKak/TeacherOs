using TeacherOs.Core;
using TeacherOs.Data;
using TeacherOs.Models;
using System.Linq.Expressions;

namespace TeacherOs.Repositories
{
    public interface ITeacherRepository : IBaseRepository<Teacher>
    {
        Task<List<Course>> GetTeacherCoursesAsync(int teacherId);
        Task<Teacher?> GetByPhoneNumberAsync(string phoneNumber);
        Task<User?> GetUserTeacherByUsernameAsync(string username);
        Task<Teacher?> GetByUserIdAsync(int userId);
        Task<PaginatedResult<User>> GetPaginatedUsersTeachersAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<User>> GetPaginatedUsersTeachersFilteredAsync(int pageNumber, int pageSize, 
            List<Expression<Func<User, bool>>> predicates);
    }
}
