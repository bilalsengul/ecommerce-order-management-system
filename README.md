# E-Commerce Order Management System

A scalable and modern order management system built with ASP.NET Core, following clean architecture principles and implementing various enterprise patterns and practices.

## Features

- Order creation and management
- Real-time order status updates via webhooks
- Caching with Redis
- Message queuing with RabbitMQ
- Monitoring with Prometheus and Grafana
- Comprehensive logging with Serilog
- API documentation with Swagger/OpenAPI
- JWT-based authentication
- Docker containerization

## Technology Stack

- ASP.NET Core 8.0
- PostgreSQL
- Entity Framework Core
- Redis
- RabbitMQ
- AutoMapper
- FluentValidation
- Serilog
- Prometheus
- Grafana
- Docker & Docker Compose

## Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose
- Git

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ecommerce-order-management.git
   cd ecommerce-order-management
   ```

2. Run the application using Docker Compose:
   ```bash
   docker-compose up -d
   ```

3. Access the services:
   - API: http://localhost:5000
   - Swagger Documentation: http://localhost:5000/swagger
   - RabbitMQ Management: http://localhost:15672
   - Prometheus: http://localhost:9090
   - Grafana: http://localhost:3000

## Project Structure

```
├── ECommerceOrderManagement.API           # API Layer
├── ECommerceOrderManagement.Core          # Domain Layer
├── ECommerceOrderManagement.Infrastructure # Infrastructure Layer
├── docker-compose.yml                     # Docker Compose configuration
├── Dockerfile                             # Docker configuration
└── README.md                              # Project documentation
```

## API Endpoints

### Orders

- `POST /api/orders` - Create a new order
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/user/{userId}` - Get orders by user
- `GET /api/orders/filter/date` - Filter orders by date range
- `GET /api/orders/filter/amount` - Filter orders by amount range
- `POST /api/orders/{id}/cancel` - Cancel an order

## Configuration

The application can be configured through the following configuration files:
- `appsettings.json`
- `appsettings.Development.json`
- `docker-compose.yml`
- `prometheus.yml`

## Monitoring and Logging

- Logs are stored in the `logs` directory and can be viewed in Grafana
- Metrics are collected by Prometheus and visualized in Grafana
- Default Grafana credentials:
  - Username: admin
  - Password: admin

## Development

To run the application locally:

1. Update connection strings in `appsettings.Development.json`
2. Run required services using Docker Compose:
   ```bash
   docker-compose up -d postgres redis rabbitmq
   ```
3. Run the API:
   ```bash
   cd ECommerceOrderManagement.API
   dotnet run
   ```

## Testing

The project includes:
- Unit tests
- Integration tests
- API tests

To run tests:
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
