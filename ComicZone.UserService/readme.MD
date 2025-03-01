## User Service

### 🚀 Getting Started

#### Apply Migrations
Run the following commands to apply database migrations:

```sh
dotnet ef migrations add <MigrationName> -o DAL/Migrations -s ./ComicZone.UserService
dotnet ef database update -s ./ComicZone.UserService
```

#### Build & Run
To build the service, use:

```sh
docker build -t userservice .
```

### 📌 Description
User Service handles user authentication, registration, and profile management.

### 🛠️ Technologies Used
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
