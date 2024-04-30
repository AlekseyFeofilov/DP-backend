# DP-backend

### Deploy
Берём адрес и креды для подключения к серверу у Лёши или из дискорда

Чтобы раскатить новую версию  :
```shell
cd /root/DP-backend
# подкачиваем актуальный код с гитхаба
git pull 
# собираем новую версию контейнера 
docker compose build app
# подымаем новые контейнеры
docker compose up 
``` 

Конфиги переписываем в `/root/DP-backend/.env` или `appsettings.{ENV=Development}.json`