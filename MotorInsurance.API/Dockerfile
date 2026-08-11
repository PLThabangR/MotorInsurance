# ==========================================
# STAGE 1: BUILD
# ==========================================
# Use the SDK image to compile the code
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY MotorInsurance.API/MotorInsurance.API.csproj MotorInsurance.API/
COPY MotorInsurance.Domain/MotorInsurance.Domain.csproj MotorInsurance.Domain/

# Restore dependencies (cached if csproj doesn't change)
RUN dotnet restore MotorInsurance.API/MotorInsurance.API.csproj

# Copy all source code
COPY . .

# Build and publish the application
WORKDIR /src/MotorInsurance.API
RUN dotnet publish -c Release -o /app/publish

# ==========================================
# STAGE 2: RUNTIME
# ==========================================
# Use the smaller runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Expose port 80 (HTTP)
EXPOSE 80

# Copy the published files from build stage
COPY --from=build /app/publish .

# Create directory for logs
RUN mkdir -p /app/logs

# Set environment variable
ENV ASPNETCORE_ENVIRONMENT=Production

# Start the application
ENTRYPOINT ["dotnet", "MotorInsurance.API.dll"]