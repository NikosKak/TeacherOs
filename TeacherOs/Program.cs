using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TeacherOs.Data;
using Serilog;
using TeacherOs.Repositories;
using TeacherOs.Security;
using TeacherOs.Services;

namespace TeacherOs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connString = builder.Configuration.GetConnectionString("DevConnection");
            builder.Services.AddDbContext<SchoolOsContext>(options => options.UseSqlServer(connString));

            builder.Services.AddSingleton<IEncryptionUtil, EncryptionUtil>();
            builder.Services.AddRepositories();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();

            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<Configuration.MapperConfig>());
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/User/Login";
                   options.AccessDeniedPath = "/Home/AccessDenied";
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                   options.SlidingExpiration = true;   // reset timeout, 30 min of idle
               });

            builder.Services.AddAuthorizationBuilder()
                .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets().AllowAnonymous();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}