using TeacherOs.Core;
using TeacherOs.DTO;

namespace TeacherOs.Services
{
    public interface ITeacherService
    {
        Task SignUpUserAsync(TeacherSignupDTO request);
        Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedUsersTeachersAsync(int pageNumber, int pageSize);
        Task DeleteTeacherAsync(int userId);
    }
}