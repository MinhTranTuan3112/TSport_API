FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /tsport_app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TSport.Api/TSport.Api.csproj", "TSport.Api/"] 
COPY ["TSport.Api.Services/TSport.Api.Services.csproj", "TSport.Api.Services/"] 
COPY ["TSport.Api.Repositories/TSport.Api.Repositories.csproj", "TSport.Api.Repositories/"] 
COPY ["TSport.Api.Models/TSport.Api.Models.csproj", "TSport.Api.Models/"] 
COPY ["TSport.Api.Shared/TSport.Api.Shared.csproj", "TSport.Api.Shared/"] 

RUN dotnet restore "TSport.Api/TSport.Api.csproj"
COPY . .

WORKDIR "/src/TSport.Api"
RUN dotnet build "TSport.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TSport.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /tsport_app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "TSport.Api.dll", "--environment=Development"]