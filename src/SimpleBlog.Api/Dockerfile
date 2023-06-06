#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/SimpleBlog.Api/SimpleBlog.Api.csproj", "src/SimpleBlog.Api/"]
COPY ["src/SimpleBlog.Application/SimpleBlog.Application.csproj", "src/SimpleBlog.Application/"]
COPY ["src/SimpleBlog.Domain/SimpleBlog.Domain.csproj", "src/SimpleBlog.Domain/"]
COPY ["src/SimpleBlog.Infrastructure/SimpleBlog.Infrastructure.csproj", "src/SimpleBlog.Infrastructure/"]
RUN dotnet restore "src/SimpleBlog.Api/SimpleBlog.Api.csproj"
COPY . .
WORKDIR "/src/src/SimpleBlog.Api"
RUN dotnet build "SimpleBlog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleBlog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleBlog.Api.dll"]