# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WlConsultings.BankChallenge.WebApi/WlConsultings.BankChallenge.WebApi.csproj", "src/WlConsultings.BankChallenge.WebApi/"]
COPY ["src/WlConsultings.BankChallenge.Application/WlConsultings.BankChallenge.Application.csproj", "src/WlConsultings.BankChallenge.Application/"]
COPY ["src/WlConsultings.BankChallenge.Domain/WlConsultings.BankChallenge.Domain.csproj", "src/WlConsultings.BankChallenge.Domain/"]
COPY ["src/WlConsultings.BankChallenge.IoC/WlConsultings.BankChallenge.IoC.csproj", "src/WlConsultings.BankChallenge.IoC/"]
COPY ["src/WlConsultings.BankChallenge.Infra/WlConsultings.BankChallenge.Infra.csproj", "src/WlConsultings.BankChallenge.Infra/"]
RUN dotnet restore "./src/WlConsultings.BankChallenge.WebApi/WlConsultings.BankChallenge.WebApi.csproj"
COPY . . 
WORKDIR "/src/src/WlConsultings.BankChallenge.WebApi"
RUN dotnet tool install --global dotnet-ef 
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet build "./WlConsultings.BankChallenge.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

## Run EF Database Update Command
#RUN dotnet ef database update --startup-project /src/src/WlConsultings.BankChallenge.WebApi --project /src/src/WlConsultings.BankChallenge.Infra 
#
# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WlConsultings.BankChallenge.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WlConsultings.BankChallenge.WebApi.dll"]

