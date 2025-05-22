# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set working directory inside container
WORKDIR /src

# Copy only the .csproj file first to leverage Docker caching
COPY SolutionBackendTeam13/*.csproj ./SolutionBackendTeam13/

# Restore dependencies
RUN dotnet restore SolutionBackendTeam13/WebAppTeam13.csproj

# Copy the rest of the source code
COPY . .

# Set working directory to actual project folder
WORKDIR /src/SolutionBackend

# Build and publish the application
RUN dotnet publish -c Release -o /app/publish

# Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Run the app
ENTRYPOINT ["dotnet", "YourProjectName.dll"]
