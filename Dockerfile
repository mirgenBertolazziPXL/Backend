FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copy the solution file
COPY SolutionBackendTeam13.sln ./

# 2. Copy all project files from SolutionBackendTeam13 and ClassLib13 folders
COPY SolutionBackendTeam13/ ./SolutionBackendTeam13/
COPY ClassLib13/ ./ClassLib13/

# 3. Restore dependencies
RUN dotnet restore SolutionBackendTeam13.sln

# 4. Build and publish
RUN dotnet publish SolutionBackendTeam13/WebAppTeam13.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebAppTeam13.dll"]
