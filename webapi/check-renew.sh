#!/bin/bash

DOMAIN="vehsogi.ru"

echo "Проверка сертификатов для $DOMAIN..."

# Проверяем срок действия
docker-compose -f docker-compose.prod.yml run --rm certbot certificates

echo ""
echo "Принудительное обновление:"
docker-compose -f docker-compose.prod.yml run --rm certbot renew --force-renewal

echo ""
echo "Перезагрузка nginx для применения новых сертификатов:"
docker-compose -f docker-compose.prod.yml exec nginx nginx -s reload