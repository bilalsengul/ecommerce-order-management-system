# E-Commerce Order Management System

A scalable and robust order management system for e-commerce platforms, built with .NET 8.0 and following clean architecture principles.

## Features

- Order Creation and Management
- Real-time Order Status Updates
- Webhook Notifications
- Caching with Redis
- Message Queue Integration with RabbitMQ
- JWT Authentication
- API Documentation with Swagger
- Monitoring with Prometheus and Grafana
- Containerization with Docker
- Kubernetes Deployment Support

## Technology Stack

- ASP.NET Core 8.0
- PostgreSQL for data persistence
- Entity Framework Core for ORM
- Redis for caching
- RabbitMQ for message queuing
- Serilog for structured logging
- FluentValidation for request validation
- AutoMapper for object mapping
- Swagger/OpenAPI for API documentation
- Docker for containerization
- Kubernetes for orchestration
- Prometheus and Grafana for monitoring

## Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose
- PostgreSQL (if running locally)
- Redis (if running locally)
- RabbitMQ (if running locally)

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ecommerce-order-management-system.git
   cd ecommerce-order-management-system
   ```

2. Run with Docker Compose:
   ```bash
   docker compose up -d
   ```

3. Access the API:
   - API: http://localhost:8080
   - Swagger Documentation: http://localhost:8080/swagger
   - Prometheus Metrics: http://localhost:9090
   - Grafana Dashboard: http://localhost:3000

## Project Structure

```
├── ECommerceOrderManagement.API           # API Layer
├── ECommerceOrderManagement.Core          # Domain Layer
├── ECommerceOrderManagement.Infrastructure # Infrastructure Layer
├── tests
│   ├── ECommerceOrderManagement.API.Tests
│   ├── ECommerceOrderManagement.Core.Tests
│   └── ECommerceOrderManagement.Infrastructure.Tests
├── docs                                   # Documentation
├── k8s                                    # Kubernetes Manifests
└── docker-compose.yml                     # Docker Compose Configuration
```

## API Endpoints

### Orders

- `POST /api/orders` - Create a new order
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/by-date-range` - Get orders by date range
- `GET /api/orders/by-amount-range` - Get orders by amount range
- `PUT /api/orders/{id}/cancel` - Cancel an order

## Authentication

The API uses JWT Bearer authentication. Include the JWT token in the Authorization header:

```
Authorization: Bearer <your-token>
```

## CI/CD Pipeline

The project uses GitHub Actions for CI/CD:

1. Build and Test
   - Restore dependencies
   - Build solution
   - Run unit tests
   - Run integration tests
   - Generate code coverage report

2. Docker Build and Push
   - Build Docker image
   - Push to Docker Hub

3. Deployment
   - Deploy to Kubernetes cluster (if configured)

## Monitoring and Logging

- Prometheus metrics available at `/metrics`
- Grafana dashboards for visualization
- Structured logging with Serilog
- Log aggregation in JSON format

## Testing

Run the tests:

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

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Documentation

- [API Documentation](docs/api.md)
- [Database Schema](docs/erd.puml)
- [Sequence Diagrams](docs/order-creation-sequence.puml)
- [Deployment Guide](docs/deployment.md)

## Support

For support, please open an issue in the GitHub repository.
