#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["DeliVeggie.Microservice/DeliVeggie.Microservice.csproj", "DeliVeggie.Microservice/"]
COPY ["DeliVeggie.Microservice.Data/DeliVeggie.Microservice.Data.csproj", "DeliVeggie.Microservice.Data/"]
COPY ["DeliVeggie.Microservice.Data.Contracts/DeliVeggie.Microservice.Data.Contracts.csproj", "DeliVeggie.Microservice.Data.Contracts/"]
COPY ["DeliVeggie.Microservice.Models/DeliVeggie.Microservice.Models.csproj", "DeliVeggie.Microservice.Models/"]
COPY ["DeliVeggie.Microservice.Domain/DeliVeggie.Microservice.Domain.csproj", "DeliVeggie.Microservice.Domain/"]
COPY ["DeliVeggie.Microservice.Domain.Contracts/DeliVeggie.Microservice.Domain.Contracts.csproj", "DeliVeggie.Microservice.Domain.Contracts/"]
RUN dotnet restore "DeliVeggie.Microservice/DeliVeggie.Microservice.csproj"
COPY . .
WORKDIR "/src/DeliVeggie.Microservice"
RUN dotnet build "DeliVeggie.Microservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeliVeggie.Microservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliVeggie.Microservice.dll"]