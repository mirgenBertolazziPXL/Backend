# Set working directory to where the .csproj file actually is
WORKDIR /src

# Copy only the .csproj file first to help with Docker layer caching
COPY SolutionBackend/*.csproj ./SolutionBackend/

# Restore dependencies
RUN dotnet restore SolutionBackend/*.csproj

# Copy the rest of the source code
COPY . .

# Set the working directory to the project folder
WORKDIR /src/SolutionBackend

# Build and publish
RUN dotnet publish -c Release -o /app/publish
