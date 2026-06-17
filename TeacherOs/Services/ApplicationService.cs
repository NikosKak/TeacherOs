using TeacherOs.Services;
namespace TeacherOs.Services
{
    public class ApplicationService : IApplicationService
    {
        public IUserService UserService { get; set; }
        public ITeacherService TeacherService { get; set; }
        public IStudentService StudentService { get; set; }
        public ApplicationService(IUserService userServices, ITeacherService teacherService, IStudentService studentService)
        {
            UserService = userServices;
            TeacherService = teacherService;
            StudentService = studentService;
        }
    }
}
