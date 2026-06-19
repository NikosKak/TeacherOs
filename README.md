##TeacherOs

An ASP.NET Core MVC web application for managing school users and courses (teachers, students, admins). The codebase follows a clean, layered architecture: Controllers → Services → Repositories (Unit of Work) → EF Core DbContext.

3Why this project is useful


Role- and capability-based access control (Role, Capability, User, Teacher, Student)
Cookie-based authentication with a 30-minute sliding expiration
Course management with teacher/student relationships
Separation of concerns: Controllers → Services → Repositories → DbContext
Custom domain exceptions (EntityNotFoundException, EntityAlreadyExistsException, etc.) for consistent error handling
AutoMapper for DTO ↔ entity mapping; dedicated signup DTOs per role (Admin/Teacher/Student)
Structured logging via Serilog
Localized error messages (English and Greek — el-GR)
Password hashing via BCrypt


#Quick links


Source: TeacherOs/
Entrypoint: TeacherOs/Program.cs
Controllers: TeacherOs/Controllers/ (Home, User, Admin, Teacher, Student)
Models: TeacherOs/Models/ (User, Teacher, Student, Role, Capability, Course)
Data context: TeacherOs/Data/
Repositories: TeacherOs/Repositories/ (with UnitOfWork)
Services: TeacherOs/Services/
DTOs: TeacherOs/DTO/
Custom exceptions: TeacherOs/Exceptions/
Localization resources: TeacherOs/Resources/


#Prerequisites


.NET SDK 10.0 or later (install from https://dotnet.microsoft.com/)
A database instance (e.g. SQL Server, SQLite, PostgreSQL) and a connection string
Optional: Visual Studio 2022/2023, VS Code, or JetBrains Rider for development


#Get started (local development)


Clone the repository


bashgit clone <repo-url>
cd <repo-directory>


Configure the application



Open TeacherOs/appsettings.json (or appsettings.Development.json) and add a connection string named DevConnection — the app reads this specific key in Program.cs:


json{
  "ConnectionStrings": {
    "DevConnection": "Server=localhost;Database=TeacherOs;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}


Restore, build and run


bashdotnet restore
dotnet build
cd TeacherOs
dotnet run


By default the app runs on the URLs reported in the console. Open the browser and navigate to the displayed URL (e.g. https://localhost:5001).



Database migrations



If the project uses EF Core migrations in your workflow, add and apply migrations as usual. Example commands (adjust project and startup project names if required):


bash# Add migration (example)
dotnet ef migrations add InitialCreate --project TeacherOs --startup-project TeacherOs
# Apply migrations
dotnet ef database update --project TeacherOs --startup-project TeacherOs

If you do not use EF migrations, ensure your target database schema is created before running the application.

#Configuration overview


appsettings.json / appsettings.Development.json — application configuration (logging, connection strings)
TeacherOs/Program.cs — application startup and DI registration
TeacherOs/Repositories/ — repository implementations and UnitOfWork
TeacherOs/Services/ — business logic layer

Contributing

Contributions are welcome. Suggested workflow:


Open an issue describing the feature or bug.
Fork the repository and create a feature branch.
Submit a pull request with a clear description and small, focused changes.


Please keep changes scoped and include tests where applicable.
Where to get help

Open an issue in this repository for bugs and feature requests.
Use the repository's code and comments as the primary documentation.

Maintainers

Maintained by the repository owner. For maintainers, review pull requests and issues in the GitHub repository.

Acknowledgements

Bootstrap and jQuery assets are included under their respective licenses in TeacherOs/wwwroot/lib/.
