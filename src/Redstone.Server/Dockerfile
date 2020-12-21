#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Redstone.Server/Redstone.Server.csproj", "src/Redstone.Server/"]
COPY ["src/common/Redstone.NBT/Redstone.NBT.csproj", "src/common/Redstone.NBT/"]
COPY ["src/Redstone.Protocol/Redstone.Protocol.csproj", "src/Redstone.Protocol/"]
COPY ["src/Redstone.Common/Redstone.Common.csproj", "src/Redstone.Common/"]
COPY ["src/Redstone.Abstractions/Redstone.Abstractions.csproj", "src/Redstone.Abstractions/"]
COPY ["src/common/Redstone.Configuration.Yaml/Redstone.Configuration.Yaml.csproj", "src/common/Redstone.Configuration.Yaml/"]
RUN dotnet restore "src/Redstone.Server/Redstone.Server.csproj"
COPY . .
WORKDIR "/src/src/Redstone.Server"
RUN dotnet build "Redstone.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Redstone.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Redstone.Server.dll"]