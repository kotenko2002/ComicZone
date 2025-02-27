# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Restore NuGet packages
COPY *.csproj ./
RUN dotnet restore

# Copy all project files and publish the application
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the environment variable to define the HTTP port and expose it
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000

# Set the working directory and copy the published application from the build stage
WORKDIR /app
COPY --from=build-env /app/out .

# Define the entry point for the application
ENTRYPOINT [ "dotnet", "ComicZone.UserService.dll" ]