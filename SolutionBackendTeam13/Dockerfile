# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

# Restore solution
RUN dotnet restore SolutionBackendTeam13.sln

# Publish project — adjust path here to your actual .csproj inside nested folder
RUN dotnet publish SolutionBackendTeam13/WebAppTeam13.csproj -c Release -o /app/publish

# Stage 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Expose port (adjust if your app listens on a different port)
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "WebAppTeam13.dll"]
