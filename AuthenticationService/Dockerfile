FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./AuthenticationService.Api/Authentication.Api.csproj","Authentication.Api/"]
COPY ["./AuthenticationService.Security/Authentication.Security.csproj","Authentication.Security/"]
COPY ["./AuthenticationService.Application/Authentication.Application.csproj","Authentication.Application/"]
COPY ["./AuthenticationService.Domain/Authentication.Domain.csproj","Authentication.Domain/"]
RUN dotnet restore "AuthenticationService.Api/Authentication.Api.csproj"
COPY . .

WORKDIR "/src/AuthenticationService.Api"
RUN dotnet build "AuthenticationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish 
ARG BUILD_CONFIGURATION=Release
RUN dtonet publish "AuthenticationService.Api.csproj" -c $$BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","AuthenticationService.Api.dll"]
