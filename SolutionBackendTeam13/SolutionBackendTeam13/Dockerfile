# Use the .NET SDK image (consider using 8.0 if your project supports it)
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set working directory
WORKDIR /src

# Copy project file first (note the corrected .caproj extension)
COPY ["WebAppTeam13.csproj", "./"]

# Restore dependencies
RUN dotnet restore "WebAppTeam13.csproj"

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish "WebAppTeam13.caproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (adjust if your app uses a different port)
EXPOSE 80
EXPOSE 443

# Start the application
ENTRYPOINT ["dotnet", "WebAppTeam13.dll"]