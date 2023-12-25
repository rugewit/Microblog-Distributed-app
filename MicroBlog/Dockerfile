# Use the official .NET SDK 7 image as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj and restore dependencies
COPY MicroBlog/*.csproj ./
RUN dotnet restore

# Copy the remaining files to the container
COPY MicroBlog/. ./

# Build the application
RUN dotnet publish -c Release -o out

# Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose the port that the application will run on
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "MicroBlog.dll"]
