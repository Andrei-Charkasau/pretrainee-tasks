# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["Inno_Shop_Cherkasov/InnoShop.csproj", "Inno_Shop_Cherkasov/"]
COPY ["InnoShop.Core.Services/InnoShop.Core.Services.csproj", "InnoShop.Core.Services/"]
COPY ["InnoShop.Auth.Services/InnoShop.Auth.Services.csproj", "InnoShop.Auth.Services/"]
COPY ["InnoShop.Core/InnoShop.Core.csproj", "InnoShop.Core/"]
COPY ["InnoShop.Core.Repositories/InnoShop.Core.Repositories.csproj", "InnoShop.Core.Repositories/"]
RUN dotnet restore "./Inno_Shop_Cherkasov/InnoShop.csproj"
COPY . .
WORKDIR "/src/Inno_Shop_Cherkasov"
RUN dotnet build "./InnoShop.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./InnoShop.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnoShop.dll"]