# FoodLink

A .NET 10 web API for connecting food donors with charities and organizations in need.

## Features

- **Donation Management**: Create and manage food donations with expiration tracking
- **User Authentication**: JWT-based authentication system
- **Business Profiles**: Support for restaurant/business donor accounts
- **Charity Profiles**: Support for charity/organization recipient accounts
- **Image Upload**: Cloudinary integration for donation photos
- **Reservation System**: Allow charities to reserve donation items
- **Review System**: Rate and review completed donations

## Tech Stack

- **Backend**: ASP.NET Core Web API (.NET 10)
- **Database**: SQLite (development) / SQL Server (production)
- **Authentication**: JWT Bearer tokens
- **Image Storage**: Cloudinary
- **Architecture**: Clean Architecture (Domain-Driven Design)
- **Testing**: xUnit

## Project Structure

```
FoodLink/
├── FoodLink.Api/          # Web API controllers and startup
├── FoodLink.Application/  # Application services and DTOs
├── FoodLink.Domain/       # Domain entities and business logic
├── FoodLink.Infrastructure/ # Data access and external services
└── FoodLink.Tests/        # Unit tests
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQLite (for development)

### Setup

1. Clone the repository
2. Navigate to the API project: `cd FoodLink.Api`
3. Update `appsettings.Development.json` with your configuration
4. Run the application: `dotnet run`

### Database

The application uses Entity Framework Core with SQLite for development:

```bash
# From FoodLink.Api directory
dotnet ef database update
```

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration

### Donations
- `GET /api/donations` - Get active donations
- `POST /api/donations` - Create new donation
- `GET /api/donations/{id}` - Get donation details

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License

This project is licensed under the MIT License.