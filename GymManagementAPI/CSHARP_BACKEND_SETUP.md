# C# ASP.NET Core Backend Setup Guide

This is a complete C# replacement for the Node.js/Express backend, using ASP.NET Core 8.0 and MongoDB.

## 📋 Prerequisites

1. **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **MongoDB** - Already installed and running
3. **Visual Studio 2022** or **Visual Studio Code** (optional)

## 🚀 Quick Start

### Step 1: Install .NET 8.0 SDK

Download and install the .NET 8.0 SDK from Microsoft's website.

Verify installation:
```powershell
dotnet --version
```

### Step 2: Restore NuGet Packages

Navigate to the GymManagementAPI folder:
```powershell
cd GymManagementAPI
dotnet restore
```

### Step 3: Update Configuration (Optional)

Edit `appsettings.json` to configure:
- MongoDB connection string
- JWT secret key
- Google OAuth credentials (if needed)
- Server port

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017/gym_management"
  },
  "Jwt": {
    "Secret": "your_super_secret_jwt_key_change_this_in_production",
    "ExpirationDays": 7
  },
  "PORT": "3000"
}
```

### Step 4: Build the Application

```powershell
dotnet build
```

### Step 5: Run the Application

```powershell
dotnet run
```

The server will start on `http://localhost:3000`

## 📁 Project Structure

```
GymManagementAPI/
├── Controllers/           # API endpoints
│   ├── AuthController.cs        # Authentication (login, register, verify)
│   ├── AdminController.cs       # Admin user management
│   ├── MembersController.cs     # Member CRUD operations
│   ├── CoachesController.cs     # Coach CRUD operations
│   ├── PaymentsController.cs    # Payment operations
│   ├── SchedulesController.cs   # Schedule management
│   └── DashboardController.cs   # Dashboard statistics
├── Models/               # Data models
│   ├── User.cs          # User model and DTOs
│   ├── Member.cs        # Member model
│   ├── Coach.cs         # Coach model
│   ├── Payment.cs       # Payment model
│   └── Schedule.cs      # Schedule model
├── Services/            # Business logic
│   ├── IAuthService.cs / AuthService.cs       # JWT & password handling
│   ├── IUserService.cs / UserService.cs       # User operations
│   ├── IMemberService.cs / MemberService.cs   # Member operations
│   ├── ICoachService.cs / CoachService.cs     # Coach operations
│   ├── IPaymentService.cs / PaymentService.cs # Payment operations
│   └── IScheduleService.cs / ScheduleService.cs # Schedule operations
├── Program.cs           # Application entry point
├── appsettings.json     # Configuration
└── GymManagementAPI.csproj  # Project file
```

## 🔐 Create Admin User

### Method 1: Using the Default Admin Account
The existing MongoDB database should already have the default admin:
- **Email**: `admin@gym.com`
- **Password**: `admin123456`

### Method 2: Command Line Tool
You can create a C# console app to create admins:

```powershell
# Add this code to a separate Program.cs or run it manually
dotnet run -- create-admin
```

### Method 3: API Call (from existing admin)
Once you have an admin account, use the API:

```http
POST /api/admin/users
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "name": "New Admin",
  "email": "newadmin@gym.com",
  "password": "securepassword",
  "role": "admin"
}
```

## 🌐 API Endpoints

All endpoints are identical to the Node.js version for frontend compatibility:

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login
- `GET /api/auth/verify` - Verify token
- `POST /api/auth/logout` - Logout
- `GET /api/auth/profile` - Get user profile

### Admin (Requires admin role)
- `GET /api/admin/users` - Get all users
- `POST /api/admin/users` - Create user (including admin)
- `PATCH /api/admin/users/:id/role` - Update user role
- `PATCH /api/admin/users/:id/activate` - Activate user
- `PATCH /api/admin/users/:id/deactivate` - Deactivate user
- `DELETE /api/admin/users/:id` - Delete user

### Members
- `GET /api/members` - Get all members
- `GET /api/members/:id` - Get member by ID
- `POST /api/members` - Create member
- `PUT /api/members/:id` - Update member
- `DELETE /api/members/:id` - Delete member

### Coaches
- `GET /api/coaches` - Get all coaches
- `POST /api/coaches` - Create coach
- `PUT /api/coaches/:id` - Update coach
- `DELETE /api/coaches/:id` - Delete coach

### Payments
- `GET /api/payments` - Get all payments
- `POST /api/payments` - Create payment

### Schedules
- `GET /api/schedules` - Get all schedules
- `POST /api/schedules` - Create schedule
- `PUT /api/schedules/:id` - Update schedule
- `DELETE /api/schedules/:id` - Delete schedule

### Dashboard
- `GET /api/dashboard/stats` - Get dashboard statistics

## 🔑 Key Features

### ✅ Security
- **JWT Authentication** with BCrypt password hashing
- **Role-based Authorization** (admin, coach, member)
- **Admin registration blocked** on public endpoints
- **Self-protection** - admins can't delete/deactivate themselves

### ✅ MongoDB Integration
- Native MongoDB.Driver support
- Async/await pattern throughout
- Proper data mapping with BSON attributes

### ✅ Clean Architecture
- **Controllers** handle HTTP requests/responses
- **Services** contain business logic
- **Models** define data structures
- **Dependency Injection** for loose coupling

### ✅ Frontend Compatible
- All API endpoints match the Node.js version
- Same request/response formats
- CORS enabled for frontend communication

## 🔄 Migration from Node.js

### What Changed:
1. **Language**: JavaScript → C#
2. **Framework**: Express.js → ASP.NET Core
3. **ORM**: Mongoose → MongoDB.Driver

### What Stayed the Same:
1. **Database**: MongoDB (same collections and schema)
2. **API Routes**: Identical endpoints
3. **Authentication**: JWT tokens (same format)
4. **Frontend**: No changes needed!

### Frontend Compatibility
Your existing HTML/JavaScript frontend will work **without any changes** because:
- API endpoints are identical
- Request/response JSON format is the same
- JWT tokens work the same way
- CORS is properly configured

## 🛠️ Development Commands

```powershell
# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Run with hot reload (watches for file changes)
dotnet watch run

# Run in production mode
dotnet run --configuration Release

# Publish for deployment
dotnet publish -c Release -o ./publish
```

## 📊 Swagger API Documentation

When running in development mode, access interactive API docs at:
```
http://localhost:3000/swagger
```

## 🐛 Troubleshooting

### Port Already in Use
Change the port in `appsettings.json` or use command line:
```powershell
dotnet run --urls="http://localhost:5000"
```

### MongoDB Connection Error
Verify MongoDB is running:
```powershell
Get-Service -Name MongoDB*
```

### Package Restore Issues
```powershell
dotnet clean
dotnet restore --force
dotnet build
```

## 🚀 Production Deployment

1. **Publish the application**:
```powershell
dotnet publish -c Release -o ./publish
```

2. **Update appsettings.json** with production values

3. **Run on IIS, Azure App Service, or Docker**

4. **Set environment variables**:
```powershell
$env:ASPNETCORE_ENVIRONMENT = "Production"
```

## 🎯 Next Steps

1. ✅ Test all API endpoints with the frontend
2. ✅ Change default admin password
3. ✅ Update JWT secret in production
4. ✅ Configure Google OAuth (if needed)
5. ✅ Set up logging and monitoring
6. ✅ Add rate limiting for security

## 📞 Support

- Frontend works with both Node.js and C# backends
- Same MongoDB database is used
- No data migration needed
- Switch between backends by changing the port!

---

**Your frontend will work immediately with this C# backend! 🎉**
