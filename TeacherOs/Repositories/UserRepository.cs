using System.Linq.Expressions;
using TeacherOs.Core;
using TeacherOs.Data;
using TeacherOs.Models;
using Microsoft.EntityFrameworkCore;
namespace TeacherOs.Repositories
{
    public class UserRepository : BaseRepository<User>,IUserRepository
    {
        public UserRepository(SchoolOsContext context) : base(context)
        {

        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username || u.Email == username);
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
        }
        public  async Task<PaginatedResult<User>> GetUsersAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            IQueryable<User> query = _context.Users;
            if (predicates != null && predicates.Count > 0)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }
            int totalRecords = await query.CountAsync();
            int skip = (pageNumber - 1) * pageSize;

            var data = await query
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<User>(data, totalRecords, pageNumber, pageSize);
        }
    }
}
