using Microsoft.EntityFrameworkCore;
using TeacherOs.Core;
using TeacherOs.Data;
using TeacherOs.Models;
using System.Linq.Expressions;

namespace TeacherOs.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolOsContext context) : base(context)
        {
        }

        public async Task<Student?> GetByAm(string? am)
        {
            return await _context.Students
                .Where(s => s.Am == am)
                .SingleOrDefaultAsync();
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersStudentsAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            var usersWithRoleStudent = await _context.Users
                .Include(u => u.Student)
                .Where(u => u.Student != null)   
                .Skip(skip)
                .Include(u => u.Role)
                .Take(pageSize)
                .ToListAsync();

            int totalRecords = await _context.Users
                .Where(u => u.Student != null)
                .CountAsync();

            return new PaginatedResult<User>(usersWithRoleStudent, totalRecords, pageNumber, pageSize);
        }

        public async Task<PaginatedResult<Student>> GetPaginatedUsersStudentsFilteredAsync(int pageNumber, 
            int pageSize, List<Expression<Func<Student, bool>>> predicates)
        {
            IQueryable<Student> query = _context.Students;
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

            return new PaginatedResult<Student>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<Student?> GetByUserIdAsync(int userId)
        {
            return await _context.Students
                .Where(s => s.UserId == userId)
                .SingleOrDefaultAsync();
        }
        public async Task<List<Course>> GetStudentCoursesAsync(int studentId)
        {
            List<Course> courses;

            courses = await _context.Students
                .Where(s => s.Id == studentId)
                .SelectMany(c => c.Courses)
                .ToListAsync();

            return courses;
        }
    }
}
