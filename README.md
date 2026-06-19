# TeacherOs

An ASP.NET Core MVC web application for managing school users and courses (teachers, students, admins). The codebase follows a clean, layered architecture: Controllers → Services → Repositories (Unit of Work) → EF Core DbContext.

## Why this project is useful

- Role- and capability-based access control (`Role`, `Capability`, `User`, `Teacher`, `Student`)
- Cookie-based authentication with a 30-minute sliding expiration
- Course management with teacher/student relationships
- Separation of concerns: Controllers → Services → Repositories → DbContext
- Custom domain exceptions (`EntityNotFoundException`, `EntityAlreadyExistsException`, etc.) for consistent error handling
- AutoMapper for DTO ↔ entity mapping; dedicated signup DTOs per role (Admin/Teacher/Student)
- Structured logging via Serilog
- Localized error messages (English and Greek — `el-GR`)
- Password hashing via BCrypt

## Quick links

- Source: `TeacherOs/`
- Entrypoint: `TeacherOs/Program.cs`
- Controllers: `TeacherOs/Controllers/` (Home, User, Admin, Teacher, Student)
- Models: `TeacherOs/Models/` (User, Teacher, Student, Role, Capability, Course)
- Data context: `TeacherOs/Data/`
- Repositories: `TeacherOs/Repositories/` (with `UnitOfWork`)
- Services: `TeacherOs/Services/`
- DTOs: `TeacherOs/DTO/`
- Custom exceptions: `TeacherOs/Exceptions/`
- Localization resources: `TeacherOs/Resources/`

## Prerequisites

- .NET SDK 10.0 or later (install from https://dotnet.microsoft.com/)
- A database instance (e.g. SQL Server, SQLite, PostgreSQL) and a connection string
- Optional: Visual Studio 2022/2023, VS Code, or JetBrains Rider for development

## Get started (local development)

1. Clone the repository

```bash
git clone <repo-url>
cd <repo-directory>
```

2. Configure the application

- Open `TeacherOs/appsettings.json` (or `appsettings.Development.json`) and add a connection string named **`DevConnection`** — the app reads this specific key in `Program.cs`:

```json
{
  "ConnectionStrings": {
    "DevConnection": "Server=localhost;Database=TeacherOs;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. Restore, build and run

```bash
dotnet restore
dotnet build
cd TeacherOs
dotnet run
```

- By default the app runs on the URLs reported in the console. Open the browser and navigate to the displayed URL (e.g. `https://localhost:5001`).

4. Database migrations

- If the project uses EF Core migrations in your workflow, add and apply migrations as usual. Example commands (adjust project and startup project names if required):

```bash
# Add migration (example)
dotnet ef migrations add InitialCreate --project TeacherOs --startup-project TeacherOs
# Apply migrations
dotnet ef database update --project TeacherOs --startup-project TeacherOs
```
If you do not use EF migrations, ensure your target database schema is created before running the application.

## Build and Deploy

### Build for production

Publish the app as a self-contained set of release binaries:

```bash
cd TeacherOs
dotnet publish -c Release -o ./publish
```

This produces a `./publish` folder containing `TeacherOs.dll`, static assets (`wwwroot/`), and config files (`appsettings.json`). Copy this folder to your target server/host.

### Configure production settings

Before deploying, set production values via **environment variables** (recommended over committing them to `appsettings.json`):

```bash
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__DevConnection="Server=<prod-host>;Database=TeacherOs;User Id=<user>;Password=<password>;TrustServerCertificate=True;"
```

Or create an `appsettings.Production.json` next to the published DLL with the same `ConnectionStrings` structure shown in the setup step above.

### Run on a server (Kestrel)

```bash
cd publish
dotnet TeacherOs.dll
```

Kestrel listens on the URLs from `ASPNETCORE_URLS` (e.g. `export ASPNETCORE_URLS=http://0.0.0.0:5000`). For production, put it behind a reverse proxy (Nginx or IIS) for TLS termination, and run it as a service (e.g. `systemd` on Linux or an IIS site with the ASP.NET Core Module on Windows) so it restarts automatically.

### After deploying

Apply EF Core migrations against the production database before first run (see the migrations commands above), and confirm the app starts correctly by checking the logs Serilog writes on startup.

## Configuration overview

- appsettings.json / appsettings.Development.json — application configuration (logging, connection strings)
- TeacherOs/Program.cs — application startup and DI registration
- TeacherOs/Repositories/ — repository implementations and UnitOfWork
- TeacherOs/Services/ — business logic layer

## Contributing

Contributions are welcome. Suggested workflow:

1. Open an issue describing the feature or bug.
2. Fork the repository and create a feature branch.
3. Submit a pull request with a clear description and small, focused changes.

Please keep changes scoped and include tests where applicable.

## Maintainers

Maintained by the repository owner. For maintainers, review pull requests and issues in the GitHub repository.

## License

No `LICENSE` file is currently included in this repository. If you intend to open-source this project, add a license file (e.g. MIT) so others know how they can use the code.

## Acknowledgements

Bootstrap and jQuery assets are included under their respective licenses in `TeacherOs/wwwroot/lib/`.
