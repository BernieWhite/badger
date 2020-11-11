# Sample Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/Badger/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY src/Badger/ ./
RUN dotnet publish -c Release -o out

# Stage 2

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Badger.dll"]

