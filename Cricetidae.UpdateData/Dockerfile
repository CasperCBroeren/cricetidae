FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Cricetidae.UpdateData.csproj"
WORKDIR "/src/Cricetidae.UpdateData"
RUN dotnet build "Cricetidae.UpdateData.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cricetidae.UpdateData.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cricetidae.UpdateData.dll"]