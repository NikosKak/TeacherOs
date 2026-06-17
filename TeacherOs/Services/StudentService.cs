using AutoMapper;
using Serilog;
using TeacherOs.Core;
using TeacherOs.DTO;
using TeacherOs.Exceptions;
using TeacherOs.Models;
using TeacherOs.Repositories;
using TeacherOs.Security;

namespace TeacherOs.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger = new LoggerFactory().AddSerilog().CreateLogger<StudentService>();
        private readonly IEncryptionUtil _encryptionUtil;
        public StudentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentService> logger, IEncryptionUtil encryptionUtil)
        {
            _encryptionUtil = encryptionUtil;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SignUpUserAsync(StudentSignupDTO request)
        {
            Student student = _mapper.Map<Student>(request);
            User user = _mapper.Map<User>(request);
            student.Am = Guid.NewGuid().ToString("N")[..8].ToUpper();
            student.Department = Guid.NewGuid().ToString("N")[..8].ToUpper();
            try
            {
                User? existingUser = await _unitOfWork.UserRepository
                    .GetUserByUsernameAsync(user.Username);

                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException("User", "User with username " +
                        existingUser.Username + " already exists");
                }
                user.Password = _encryptionUtil.EncryptPassword(user.Password);
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveAsync();
                student.UserId = user.Id;
                await _unitOfWork.StudentRepository.AddAsync(student);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Student {Student} signed up successfully.", student);
            }
            catch (EntityAlreadyExistsException ex)
            {
                _logger.LogError("Error signing up student {Student}. {Message}", student, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error signing up student {Student}. {Message}", student, ex.Message);
                throw;
            }
        }
        public async Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedStudentsAsync(int pageNumber, int pageSize)
        {
            var result = await _unitOfWork.StudentRepository.GetPaginatedUsersStudentsAsync(pageNumber, pageSize);

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
            _logger.LogInformation("Retrieved {Count} users-students", dtoResult.Data.Count);
            return dtoResult;
        }
        public async Task DeleteStudentAsync(int userId)
        {
            try
            {
                var student = await _unitOfWork.StudentRepository
                    .GetByUserIdAsync(userId);
                if (student == null)
                    throw new Exception("Student not found");

                await _unitOfWork.StudentRepository.DeleteAsync(student.Id);
                await _unitOfWork.UserRepository.DeleteAsync(userId);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Student with userId {UserId} deleted.", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting student {UserId}. {Message}", userId, ex.Message);
                throw;
            }
        }
    }
}
