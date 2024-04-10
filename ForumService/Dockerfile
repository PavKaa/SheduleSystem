FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./ForumService/ForumService.csproj ./ForumService/
COPY ./DAL/DAL.csproj ./DAL/
COPY ./Domain/Domain.csproj ./Domain/
COPY ./Service/Service.csproj ./Service/
RUN dotnet restore ./ForumService/ForumService.csproj

COPY . .
WORKDIR /src/ForumService
RUN dotnet build "ForumService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForumService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForumService.dll"]