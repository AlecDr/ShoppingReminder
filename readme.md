# Shopping Reminder API

A multi-tenant SaaS application for managing shared shopping lists with offline-first mobile support.

## 🎯 Project Overview

Shopping Reminder helps families and groups collaborate on shopping lists. Users can create groups, invite members, manage multiple lists, and sync seamlessly between devices with offline-first capabilities.

### Key Features

- **Multi-tenant Architecture**: Support for multiple groups per user
- **Offline-First**: Mobile app works without internet, syncs when available
- **Real-time Collaboration**: Multiple users can manage shared shopping lists
- **Smart Features**: Purchase history, templates, urgent items, notifications
- **Audit Trail**: Full tracking of who created/modified/deleted items
- **Soft Deletes**: Data safety with reversible deletions

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:
```
┌─────────────────────────────────────────┐
│            API Layer                    │
│  (Controllers, Middleware, Auth)        │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│       Application Layer                 │
│  (Use Cases, CQRS, Validation)          │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│         Domain Layer                    │
│  (Entities, Business Logic)             │
└─────────────────────────────────────────┘
                 ▲
                 │        
┌────--------────┴────────────────────────┐
│      Infrastructure Layer               │
│  (Database, External Services)          │
└─────────────────────────────────────────┘
```

### Project Structure

- **ShoppingReminder.Domain**: Core business entities and logic
- **ShoppingReminder.Application**: Use cases, DTOs, interfaces
- **ShoppingReminder.Infrastructure**: Database, external services
- **ShoppingReminder.API**: REST API endpoints, authentication
- **ShoppingReminder.UnitTests**: Unit tests for business logic
- **ShoppingReminder.IntegrationTests**: API integration tests

## 🛠️ Technology Stack

### Backend
- **.NET 8.0**: Latest LTS version
- **ASP.NET Core Web API**: RESTful API
- **Entity Framework Core 8**: ORM
- **PostgreSQL**: Primary database
- **MediatR**: CQRS pattern implementation
- **FluentValidation**: Request validation
- **Mapster**: Object mapping
- **JWT**: Authentication
- **Hangfire**: Background jobs (notifications, digests)
- **Serilog**: Structured logging
- **Swagger/OpenAPI**: API documentation

### Mobile (Planned)
- **Flutter**: Cross-platform mobile app
- **SQLite**: Local offline storage
- **Drift**: Type-safe SQL wrapper

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Docker](https://www.docker.com/) (optional, for containerized PostgreSQL)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/shopping-reminder.git
cd shopping-reminder
```

### 2. Set Up PostgreSQL

**Option A: Local Installation**
```bash
# Create database
createdb ShoppingReminderDb
```

**Option B: Docker**
```bash
docker run --name postgres-shopping \
  -e POSTGRES_PASSWORD=yourpassword \
  -e POSTGRES_DB=ShoppingReminderDb \
  -p 5432:5432 \
  -d postgres:15
```

### 3. Configure Connection String

Update `appsettings.Development.json` in the API project:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ShoppingReminderDb;Username=postgres;Password=yourpassword"
  }
}
```

### 4. Run Migrations

Open **Package Manager Console** in Visual Studio:
- Set `ShoppingReminder.API` as **Startup Project**
- Select `ShoppingReminder.Infrastructure` in console dropdown
```powershell
Add-Migration InitialCreate
Update-Database
```

### 5. Run the API

Press `F5` in Visual Studio or:
```bash
cd src/ShoppingReminder.API
dotnet run
```

The API will be available at:
- `https://localhost:7001` (HTTPS)
- `http://localhost:5001` (HTTP)
- Swagger UI: `https://localhost:7001/swagger`

## 📚 API Documentation

Once the API is running, visit the Swagger UI at:
```
https://localhost:7001/swagger
```

### Main Endpoints

#### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token
- `POST /api/auth/refresh` - Refresh access token

#### Groups
- `GET /api/groups` - List user's groups
- `POST /api/groups` - Create new group
- `POST /api/groups/{id}/invite` - Invite user to group
- `PUT /api/groups/{id}/members/{userId}` - Update member role
- `DELETE /api/groups/{id}/members/{userId}` - Remove member

#### Shopping Lists
- `GET /api/lists` - Get all lists in group
- `POST /api/lists` - Create new list
- `GET /api/lists/{id}` - Get list with items
- `PUT /api/lists/{id}` - Update list
- `DELETE /api/lists/{id}` - Delete list (soft delete)

#### Shopping Items
- `POST /api/lists/{listId}/items` - Add item to list
- `PUT /api/items/{id}` - Update item
- `PATCH /api/items/{id}/purchase` - Mark as purchased
- `DELETE /api/items/{id}` - Delete item (soft delete)

#### Sync
- `POST /api/sync` - Batch sync for offline changes
- `GET /api/sync/changes?since={timestamp}` - Get changes since timestamp

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test tests/ShoppingReminder.UnitTests
```

### Run Integration Tests
```bash
dotnet test tests/ShoppingReminder.IntegrationTests
```

### Run All Tests with Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 🗄️ Database Schema

### Core Tables

- **Users**: User accounts and authentication
- **Groups**: Collaborative groups (families, roommates)
- **GroupMembers**: User membership in groups with roles
- **ShoppingLists**: Lists within groups
- **ShoppingItems**: Individual items in lists
- **Invitations**: Pending group invitations
- **PurchaseHistory**: Denormalized history for quick re-adds

### Key Features

- **Soft Deletes**: All entities support reversible deletion
- **Audit Trail**: Track who created/modified/deleted and when
- **Optimistic Concurrency**: Version numbers for conflict resolution
- **Indexes**: Optimized for common queries

## 🔐 Authentication & Security

- **JWT Tokens**: Bearer token authentication
- **Password Hashing**: BCrypt with salt
- **Email Verification**: Required for new accounts
- **Role-Based Access**: Group owners, admins, members, viewers
- **Rate Limiting**: Prevent API abuse
- **CORS**: Configured for mobile app origins

## 📱 Mobile App (Coming Soon)

The Flutter mobile app will feature:
- Offline-first architecture with SQLite
- Background sync when connectivity available
- Push notifications for urgent items
- QR code group invites
- Barcode scanning for quick item entry
- Location-based reminders

## 🔄 CI/CD Pipeline (Planned)

- **GitHub Actions**: Automated builds and tests
- **Docker**: Containerized deployment
- **Azure/AWS**: Cloud hosting
- **Automated Migrations**: Database updates on deployment

## 🤝 Contributing

This is a learning project, but contributions are welcome!

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 Development Roadmap

### Phase 1: MVP (Current)
- [x] Project structure
- [x] Clean Architecture setup
- [ ] Domain entities
- [ ] Database migrations
- [ ] Basic CRUD operations
- [ ] JWT authentication
- [ ] User registration/login

### Phase 2: Core Features
- [ ] Group management
- [ ] Invitation system
- [ ] Shopping lists CRUD
- [ ] Item management
- [ ] Purchase history
- [ ] Basic sync endpoint

### Phase 3: Advanced Features
- [ ] Smart sync (delta updates)
- [ ] Conflict resolution
- [ ] Push notifications
- [ ] Email service
- [ ] Templates
- [ ] Urgent items

### Phase 4: Mobile App
- [ ] Flutter project setup
- [ ] Offline database
- [ ] API integration
- [ ] Sync engine
- [ ] UI/UX polish

### Phase 5: Production Ready
- [ ] Comprehensive testing
- [ ] Performance optimization
- [ ] Security audit
- [ ] Docker deployment
- [ ] CI/CD pipeline
- [ ] Monitoring & logging

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Alecsander**
- Learning .NET and building real-world applications
- Focused on clean architecture and best practices

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- ASP.NET Core documentation
- Entity Framework Core guides
- The .NET community

---

**Note**: This is an educational project built to showcase modern .NET development practices and Clean Architecture principles.