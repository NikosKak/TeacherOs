# TeacherOs

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]() [![License](https://img.shields.io/badge/license-MIT-blue)]()

A simple ASP.NET Core web application for managing school users and courses (teachers, students, admin). The codebase follows a clean architecture with controllers, services, repositories and EF Core-style DbContext.

## Why this project is useful

- Role-based user management (Admin, Teacher, Student, User)
- Separation of concerns: Controllers → Services → Repositories
- Simple, extensible starter for school management features (courses, users)
- Includes server-side validation, error handling and resource localization

## Quick links

- Source: `TeacherOs/`
- Entrypoint: `TeacherOs/Program.cs`
- Controllers: `TeacherOs/Controllers/`
- Data context: `TeacherOs/Data/SchoolOsContext.cs`
- Repositories: `TeacherOs/Repositories/`
- Services: `TeacherOs/Services/`

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

- Open `TeacherOs/appsettings.json` (or `appsettings.Development.json`) and set your connection string under `ConnectionStrings`.

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

## Configuration overview

- appsettings.json / appsettings.Development.json — application configuration (logging, connection strings)
- TeacherOs/Program.cs — application startup and DI registration
- TeacherOs/Repositories/ — repository implementations and UnitOfWork
- TeacherOs/Services/ — business logic layer

## Testing

There are no unit tests included in the repository. To add tests, create a new test project (xUnit / NUnit / MSTest), reference `TeacherOs` and follow standard .NET testing patterns.

## Contributing

Contributions are welcome. Suggested workflow:

1. Open an issue describing the feature or bug.
2. Fork the repository and create a feature branch.
3. Submit a pull request with a clear description and small, focused changes.

Please keep changes scoped and include tests where applicable.

## Where to get help

- Open an issue in this repository for bugs and feature requests.
- Use the repository's code and comments as the primary documentation.

## Maintainers

Maintained by the repository owner. For maintainers, review pull requests and issues in the GitHub repository.

## License

See the `LICENSE` file in this repository for license details.

## Acknowledgements

Bootstrap and jQuery assets are included under their respective licenses in `TeacherOs/wwwroot/lib/`.
