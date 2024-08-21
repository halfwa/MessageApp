
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["../MessageApp.API/MessageApp.API.csproj", "MessageApp.API/"]
COPY ["../MessageApp.BLL/MessageApp.BLL.csproj", "MessageApp.BLL/"]
COPY ["../MessageApp.DAL/MessageApp.DAL.csproj", "MessageApp.DAL/"]
RUN dotnet restore "./MessageApp.API/MessageApp.API.csproj"
COPY . .
WORKDIR "/src/MessageApp.API"
RUN dotnet build "./MessageApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release 
RUN dotnet publish "./MessageApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageApp.API.dll"]