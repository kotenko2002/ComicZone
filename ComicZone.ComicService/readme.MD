## Comic Service

### 🚀 Getting Started

#### Apply Migrations
Run the following commands to apply database migrations:

```sh
dotnet ef migrations add <MigrationName> -o DAL/Migrations -s ./ComicZone.ComicService
dotnet ef database update -s ./ComicZone.ComicService
```

#### Build & Run
To build the service, use:

```sh
docker build -t comicservice .
```

### 📌 Description
Comic Service manages comic-related operations, such as storing metadata, chapters, and categories.

### 🛠️ Technologies Used
- ASP.NET Core
- Entity Framework Core
- PostgreSQL