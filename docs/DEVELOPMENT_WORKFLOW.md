# 🔄 Development Workflow Guide

## 📝 File Organization Rules

**IMPORTANT:** Always create new files in their designated folders!

---

## ✅ Quick Reference: Where Files Go

| File Type | Folder | URL Path |
|-----------|--------|----------|
| HTML pages | `frontend/html/` | `http://localhost:3000/page.html` |
| CSS stylesheets | `frontend/css/` | `/css/style.css` |
| JavaScript files | `frontend/js/` | `/js/script.js` |
| C# Controllers | `GymManagementAPI/Controllers/` | `/api/...` |
| C# Services | `GymManagementAPI/Services/` | N/A (internal) |
| C# Models | `GymManagementAPI/Models/` | N/A (internal) |
| Documentation | `docs/` | N/A |
| Utility Scripts | `scripts/` | N/A |
| Configuration | `config/` or root | N/A |

---

## 🚀 Adding New Features

### **Step-by-Step Workflow**

#### **1. Plan the Feature**
- [ ] Define requirements
- [ ] Design data model
- [ ] Plan API endpoints
- [ ] Design UI

#### **2. Create Backend (C#)**

**A. Create Model** (`GymManagementAPI/Models/`)
```csharp
// GymManagementAPI/Models/YourFeature.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GymManagementAPI.Models;

public class YourFeature
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("__v")]
    [BsonIgnoreIfNull]
    public int? Version { get; set; }
}
```

**B. Create Service** (`GymManagementAPI/Services/`)

Interface: `IYourFeatureService.cs`
```csharp
using GymManagementAPI.Models;

namespace GymManagementAPI.Services;

public interface IYourFeatureService
{
    Task<List<YourFeature>> GetAllAsync();
    Task<YourFeature?> GetByIdAsync(string id);
    Task<YourFeature> CreateAsync(YourFeature feature);
    Task<YourFeature?> UpdateAsync(string id, YourFeature feature);
    Task<bool> DeleteAsync(string id);
}
```

Implementation: `YourFeatureService.cs`
```csharp
using GymManagementAPI.Models;
using MongoDB.Driver;

namespace GymManagementAPI.Services;

public class YourFeatureService : IYourFeatureService
{
    private readonly IMongoCollection<YourFeature> _collection;

    public YourFeatureService(IMongoDatabase database)
    {
        _collection = database.GetCollection<YourFeature>("yourfeatures");
    }

    public async Task<List<YourFeature>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
    
    // Implement other methods...
}
```

**C. Register Service** (`GymManagementAPI/Program.cs`)
```csharp
builder.Services.AddScoped<IYourFeatureService, YourFeatureService>();
```

**D. Create Controller** (`GymManagementAPI/Controllers/`)
```csharp
using GymManagementAPI.Models;
using GymManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YourFeatureController : ControllerBase
{
    private readonly IYourFeatureService _service;

    public YourFeatureController(IYourFeatureService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] YourFeature feature)
    {
        var created = await _service.CreateAsync(feature);
        return StatusCode(201, created);
    }
    
    // Add other endpoints...
}
```

#### **3. Create Frontend**

**A. Create HTML** (`frontend/html/`)
```html
<!-- frontend/html/your-feature.html -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Your Feature</title>
    <link rel="stylesheet" href="/css/style.css">
    <link rel="stylesheet" href="/css/your-feature.css">
</head>
<body>
    <!-- Your UI here -->
    
    <script src="/js/auth-check.js"></script>
    <script src="/js/your-feature.js"></script>
</body>
</html>
```

**B. Create JavaScript** (`frontend/js/`)
```javascript
// frontend/js/your-feature.js
document.addEventListener('DOMContentLoaded', async () => {
    await loadFeatures();
});

async function loadFeatures() {
    try {
        const response = await authenticatedFetch('/api/yourfeature');
        const features = await response.json();
        displayFeatures(features);
    } catch (error) {
        console.error('Error loading features:', error);
    }
}

function displayFeatures(features) {
    // Display logic here
}
```

**C. Create CSS** (`frontend/css/`) - Optional
```css
/* frontend/css/your-feature.css */
.feature-container {
    padding: 20px;
}

.feature-card {
    background: white;
    border-radius: 8px;
    padding: 15px;
    margin: 10px 0;
}
```

#### **4. Test the Feature**

**A. Test Backend**
```powershell
# Test API endpoint
Invoke-RestMethod -Uri "http://localhost:3000/api/yourfeature" -Method GET
```

**B. Test Frontend**
- Open `http://localhost:3000/your-feature.html`
- Check browser console for errors
- Test all CRUD operations

#### **5. Document the Feature**

Create `docs/YOUR_FEATURE.md`:
```markdown
# Your Feature Documentation

## Overview
Brief description of the feature.

## API Endpoints
- GET /api/yourfeature - Get all items
- POST /api/yourfeature - Create item
- PUT /api/yourfeature/:id - Update item
- DELETE /api/yourfeature/:id - Delete item

## Usage
How to use the feature from the UI.

## Technical Details
Implementation details, database schema, etc.
```

---

## 🔄 Daily Development Workflow

### **Morning Routine**
```powershell
# 1. Pull latest changes (if using git)
git pull

# 2. Install any new dependencies
cd config
npm install
cd ..

cd GymManagementAPI
dotnet restore
cd ..

# 3. Start MongoDB
net start MongoDB

# 4. Start the development server
cd GymManagementAPI
dotnet watch run  # Auto-restarts on changes
```

### **During Development**

**Before creating any file:**
1. Check `docs/FILE_ORGANIZATION_GUIDE.md`
2. Create file in correct folder
3. Follow naming conventions
4. Use proper references/imports

**When adding features:**
1. Create backend first (Model → Service → Controller)
2. Test API endpoints
3. Create frontend (HTML → JS → CSS)
4. Test UI
5. Document changes

### **Before Committing (if using git)**
```powershell
# 1. Test everything works
dotnet build
npm test

# 2. Check for lint errors
# (add linting tools as needed)

# 3. Update documentation
# Add/update relevant .md files in docs/

# 4. Review changes
git status
git diff

# 5. Commit with clear message
git add .
git commit -m "feat: Add your feature description"
```

---

## 📁 File Checklist

Before creating a new file, verify:

- [ ] File is in the correct folder
- [ ] Naming convention followed
- [ ] Proper file extension
- [ ] References/imports use correct paths
- [ ] Related files created (e.g., interface + implementation)

---

## 🎯 Common Tasks

### **Adding a New Page**

1. Create HTML in `frontend/html/new-page.html`
2. Create JS in `frontend/js/new-page.js`
3. Create CSS in `frontend/css/new-page.css` (optional)
4. Add navigation link in menu
5. Test: `http://localhost:3000/new-page.html`

### **Adding a New API Endpoint**

1. Create/update Model in `GymManagementAPI/Models/`
2. Create/update Service in `GymManagementAPI/Services/`
3. Add method in Controller
4. Register service in `Program.cs` if new
5. Test with curl or Postman

### **Adding a Utility Script**

1. Create script in `scripts/your-script.js`
2. Add documentation comment at top
3. Make it executable if needed
4. Document in `docs/SCRIPTS_GUIDE.md`

### **Adding Documentation**

1. Create .md file in `docs/YOUR_DOC.md`
2. Use clear headers and formatting
3. Include examples
4. Update main README.md with link if major

---

## 🚫 What NOT to Do

❌ Don't create files in the root directory  
❌ Don't mix file types in same folder  
❌ Don't use unclear file names like `test1.html`  
❌ Don't skip documentation  
❌ Don't commit sensitive data (.env files)  
❌ Don't modify `node_modules` directly  
❌ Don't change folder structure without updating `Program.cs`  

---

## ✅ Best Practices

### **Code Quality**
- Write clear, self-documenting code
- Add comments for complex logic
- Follow existing code style
- Use meaningful variable names

### **Security**
- Never commit passwords or API keys
- Validate all user input
- Use parameterized queries
- Check authorization on all endpoints

### **Performance**
- Use async/await for I/O operations
- Optimize database queries
- Minimize frontend bundle sizes
- Use caching where appropriate

### **Testing**
- Test manually before committing
- Write unit tests for business logic
- Test edge cases
- Verify error handling

---

## 🛠️ Useful Commands

### **Development**
```powershell
# Start server with auto-reload
cd GymManagementAPI
dotnet watch run

# Check for build errors
dotnet build

# Run tests (when added)
dotnet test

# View logs
# Check terminal output
```

### **Database**
```powershell
# Connect to MongoDB
mongosh gym_management

# View collections
show collections

# Query data
db.users.find().pretty()

# Run maintenance script
node scripts/backup-database.js
```

### **Frontend**
```powershell
# Install npm packages
cd config
npm install package-name

# Check for updates
npm outdated
```

---

## 📊 Folder Organization Summary

```
✅ CORRECT:
frontend/html/new-feature.html
frontend/css/new-feature.css
frontend/js/new-feature.js
GymManagementAPI/Controllers/NewFeatureController.cs
GymManagementAPI/Services/NewFeatureService.cs
GymManagementAPI/Models/NewFeature.cs
docs/NEW_FEATURE.md
scripts/utility-script.js

❌ INCORRECT:
new-feature.html              (root)
NewFeatureController.cs       (root)
my-script.js                  (root)
notes.txt                     (root)
```

---

## 🎓 Learning Resources

### **For New Developers**
1. Read `README.md` first
2. Study `docs/FOLDER_STRUCTURE.md`
3. Review existing code
4. Follow this workflow guide

### **Before Making Changes**
1. Understand current structure
2. Check where similar files are
3. Follow existing patterns
4. Ask if unsure!

---

## 📞 Need Help?

If you're unsure where to put a file:
1. Check `docs/FILE_ORGANIZATION_GUIDE.md`
2. Look at similar existing files
3. Follow the patterns you see
4. When in doubt, ask!

---

**Keep your code organized and your project will stay maintainable! 📁✨**
