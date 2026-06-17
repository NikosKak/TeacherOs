using TeacherOs.Core;
using TeacherOs.DTO;
namespace TeacherOs.Services
{
    public interface IStudentService
    {
        Task SignUpUserAsync(StudentSignupDTO request);
        Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedStudentsAsync(int pageNumber, int pageSize);
        Task DeleteStudentAsync(int userId);
    }
}