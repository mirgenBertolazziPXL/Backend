# Use the .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set working directory inside container
WORKDIR /src

# Copy only the .csproj file(s) first
COPY SolutionBackendTeam13/WebAppTeam13.csproj ./SolutionBackendTeam13/

# Restore dependencies
RUN dotnet restore SolutionBackendTeam13/WebAppTeam13.csproj

# Copy the rest of the source code
COPY . .

# Set working directory to the project folder
WORKDIR /src/SolutionBackendTeam13

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "WebAppTeam13.dll"]

