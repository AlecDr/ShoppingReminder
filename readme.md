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
┌────────────────┴────────────────────────┐
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
- **.NET 10**: Latest .NET version
- **ASP.NET Core Web API**: RESTful API
- **Entity Framework Core 10**: ORM with code-first migrations
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

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Visual Studio 2022 17.13+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Docker](https://www.docker.com/) (optional, for containerized PostgreSQL)

## 🚀 Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/shopping-reminder.git
cd shopping-reminder
```

### 2. Set Up Database (Docker - easiest)
```bash
docker run --name postgres-shopping \
  -e POSTGRES_PASSWORD=yourpassword \
  -e POSTGRES_DB=ShoppingReminderDb \
  -p 5432:5432 \
  -d postgres:15
```

### 3. Configure Secrets (never commit passwords!)
```bash
# Database connection
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=ShoppingReminderDb;Username=postgres;Password=YOUR_PASSWORD" --project ShoppingReminder.API/ShoppingReminder.API.csproj

# JWT secret (generate a strong 32+ character key)
dotnet user-secrets set "JwtSettings:Secret" "your-super-secret-jwt-key-min-32-chars" --project ShoppingReminder.API/ShoppingReminder.API.csproj
```

### 4. Run Migrations
```bash
dotnet ef database update --project ShoppingReminder.Infrastructure/ShoppingReminder.Infrastructure.csproj
```

### 5. Run the API
```bash
cd ShoppingReminder.API
dotnet run
```

### 6. Open Swagger UI
Navigate to: `https://localhost:7001/swagger`

---

## 📖 Detailed Setup Guide

**For complete setup instructions including:**
- PostgreSQL installation options
- User Secrets configuration
- JWT settings
- Troubleshooting
- Production deployment

**👉 See [SETUP.md](SETUP.md)**

---

## 📚 API Documentation

Once the API is running, visit the Swagger UI at: `https://localhost:7001/swagger`

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

### Run All Tests
```bash
dotnet test
```

### Run Unit Tests
```bash
dotnet test tests/ShoppingReminder.UnitTests
```

### Run Integration Tests
```bash
dotnet test tests/ShoppingReminder.IntegrationTests
```

### Run with Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 🗄️ Database Schema

### Core Tables

- **Users**: User accounts and authentication
- **Groups**: Collaborative groups (families, roommates)
- **GroupMembers**: User membership in groups with roles (Owner, Admin, Member, Viewer)
- **ShoppingLists**: Lists within groups
- **ShoppingItems**: Individual items in lists
- **Invitations**: Pending group invitations
- **PurchaseHistory**: Denormalized history for quick re-adds

### Key Features

- **Soft Deletes**: All entities support reversible deletion with audit trail
- **Audit Trail**: Track who created/modified/deleted and when
- **Optimistic Concurrency**: Version numbers for conflict resolution
- **Indexes**: Optimized for common query patterns

## 🔐 Security

- **JWT Tokens**: Bearer token authentication with refresh tokens
- **Password Hashing**: Secure password hashing with BCrypt
- **Email Verification**: Required for new accounts
- **Role-Based Access**: Group owners, admins, members, viewers
- **User Secrets**: Sensitive data never committed to source control
- **Rate Limiting**: Prevent API abuse
- **CORS**: Configured for mobile app origins

### Security Best Practices

⚠️ **Never commit:**
- Connection strings with passwords
- JWT secrets
- API keys
- Files with sensitive data

✅ **Always use:**
- User Secrets for local development
- Environment Variables for production
- Strong, random JWT secrets (minimum 32 characters)
- Different secrets for each environment

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
- **Health Checks**: Monitor application status

## 🤝 Contributing

This is a learning project, but contributions are welcome!

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 Development Roadmap

### Phase 1: Foundation ✅
- [x] Project structure
- [x] Clean Architecture setup
- [x] Domain entities
- [x] Database migrations
- [x] User Secrets configuration
- [x] Soft deletes and audit trail

### Phase 2: Authentication & Authorization 🔄
- [ ] User registration with email verification
- [ ] Login with JWT tokens
- [ ] Refresh token mechanism
- [ ] Password reset flow
- [ ] Role-based authorization

### Phase 3: Core Features
- [ ] Group management
- [ ] Invitation system
- [ ] Shopping lists CRUD
- [ ] Item management
- [ ] Purchase history
- [ ] Basic sync endpoint

### Phase 4: Advanced Features
- [ ] Smart sync (delta updates)
- [ ] Conflict resolution
- [ ] Push notifications
- [ ] Email service
- [ ] Templates
- [ ] Urgent items

### Phase 5: Mobile App
- [ ] Flutter project setup
- [ ] Offline database
- [ ] API integration
- [ ] Sync engine
- [ ] UI/UX polish

### Phase 6: Production Ready
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
- Location: Sant'Ana do Livramento, RS, Brazil

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- ASP.NET Core documentation
- Entity Framework Core guides
- The .NET community

## 📞 Support

For questions or issues:
- Open an [issue](https://github.com/yourusername/shopping-reminder/issues)
- Check [SETUP.md](SETUP.md) for detailed configuration
- Review existing issues for solutions

---

**Note**: This is an educational project built to showcase modern .NET development practices and Clean Architecture principles.

## 🌟 Star History

If you find this project helpful, please consider giving it a star! ⭐

---

Built with ❤️ using .NET 10, PostgreSQL, and Clean Architecture