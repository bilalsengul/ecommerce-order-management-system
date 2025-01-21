# E-Commerce Order Management System

A scalable and robust order management system for e-commerce platforms, built with .NET 8.0 and following clean architecture principles.

## Features

- Order Creation and Management
- Product Catalog Management
- User Management and Authentication
- Real-time Order Status Updates
- Webhook Notifications
- Caching with Redis
- Message Queue Integration with RabbitMQ
- JWT Authentication
- API Documentation with Swagger
- Monitoring with Prometheus and Grafana
- Containerization with Docker

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
- Prometheus and Grafana for monitoring

## Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose
- Node.js and npm (for frontend)

## Getting Started

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd ecommerce-order-management-system
   ```

2. Run with Docker Compose:
   ```bash
   docker compose up -d
   ```

3. Access the services:
   - Frontend: http://localhost:3000
   - API: http://localhost:80
   - Swagger Documentation: http://localhost:80/swagger
   - Prometheus: http://localhost:9090
   - Grafana: http://localhost:3000

## Project Structure

```
├── ECommerceOrderManagement.API           # API Layer
│   ├── Controllers                        # API Controllers
│   ├── Program.cs                         # Application Entry Point
│   └── appsettings.json                   # Configuration
├── ECommerceOrderManagement.Core          # Domain Layer
│   ├── DTOs                              # Data Transfer Objects
│   ├── Entities                          # Domain Entities
│   ├── Interfaces                        # Interfaces/Contracts
│   └── Validators                        # Request Validators
├── ECommerceOrderManagement.Infrastructure # Infrastructure Layer
│   ├── Data                              # Database Context and Migrations
│   ├── Repositories                      # Repository Implementations
│   └── Services                          # Service Implementations
├── frontend                              # React Frontend Application
└── docker-compose.yml                    # Docker Compose Configuration
```

## API Endpoints

### Authentication
- `POST /api/auth/login` - Login user
- `POST /api/auth/register` - Register new user

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Orders
- `POST /api/orders` - Create a new order
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `PUT /api/orders/{id}/status` - Update order status
- `DELETE /api/orders/{id}` - Cancel order

## Authentication

The API uses JWT Bearer authentication. Include the JWT token in the Authorization header:

```
Authorization: Bearer <your-token>
```

## Database Migrations

The application automatically applies database migrations on startup. The migrations include:
- Product table creation
- User table creation
- Order and OrderItem tables creation
- Initial seed data for products and users

## Environment Variables

The following environment variables can be configured in `docker-compose.yml`:

```yaml
- ConnectionStrings__DefaultConnection: PostgreSQL connection string
- ConnectionStrings__Redis: Redis connection string
- RabbitMQ__Host: RabbitMQ host
- Jwt__Key: JWT signing key
- Jwt__Issuer: JWT issuer
- Jwt__Audience: JWT audience
```

## Monitoring and Logging

- Prometheus metrics available at `/metrics`
- Grafana dashboards for visualization
- Structured logging with Serilog
- Logs available in the `logs` directory

## Development

To run the application in development mode:

1. Start the infrastructure services:
   ```bash
   docker compose up -d postgres redis rabbitmq
   ```

2. Run the API:
   ```bash
   cd ECommerceOrderManagement.API
   dotnet run
   ```

3. Run the frontend:
   ```bash
   cd frontend
   npm install
   npm start
   ```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## Support

For support, please open an issue in the GitHub repository.
