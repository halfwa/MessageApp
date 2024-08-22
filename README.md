# MessageApp

## Ветка в разработке

## Ветка orleans - реализация приложения с MS Orleans 
## Ветка main - классическая реализация DAL через ADO.NET, Async code, .NET 8, MVC + Web API, Postgres, docker-compose 

### Get started

```powershell
docker-compose up
```

### Проект

#### API - Silo server
Простое и емкое REST API с глобальным отловом ошибок из DAL и BLL, логированием и сокетом для потоковой передачи сообщений.

#### BLL - Бизнес логика
Акторы(Grains), Модели, Сервисы и абстракция к ним.

#### DAL - Доступ к данынм
Реализация доступа к Postgres через Orleans ADO.NET Persistence.

#### WEB - Представление
MVC приложение для представления данных.

