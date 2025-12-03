FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder
EXPOSE 80
EXPOSE 443

WORKDIR /Application

# Обновляем CA сертификаты
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates

# Копируем только файлы проектов для восстановления пакетов
COPY ["webapi.csproj", "./"]

# Восстанавливаем пакеты из nuget-packages (если есть) и официальных источников
RUN dotnet restore "webapi.csproj"

# Копируем остальные файлы проекта
COPY . ./

# Публикуем приложение
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /Application

# Создаем директорию для сертификатов
RUN mkdir -p /app/certs && chmod 755 /app/certs

COPY --from=builder /Application/output .
ENTRYPOINT ["dotnet", "webapi.dll"]
