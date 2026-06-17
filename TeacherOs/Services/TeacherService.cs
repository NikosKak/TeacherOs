using AutoMapper;
using TeacherOs.Core;
using TeacherOs.DTO;
using TeacherOs.Exceptions;
using TeacherOs.Models;
using TeacherOs.Repositories;
using TeacherOs.Security;

namespace TeacherOs.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEncryptionUtil _encryptionUtil;
        private readonly ILogger<TeacherService> _logger;

        public TeacherService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TeacherService> logger, IEncryptionUtil encryptionUtil)
        {
            _encryptionUtil = encryptionUtil;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedUsersTeachersAsync(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.TeacherRepository.GetPaginatedUsersTeachersAsync(pageNumber, pageSize);

            var dtoResult = new PaginatedResult<UserReadOnlyDTO>()
            {
                Data = result.Data.Select(u => new UserReadOnlyDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    UserRole = u.Role.Name
                }).ToList(),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
            _logger.LogInformation("Retrieved {Count} users-teachers", dtoResult.Data.Count);
            return dtoResult;
        }
        public async Task SignUpUserAsync(TeacherSignupDTO request)
        {
            Teacher teacher = _mapper.Map<Teacher>(request);
            User user = _mapper.Map<User>(request);

            try
            {
                User? existingUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(user.Username);

                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException("User", "User with username " +
                        existingUser.Username + " already exists");
                }

                user.Teacher = teacher;
                user.Password = _encryptionUtil.EncryptPassword(user.Password);
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.TeacherRepository.AddAsync(teacher);

                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Teacher {Teacher} signed up successfully.", teacher);
            }
            catch (EntityAlreadyExistsException ex)
            {
                _logger.LogError("Error signing up tecaher {Teacher}. {Message}", teacher, ex.Message);
                throw;
            }
        }
        public async Task DeleteTeacherAsync(int userId)
        {
            try
            {
                var teacher = await _unitOfWork.TeacherRepository
                    .GetByUserIdAsync(userId);
                if (teacher == null)
                    throw new Exception("Teacher not found");

                await _unitOfWork.TeacherRepository.DeleteAsync(teacher.Id);
                await _unitOfWork.UserRepository.DeleteAsync(userId);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Teacher with userId {UserId} deleted.", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting teacher {UserId}. {Message}", userId, ex.Message);
                throw;
            }
        }
    }
}
