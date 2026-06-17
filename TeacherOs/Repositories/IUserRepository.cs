using System.Linq.Expressions;
using TeacherOs.Models;
using TeacherOs.Core;
namespace TeacherOs.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<PaginatedResult<User>> GetUsersAsync(int pageNumber, int pageSize,
        List<Expression<Func<User,bool>>> predicates);
        Task<User?> GetByIdAsync(int id);


    }
}
