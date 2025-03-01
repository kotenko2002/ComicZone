# ComicZone Microservices

## 🚀 Getting Started

To launch the entire system, use the following command:

```sh
cd ./docker-compose
docker-compose --project-name comiczone -f ./file-storage.yml -f ./comic-service.yml -f ./user-service.yml up -d
```

## 📌 Description
ComicZone is a microservices-based web platform for viewing comics. The system consists of multiple services running in Docker containers.

## 🏗️ Microservices
- **Comic Service** – Handles comic-related data and operations.
- **User Service** – Manages user accounts and authentication.

## 🛠️ Technologies Used
- **Backend:** ASP.NET Core Web API (microservices architecture)
- **Database:** PostgreSQL
- **Storage:** MinIO (local via Docker)
