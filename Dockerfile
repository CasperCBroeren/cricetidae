FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Cricetidae.Api/Cricetidae.Api.csproj", "Cricetidae.Api/"]
COPY ["Cricetidae/Cricetidae.csproj", "Cricetidae/"]
RUN dotnet restore "Cricetidae.Api/Cricetidae.Api.csproj"
COPY . .
WORKDIR "/src/Cricetidae.Api"
RUN dotnet build "Cricetidae.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cricetidae.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cricetidae.Api.dll"]