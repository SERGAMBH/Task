#!/bin/bash

# Переменные
DOMAIN="vehsogi.ru"
EMAIL="ambh@list.ru"
COMPOSE_FILE="docker-compose.prod.yml"

echo "=== Получение SSL сертификата для $DOMAIN ==="

# 1. Останавливаем ВСЕ контейнеры, занимающие порт 80
echo "Останавливаем контейнеры на порту 80..."
docker-compose -f $COMPOSE_FILE down

# Проверяем, что порт 80 свободен
if lsof -ti:80 | grep -q .; then
    echo "ОШИБКА: Порт 80 занят другими процессами:"
    lsof -ti:80 | xargs ps -o pid,command -p
    echo "Освободите порт 80 и попробуйте снова"
    exit 1
fi

# 2. Создаем директории
echo "Создаем директории для certbot..."
mkdir -p data/certbot/conf data/certbot/www

# 3. Получаем первый сертификат через standalone
echo "Получаем SSL сертификат через standalone режим..."
docker run -it --rm \
  -p 80:80 \
  -p 443:443 \
  -v "$(pwd)/data/certbot/conf:/etc/letsencrypt" \
  -v "$(pwd)/data/certbot/www:/var/www/certbot" \
  certbot/certbot certonly --standalone \
  --email $EMAIL \
  --agree-tos \
  --no-eff-email \
  --non-interactive \
  --domain $DOMAIN \
  --domain www.$DOMAIN \
  --preferred-challenges http

# 4. Проверяем получение сертификата
if [ -d "./data/certbot/conf/live/$DOMAIN" ]; then
    echo "✅ Сертификат успешно получен!"
    echo "📁 Путь: ./data/certbot/conf/live/$DOMAIN/"
    ls -la "./data/certbot/conf/live/$DOMAIN/"
    
    # 5. Запускаем production окружение
    echo "Запускаем production окружение..."
    docker-compose -f $COMPOSE_FILE up -d
    
    # 6. Проверяем сервисы
    echo "Статус сервисов:"
    docker-compose -f $COMPOSE_FILE ps
    
    # 7. Информация об автообновлении
    echo ""
    echo "📋 Информация:"
    echo "- Сертификат будет автоматически обновляться каждые 12 часов"
    echo "- Для принудительного обновления: docker-compose -f $COMPOSE_FILE run --rm certbot renew"
    echo "- Проверить срок действия: docker-compose -f $COMPOSE_FILE run --rm certbot certificates"
    
else
    echo "❌ Ошибка: Сертификат не получен!"
    echo "Попробуйте с флагом --staging для тестирования:"
    echo "docker run -it --rm -p 80:80 -v $(pwd)/data/certbot/conf:/etc/letsencrypt certbot/certbot certonly --standalone --staging --email $EMAIL --agree-tos -d $DOMAIN"
    exit 1
fi