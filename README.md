# Minesweeper API

## Описание
Minesweeper API — это серверная часть для работы с сайтом [Minesweeper Test](https://minesweeper-test.studiotg.ru/). API управляет логикой игры, обработкой ходов и взаимодействием с базой данных.

## Требования
Перед началом работы убедитесь, что у вас установлены:
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (если требуется локальный запуск без Docker)

## Установка и запуск

### 1. Запуск через Docker

1. Клонируйте репозиторий и перейдите в корневую папку:
   ```sh
   git clone <репозиторий>
   cd MinesweeperApi
   ```
2. Запустите контейнеры с помощью Docker Compose:
   ```sh
   docker-compose up --build
   ```
3. API будет доступен по адресу:
   ```
   http://localhost:10000/api
   ```
4. Вставьте этот URL в [Minesweeper Test](https://minesweeper-test.studiotg.ru/) для работы.

### 2. Локальный запуск без Docker

1. Настройте переменные окружения из `env.api`.
2. Запустите Redis (если требуется):
   ```sh
   docker run --name minesweeper_redis -d -p 6379:6379 redis:latest
   ```
3. Перейдите в директорию API:
   ```sh
   cd Systems/API/MinesweeperApi.API
   ```
4. Выполните команду для запуска API:
   ```sh
   dotnet run
   ```

## Структура проекта
- `Application/` — бизнес-логика API (модели, сервисы).
- `Infrastructure/` — работа с данными (контексты, базы данных).
- `Shared/` — общие модули (исключения, настройки).
- `Systems/API/` — основной API-контроллер.
- `Tests/` — тесты.

## API Методы

### 1. Создание новой игры
**POST** `/api/game/new`
#### Пример запроса:
```json
{
  "width": 10,
  "height": 10,
  "mines": 20
}
```
#### Пример ответа:
```json
{
  "gameId": "12345",
  "board": [["?", "?", "?"], ["?", "?", "?"], ["?", "?", "?"]]
}
```

### 2. Ход игрока
**POST** `/api/game/move`
#### Пример запроса:
```json
{
  "gameId": "12345",
  "x": 2,
  "y": 3
}
```
#### Пример ответа:
```json
{
  "board": [["1", "1", "0"], ["1", "M", "0"], ["1", "1", "0"]]
}
```

## Тестирование
Для запуска тестов выполните:
```sh
cd Tests
 dotnet test
```

## CI/CD
Проект использует GitHub Actions для автоматического тестирования при `pull request` и `merge` в `develop` и `main`.

## Контакты
Для вопросов и предложений свяжитесь с разработчиком.

