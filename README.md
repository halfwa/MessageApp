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
Модели, Сервисы и абстракция к ним.

#### DAL - Доступ к данынм
Реализация доступа к Postgres через ADO.NET.

#### WEB - Представление
MVC приложение для представления данных.

![Снимок экрана 2024-08-21 182811](https://github.com/halfwa/MessageApp/blob/main/assets/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202024-08-21%20182811.png)


![Снимок экрана 2024-08-21 184800](https://github.com/halfwa/MessageApp/blob/main/assets/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202024-08-21%20184800.png)


![Снимок экрана 2024-08-22 011426](https://github.com/halfwa/MessageApp/blob/main/assets/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202024-08-22%20011426.png)

