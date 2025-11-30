# 📁 Gym Management System - Folder Structure

This document describes the organized folder structure of the Gym Management System project.

## 📂 Root Directory

```
Gym Management System/
├── 📄 README.md                    # Main project documentation
├── 📄 .gitignore                   # Git ignore rules
├── 📄 CNAME                        # GitHub Pages custom domain
├── 📄 FOLDER_STRUCTURE.md          # This file
├── 📄 Gym Management System.sln    # Visual Studio solution file
├── 📁 GymManagementAPI/            # C# .NET 8 Backend
├── 📁 frontend/                    # Frontend application
├── 📁 docs/                        # Documentation
├── 📁 scripts/                     # Database scripts
├── 📁 tools/                       # Development tools
└── 📁 archive/                     # Legacy files
```

## 📚 Detailed Structure

### 🔧 `/GymManagementAPI/` - C# .NET 8 Backend
The main backend API built with ASP.NET Core.

```
GymManagementAPI/
├── Controllers/          # API endpoints
│   ├── AuthController.cs
│   ├── MembersController.cs
│   ├── CoachesController.cs
│   ├── SchedulesController.cs
│   └── PaymentsController.cs
├── Models/              # Data models
│   ├── User.cs
│   ├── Member.cs
│   ├── Coach.cs
│   ├── Schedule.cs
│   └── Payment.cs
├── Services/            # Business logic
│   ├── AuthService.cs
│   ├── MemberService.cs
│   ├── CoachService.cs
│   └── ...
├── Program.cs           # Application entry point
├── appsettings.json     # Configuration
└── README.md            # Backend documentation
```

### 🎨 `/frontend/` - Frontend Application
HTML, CSS, and JavaScript files for the user interface.

```
frontend/
├── html/               # HTML pages
│   ├── login.html
│   ├── dashboard.html
│   ├── members.html
│   ├── coaches.html
│   ├── schedules.html
│   ├── payments.html
│   └── admin-users.html
├── css/                # Stylesheets
│   ├── style.css       # Global styles + user menu
│   ├── login-style.css
│   ├── dashboard.css
│   ├── members.css
│   ├── coaches.css
│   └── schedules.css
└── js/                 # JavaScript files
    ├── auth-check.js   # Authentication & user menu
    ├── login.js
    ├── dashboard.js
    ├── members.js
    ├── coaches.js
    ├── schedules.js
    └── payments.js
```

### 📖 `/docs/` - Documentation
Comprehensive project documentation.

```
docs/
├── README_CSHARP_BACKEND.md
├── HOW_TO_RUN.md
├── DEVELOPMENT_WORKFLOW.md
├── AUTHENTICATION_FLOW.md
├── ROLE_BASED_ACCESS_CONTROL.md
├── FEATURES_COMPLETE.md           # Moved from root
├── GIT_PUSH_INSTRUCTIONS.md       # Moved from root
├── SESSION_SUMMARY.md             # Moved from root
├── textfile/                      # Auth documentation
└── ...                            # Many other feature docs
```

### 🗄️ `/scripts/` - Database Scripts
MongoDB initialization and maintenance scripts.

```
scripts/
├── create-admin.js              # Create admin user
├── create-default-admin.js      # Default admin setup
├── fix-duplicate-index.js       # Database fixes
└── fix-googleid-final.js        # Google auth fixes
```

### 🛠️ `/tools/` - Development Tools
Utility tools for development and administration.

```
tools/
├── acli.exe          # Atlassian CLI (Jira/Confluence)
└── README.md         # Tools documentation
```

### 📦 `/archive/` - Legacy Files
Legacy Node.js backend (replaced by C# .NET).

```
archive/
├── server.js         # Original Node.js Express server
├── config/           # Node.js configuration
│   ├── package.json
│   └── package-lock.json
└── README.md         # Archive information
```

## 🎯 Organization Principles

### Clean Root Directory
The root level contains only essential files:
- **Solution file** (.sln)
- **Main README**
- **Git configuration** (.gitignore)
- **GitHub Pages config** (CNAME)
- **This structure guide** (FOLDER_STRUCTURE.md)

### Logical Grouping
- **Backend code**: `/GymManagementAPI/`
- **Frontend code**: `/frontend/`
- **Documentation**: `/docs/`
- **Database scripts**: `/scripts/`
- **Development tools**: `/tools/`
- **Legacy files**: `/archive/`

### Separation of Concerns
- Backend and frontend are clearly separated
- Documentation is centralized in `/docs/`
- Legacy code is archived separately
- Tools are isolated in `/tools/`

## 🚀 Quick Start

1. **Run the backend**: See `/GymManagementAPI/README.md`
2. **Open frontend**: Navigate to `/frontend/html/login.html`
3. **Read documentation**: Check `/docs/HOW_TO_RUN.md`
4. **Setup database**: Use scripts in `/scripts/`

## 📝 File Naming Conventions

- **PascalCase**: C# files (Controllers, Models, Services)
- **kebab-case**: HTML, CSS files
- **camelCase**: JavaScript files
- **UPPERCASE**: Documentation files (README.md, etc.)

## 🔄 Recent Changes

### Reorganization (Latest)
- Moved `acli.exe` → `tools/`
- Moved `server.js` + `config/` → `archive/`
- Moved documentation files → `docs/`
- Created README files for new directories
- Maintained clean root directory

## 📚 Additional Resources

- **Main README**: `/README.md`
- **Backend Setup**: `/GymManagementAPI/README.md`
- **How to Run**: `/docs/HOW_TO_RUN.md`
- **Development Guide**: `/docs/DEVELOPMENT_WORKFLOW.md`
- **Feature Documentation**: `/docs/` (multiple files)

---

**Last Updated**: January 2025  
**Project**: Gym Management System v2.0  
**Backend**: C# .NET 8  
**Frontend**: HTML/CSS/JavaScript  
**Database**: MongoDB
