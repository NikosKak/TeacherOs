using TeacherOs.DTO;
using TeacherOs.Core;
using TeacherOs.Models;
using TeacherOs.Core.Filters;

namespace TeacherOs.Services
{
    public interface IUserService
    {
        Task<User?> VerifyAndGetAsync(UserLoginDTO credentials);
        Task<UserReadOnlyDTO?> GetUserByUsernameAsync(string username);
        Task<PaginatedResult<UserReadOnlyDTO>> GetUsersByUsernameFilteredAsync(
            int pageNumber,
            int page,
            UserFiltersDTO userFiltrsDTO
        );
        Task<UserEditDTO?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(UserEditDTO dto);
        public interface IUserService
        {
            Task<User> VerifyAndGetAsync(UserLoginDTO credentials);
            Task<UserEditDTO?> GetUserByIdAsync(int id);
            Task UpdateUserAsync(UserEditDTO dto);
            Task AdminSignUpAsync(AdminSignupDTO request);
        }
        Task AdminSignUpAsync(AdminSignupDTO request);
    }
}
