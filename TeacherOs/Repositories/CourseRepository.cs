using Microsoft.EntityFrameworkCore;
using TeacherOs.Data;
using TeacherOs.Models;

namespace TeacherOs.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(SchoolOsContext context) : base(context)
        {
        }

        public async Task<List<Student>> GetCourseStudentsAsync(int courseId)
        {
            return await _context.Courses
               .Where(c => c.Id == courseId)
               .SelectMany(c => c.Students)
               .ToListAsync();
        }
        public async Task<Teacher?> GetCourseTeacherAsync(int courseId)
        {
           
            var course = await _context.Courses
                    .Include(c => c.Teacher)
                    .FirstOrDefaultAsync(c => c.Id == courseId);

            return course?.Teacher;
        }
    }
}