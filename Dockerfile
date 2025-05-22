# Use the .NET SDK image (recommend using 7.0 or 8.0 based on your project)
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set working directory
WORKDIR /src

# Copy the solution file first (since your structure shows SolutionBackendTeam13.sln)
COPY ["SolutionBackendTeam13.sln", "./"]
COPY ["SolutionBackendTeam13/WebAppTeam13.csproj", "./SolutionBackendTeam13/"]

# Restore dependencies
RUN dotnet restore "SolutionBackendTeam13.sln"

# Copy everything else
COPY . .

# Build and publish
WORKDIR /src/SolutionBackendTeam13
RUN dotnet publish "WebAppTeam13.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Start the application
ENTRYPOINT ["dotnet", "WebAppTeam13.dll"]