# Car Rental Service

Сервис для автоматизации пункта проката автомобилей

## Стек технологий

* Platform: .NET 8 (C# 12)
* Database: SQL Server
* ORM: Entity Framework Core 8
* Messaging: RabbitMQ (RabbitMQ.Client)
* Mapping: AutoMapper
* Testing: xUnit, Bogus (Fake Data)
* Orchestration: .NET Aspire

## Структура проекта

### 1. CarRental.Domain

Доменная модель. Содержит доменные сущности:

* `CarModel` — модель автомобиля (справочник): тип привода, класс, тип кузова, количество мест
* `ModelGeneration` — поколение модели: год выпуска, объём двигателя, коробка передач, стоимость часа аренды
* `Car` — физический экземпляр автомобиля: госномер, цвет, поколение модели
* `Client` — клиент: номер водительского удостоверения, ФИО, дата рождения
* `Rental` — договор аренды: клиент, автомобиль, время выдачи, длительность в часах

### 2. CarRental.Application.Contracts

Контракты бизнес-логики приложения.

* `Contracts/Dto` — DTO (records) для CRUD-операций и аналитики
* `MappingProfile` — профили AutoMapper для маппинга сущностей в DTO и обратно

### 3. CarRental.Infrastructure

Реализация работы с внешними системами.

* `Persistence/AppDbContext` — конфигурация DbContext и таблиц
* `Migrations` — миграции базы данных EF Core
* `Repositories/DbRepository` — реализация универсального репозитория

### 4. CarRental.Infrastructure.Messaging

Реализация работы с брокером сообщений.

* `RentalQueueConsumer` — фоновый сервис (`BackgroundService`), который подписывается на очередь RabbitMQ, валидирует входящие пакеты договоров аренды и сохраняет их в базу данных

### 5. CarRental.API

Точка входа Web API.

* `Controllers` — REST API контроллеры для всех сущностей (`CarModelsController`, `CarsController`, `ClientsController`, `ModelGenerationsController`, `RentalsController`, `AnalyticsController`)
* `Program.cs` — конфигурация приложения, регистрация зависимостей

### 6. CarRental.AppHost

Оркестратор .NET Aspire.

* Управляет запуском SQL Server, RabbitMQ, CarRental.API и CarRental.Generator.Host
* Автоматически настраивает строки подключения и переменные окружения

### 7. CarRental.ServiceDefaults

Общие настройки сервисов (health checks, OpenTelemetry, service discovery).

### 8. CarRental.Generator.Host

Сервис генерации нагрузки.

* `Generator/RentalGenerator` — генерирует фейковые DTO договоров аренды через Bogus
* `Messaging/RentalPublisher` — публикует пакеты в обменник RabbitMQ
* `Controllers/GeneratorController` — REST-контроллер для запуска генерации: `POST /api/Generator/rentals?listSize=100&batchSize=10&delayMs=500`
* Flow: генерирует пачки аренд -> публикует в RabbitMQ -> CarRental.API считывает и сохраняет в БД
