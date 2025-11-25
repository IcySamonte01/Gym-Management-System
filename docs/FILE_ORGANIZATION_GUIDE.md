# 📁 File Organization Guide

## 🎯 Where to Put New Files

Follow these guidelines when creating new files in the project:

---

## 📂 Frontend Files

### **HTML Pages** → `frontend/html/`

**When to use:**
- New web pages
- User interfaces
- Forms

**Template:**
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Your Page Title</title>
    <link rel="stylesheet" href="/css/style.css">
</head>
<body>
    <!-- Your content -->
    
    <script src="/js/auth-check.js"></script>
    <script src="/js/your-script.js"></script>
</body>
</html>
```

**Examples:**
- `frontend/html/new-feature.html`
- `frontend/html/reports.html`
- `frontend/html/settings.html`

---

### **CSS Files** → `frontend/css/`

**When to use:**
- Stylesheets
- Themes
- Component styles

**Reference in HTML:**
```html
<link rel="stylesheet" href="/css/your-style.css">
```

**Examples:**
- `frontend/css/dashboard-style.css`
- `frontend/css/custom-theme.css`
- `frontend/css/components.css`

---

### **JavaScript Files** → `frontend/js/`

**When to use:**
- Client-side logic
- Form validation
- UI interactions
- API calls

**Reference in HTML:**
```html
<script src="/js/your-script.js"></script>
```

**Examples:**
- `frontend/js/dashboard.js`
- `frontend/js/validation.js`
- `frontend/js/api-client.js`

---

## 🔧 Backend Files (C#)

### **Controllers** → `GymManagementAPI/Controllers/`

**When to use:**
- New API endpoints
- HTTP request handlers

**Template:**
```csharp
using Microsoft.AspNetCore.Mvc;
using GymManagementAPI.Services;
using GymManagementAPI.Models;

namespace GymManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YourController : ControllerBase
{
    private readonly IYourService _yourService;

    public YourController(IYourService yourService)
    {
        _yourService = yourService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _yourService.GetAllAsync();
        return Ok(items);
    }
}
```

**Examples:**
- `GymManagementAPI/Controllers/ReportsController.cs`
- `GymManagementAPI/Controllers/NotificationsController.cs`

---

### **Services** → `GymManagementAPI/Services/`

**When to use:**
- Business logic
- Database operations
- Data processing

**Create both interface and implementation:**

**Interface:** `IYourService.cs`
```csharp
namespace GymManagementAPI.Services;

public interface IYourService
{
    Task<List<YourModel>> GetAllAsync();
    Task<YourModel?> GetByIdAsync(string id);
    Task<YourModel> CreateAsync(YourModel model);
}
```

**Implementation:** `YourService.cs`
```csharp
using MongoDB.Driver;
using GymManagementAPI.Models;

namespace GymManagementAPI.Services;

public class YourService : IYourService
{
    private readonly IMongoCollection<YourModel> _collection;

    public YourService(IMongoDatabase database)
    {
        _collection = database.GetCollection<YourModel>("collectionName");
    }

    public async Task<List<YourModel>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
}
```

**Examples:**
- `GymManagementAPI/Services/IReportService.cs`
- `GymManagementAPI/Services/ReportService.cs`

---

### **Models** → `GymManagementAPI/Models/`

**When to use:**
- Data structures
- Database schemas
- DTOs (Data Transfer Objects)

**Template:**
```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GymManagementAPI.Models;

public class YourModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("fieldName")]
    public string FieldName { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Mongoose compatibility
    [BsonElement("__v")]
    [BsonIgnoreIfNull]
    public int? Version { get; set; }
}
```

**Examples:**
- `GymManagementAPI/Models/Report.cs`
- `GymManagementAPI/Models/Notification.cs`

---

## 📚 Documentation Files

### **Documentation** → `docs/`

**When to use:**
- User guides
- API documentation
- Technical specifications
- Setup instructions
- Troubleshooting guides

**Naming convention:**
- Use UPPERCASE for important docs: `SETUP_GUIDE.md`
- Use proper case for features: `Feature_Documentation.md`
- Use hyphens or underscores: `api-reference.md` or `API_REFERENCE.md`

**Examples:**
- `docs/API_REFERENCE.md`
- `docs/USER_GUIDE.md`
- `docs/DEPLOYMENT_GUIDE.md`
- `docs/TROUBLESHOOTING.md`

---

## 🛠️ Scripts & Utilities

### **Utility Scripts** → `scripts/`

**When to use:**
- Database maintenance
- Data migration
- Testing utilities
- Admin tools
- Automation scripts

**Examples:**
- `scripts/backup-database.js`
- `scripts/seed-data.js`
- `scripts/generate-reports.js`
- `scripts/cleanup-old-data.js`

---

## ⚙️ Configuration Files

### **Configuration** → `config/`

**When to use:**
- Package dependencies
- Environment settings
- Build configurations

**Examples:**
- `config/package.json`
- `config/.env.example`
- `config/tsconfig.json`

**Note:** Keep `appsettings.json` in `GymManagementAPI/` as it's C# specific.

---

## 🧪 Test Files

### **Create a Tests Folder** → `tests/` (if needed)

**When to use:**
- Unit tests
- Integration tests
- End-to-end tests

**Structure:**
```
tests/
├── unit/
│   ├── services/
│   └── models/
├── integration/
│   └── api/
└── e2e/
    └── scenarios/
```

**Examples:**
- `tests/unit/services/UserServiceTests.cs`
- `tests/integration/api/AuthControllerTests.cs`

---

## 📦 Static Assets (Future)

### **If you add images/fonts/etc.** → `frontend/assets/`

**Structure:**
```
frontend/assets/
├── images/
│   ├── logo.png
│   └── icons/
├── fonts/
│   └── custom-font.woff
└── files/
    └── sample.pdf
```

**Reference in HTML:**
```html
<img src="/assets/images/logo.png" alt="Logo">
```

**Update Program.cs:**
```csharp
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(parentDirectory, "frontend", "assets")),
    RequestPath = "/assets"
});
```

---

## 🗂️ Complete Folder Structure Reference

```
Gym Management System/
│
├── frontend/                    # All frontend code
│   ├── html/                   # Web pages
│   ├── css/                    # Stylesheets
│   ├── js/                     # JavaScript
│   └── assets/                 # Static assets (future)
│       ├── images/
│       ├── fonts/
│       └── files/
│
├── GymManagementAPI/           # C# Backend
│   ├── Controllers/            # API endpoints
│   ├── Services/               # Business logic
│   ├── Models/                 # Data models
│   ├── Properties/             # Project settings
│   ├── Program.cs              # Entry point
│   └── appsettings.json        # Configuration
│
├── docs/                       # Documentation
│   ├── QUICK_START.md
│   ├── API_REFERENCE.md
│   └── ...
│
├── scripts/                    # Utility scripts
│   ├── create-admin.js
│   ├── backup-db.js
│   └── ...
│
├── config/                     # Configuration files
│   ├── package.json
│   ├── .gitignore
│   └── ...
│
├── tests/                      # Tests (future)
│   ├── unit/
│   ├── integration/
│   └── e2e/
│
├── textfile/                   # Legacy docs
├── node_modules/               # Dependencies
├── server.js                   # Node.js backend
└── README.md                   # Main readme
```

---

## ✅ Quick Reference Checklist

**Before creating a new file, ask:**

- [ ] Is it a web page? → `frontend/html/`
- [ ] Is it styling? → `frontend/css/`
- [ ] Is it client-side code? → `frontend/js/`
- [ ] Is it an API endpoint? → `GymManagementAPI/Controllers/`
- [ ] Is it business logic? → `GymManagementAPI/Services/`
- [ ] Is it a data model? → `GymManagementAPI/Models/`
- [ ] Is it documentation? → `docs/`
- [ ] Is it a utility script? → `scripts/`
- [ ] Is it configuration? → `config/`
- [ ] Is it a test? → `tests/`

---

## 🔄 After Creating Files

### **For Frontend Files:**

1. **HTML files**: No extra steps needed
2. **CSS files**: Reference in HTML with `/css/filename.css`
3. **JS files**: Reference in HTML with `/js/filename.js`

### **For Backend Files:**

1. **Controllers**: Register in `Program.cs` if needed
2. **Services**: Add to dependency injection in `Program.cs`:
   ```csharp
   builder.Services.AddScoped<IYourService, YourService>();
   ```
3. **Models**: No extra steps needed

---

## 📝 Naming Conventions

### **Files:**
- Use **PascalCase** for C# files: `UserService.cs`
- Use **kebab-case** for frontend files: `user-profile.html`
- Use **SCREAMING_SNAKE_CASE** or **kebab-case** for docs: `API_REFERENCE.md` or `api-reference.md`

### **Classes/Interfaces:**
- **PascalCase**: `UserService`, `IUserService`

### **Variables:**
- **camelCase**: `userName`, `emailAddress`

### **Constants:**
- **SCREAMING_SNAKE_CASE**: `MAX_LOGIN_ATTEMPTS`

---

## 💡 Pro Tips

### **Keep Related Files Together**
```
frontend/
├── html/
│   └── user-profile.html
├── css/
│   └── user-profile.css
└── js/
    └── user-profile.js
```

### **Use Clear, Descriptive Names**
✅ `frontend/html/member-registration-form.html`  
❌ `frontend/html/form1.html`

### **Follow Existing Patterns**
Look at existing files for naming and structure examples.

### **Update Documentation**
When adding major features, update the relevant docs in `docs/`.

---

## 🎯 Examples of Adding New Features

### **Example 1: Add a Reports Feature**

**Steps:**
1. Create model: `GymManagementAPI/Models/Report.cs`
2. Create service: `GymManagementAPI/Services/IReportService.cs` & `ReportService.cs`
3. Create controller: `GymManagementAPI/Controllers/ReportsController.cs`
4. Create frontend page: `frontend/html/reports.html`
5. Create frontend script: `frontend/js/reports.js`
6. Create frontend styles: `frontend/css/reports.css` (if needed)
7. Document it: `docs/REPORTS_FEATURE.md`

### **Example 2: Add a Backup Script**

**Steps:**
1. Create script: `scripts/backup-database.js`
2. Make it executable
3. Document usage: Add to `docs/SCRIPTS_GUIDE.md`

---

## 🔍 Finding Files

### **By Type:**
- HTML: `Get-ChildItem frontend/html/*.html`
- CSS: `Get-ChildItem frontend/css/*.css`
- JS: `Get-ChildItem frontend/js/*.js`
- Controllers: `Get-ChildItem GymManagementAPI/Controllers/*Controller.cs`
- Docs: `Get-ChildItem docs/*.md`

### **By Name:**
```powershell
Get-ChildItem -Recurse -Filter "*user*"
```

---

**Follow this guide to keep your project organized! 📁✨**
