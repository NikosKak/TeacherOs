
using TeacherOs.Data;

namespace TeacherOs.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolOsContext _context;
        public IUserRepository UserRepository { get; }
        public ITeacherRepository TeacherRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public ICourseRepository CourseRepository { get; }

        public UnitOfWork(SchoolOsContext context)
        {
            _context = context;
            UserRepository = new UserRepository(context);
            StudentRepository = new StudentRepository(context);
            TeacherRepository = new TeacherRepository(context);
            CourseRepository = new CourseRepository(context);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
