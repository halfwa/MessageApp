
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8880
EXPOSE 8881

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["../MessageApp.WEB/MessageApp.WEB.csproj", "MessageApp.WEB/"]
RUN dotnet restore "./MessageApp.WEB/MessageApp.WEB.csproj"
COPY . .
WORKDIR "/src/MessageApp.WEB"
RUN dotnet build "./MessageApp.WEB.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MessageApp.WEB.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageApp.WEB.dll"]