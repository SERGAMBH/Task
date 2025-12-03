#!/bin/bash

set -e  # Останавливать при ошибках

# Если сертификатов нет - получить
if [ ! -d "./data/certbot/conf/live/vehsogi.ru" ]; then
    echo "Сертификатов нет, запускаем получение..."
    ./init-certbot.sh
else
    echo "Сертификаты найдены, запускаем сервисы..."
    docker-compose -f docker-compose.prod.yml up -d
fi

# Показываем логи
echo ""
echo "Логи nginx:"
docker-compose -f docker-compose.prod.yml logs --tail=50 nginx

echo ""
echo "Логи certbot:"
docker-compose -f docker-compose.prod.yml logs --tail=20 certbot