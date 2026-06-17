using AutoMapper;
using System.Linq.Expressions;
using TeacherOs.Core;
using TeacherOs.Core.Filters;
using TeacherOs.DTO;
using TeacherOs.Exceptions;
using TeacherOs.Models;
using TeacherOs.Repositories;
using TeacherOs.Security;
namespace TeacherOs.Services
{
    public class UserService : IUserService
    {
        private readonly IEncryptionUtil _encryptionUtil;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<UserService> logger, IEncryptionUtil encryptionUtil)
        {
            _encryptionUtil = encryptionUtil;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<PaginatedResult<UserReadOnlyDTO>> GetUsersByUsernameFilteredAsync(
            int pageNumber,
            int pageSize,
            UserFiltersDTO userFiltrsDTO
        )
        {
            List<User> users = [];
            List<Expression<Func<User, bool>>> predicates = [];
            if (!string.IsNullOrEmpty(userFiltrsDTO.Username))
            {
                predicates.Add(u => u.Username==userFiltrsDTO.Username);
            }
            if (!string.IsNullOrEmpty(userFiltrsDTO.Email))
            {
                predicates.Add(u => u.Email==userFiltrsDTO.Email);
            }
            if (!string.IsNullOrEmpty(userFiltrsDTO.UserRole))
            {
                predicates.Add(u => u.Role.Name==userFiltrsDTO.UserRole);
            }
            var result = await _unitOfWork.UserRepository.GetUsersAsync(
                pageNumber,
                pageSize,
                predicates
            );
            var dtoResult = new PaginatedResult<UserReadOnlyDTO>()
            {
                Data = _mapper.Map<List<UserReadOnlyDTO>>(result.Data),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
            _logger.LogInformation("Retrieved {Count} users with filters: {@Filters}", dtoResult.Data.Count, userFiltrsDTO);
            return dtoResult;
        }
        public async Task AdminSignUpAsync(AdminSignupDTO request)
        {
            try
            {
                User? existingUser = await _unitOfWork.UserRepository
                    .GetUserByUsernameAsync(request.Username!);

                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException("User", "User with username " +
                        existingUser.Username + " already exists");
                }

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    Password = _encryptionUtil.EncryptPassword(request.Password!),
                    RoleId = 1
                };

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Admin {Username} signed up successfully.", user.Username);
            }
            catch (EntityAlreadyExistsException ex)
            {
                _logger.LogError("Error signing up admin. {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error signing up admin. {Message}", ex.Message);
                throw;
            }
        }
        public async Task<UserReadOnlyDTO?> GetUserByUsernameAsync(string username)
        {
            try
            {
                User? user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    throw new EntityNotFoundException("User", "User with username:" + "not found");
                }
                _logger.LogInformation("Retrieved user with username: {Username}", username);
                return _mapper.Map<UserReadOnlyDTO>(user);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError("Error retrieving user with username: {Username}. Exception: {ExceptionMessage}", username, ex.Message);
                throw;
            }
        }
        public async Task<User?> VerifyAndGetAsync(UserLoginDTO credentials)
        {
            User? user = null;
            try
            {
                user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(credentials.Username);
                if(user == null|| !_encryptionUtil.VerifyPassword(credentials.Password, user.Password))
                {
                    throw new EntityNotAuthorizedException("User", Resources.ErrorMessages.BadCredentials);
                }
                _logger.LogInformation("User {Username} successfully authenticated", credentials.Username);
            }
            catch(EntityNotAuthorizedException ex) {
                _logger.LogError("Error authenticating user {Username}. Exception: {ExceptionMessage}", credentials.Username, ex.Message);
            }
            return user;
        }
        public async Task<UserEditDTO?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return null;
            return new UserEditDTO
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname
            };
        }

        public async Task UpdateUserAsync(UserEditDTO dto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(dto.Id);
            if (user == null) throw new Exception("User not found");
            user.Firstname = dto.Firstname;
            user.Lastname = dto.Lastname;
            await _unitOfWork.SaveAsync();
        }

    }
}
