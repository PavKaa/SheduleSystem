#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./PersonalAccountService/PersonalAccountService.csproj, ./PersonalAccountService/
COPY ./BusinessLogic/BusinessLogic.csproj, ./BusinessLogic/
COPY ./DAL/DAL.csproj, ./DAL/
RUN dotnet restore ./PersonalAccountService/PersonalAccountService.csproj

COPY . .
WORKDIR /src/PersonalAccountService
RUN dotnet build "PersonalAccountService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonalAccountService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonalAccountService.dll"]