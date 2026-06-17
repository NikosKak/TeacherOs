using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherOs.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace TeacherOs.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            ClaimsPrincipal? principal = HttpContext.User;

            if (!principal!.Identity!.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
