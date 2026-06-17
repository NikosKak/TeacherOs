using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherOs.DTO;
using TeacherOs.Services;

namespace TeacherOs.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly IApplicationService _applicationService;

        public AdminController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var students = await _applicationService.StudentService
                .GetPaginatedStudentsAsync(pageNumber, pageSize);
            var teachers = await _applicationService.TeacherService
                .GetPaginatedUsersTeachersAsync(pageNumber, pageSize);

            ViewData["Students"] = students;
            ViewData["Teachers"] = teachers;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _applicationService.StudentService.DeleteStudentAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await _applicationService.TeacherService.DeleteTeacherAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _applicationService.UserService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _applicationService.UserService.UpdateUserAsync(dto);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(AdminSignupDTO adminSignupDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(adminSignupDTO);
            }

            try
            {
                await _applicationService.UserService.AdminSignUpAsync(adminSignupDTO);
                return RedirectToAction("Login", "User");
            }
            catch (Exception e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View(adminSignupDTO);
            }
        }
    }
}