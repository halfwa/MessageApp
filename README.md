# MessageApp

## Ветка main - классическая реализация DAL через ADO.NET, Async code, .NET 8, MVC + Web API, Postgres, docker-compose 
## Ветка orleans - реализация приложения с MS Orleans 

### Get started

```powershell
docker-compose up
```

### Проект

3 слоя - BLL, DAL, WEB + API

#### API - Ядро
Простое и емкое REST API с глобальным отловом ошибок из DAL и BLL, логированием и сокетом для потоковой передачи сообщений.

#### BLL - Бизнес логика
Модели, Севрисы и абстракция к ним.

#### DAL - Доступ к данынм
Реализация доступа к Postgres через ADO.NET.

#### WEB - Представление
MVC приложение для преставления данных.
