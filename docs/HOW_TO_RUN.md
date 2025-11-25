# 🚀 How to Run Your Gym Management System

You now have **TWO backends** to choose from:

---

## Option 1: C# Backend (Recommended - Better Performance)

### **Start Server**
```powershell
cd GymManagementAPI
dotnet run
```

### **Access**
- URL: http://localhost:3000
- Login: http://localhost:3000/login.html
- Swagger API: http://localhost:3000/swagger

### **Stop Server**
```powershell
Get-Process -Name "GymManagementAPI" | Stop-Process -Force
```

---

## Option 2: Node.js Backend (Original)

### **Start Server**
```powershell
npm start
```

### **Access**
- URL: http://localhost:3000
- Login: http://localhost:3000/login.html

### **Stop Server**
```powershell
# Press Ctrl+C in the terminal
# Or find and kill the process:
Get-Process -Name "node" | Stop-Process -Force
```

---

## 🔐 Default Login

**Admin Account:**
- Email: `admin@gym.com`
- Password: `admin123456`

⚠️ **Change this password after first login!**

---

## 📊 Comparison

| Feature | Node.js | C# | Winner |
|---------|---------|-----|--------|
| Performance | Good | Excellent | 🏆 C# |
| Setup | Easy | Moderate | 🏆 Node.js |
| Type Safety | No | Yes | 🏆 C# |
| Memory | Higher | Lower | 🏆 C# |
| Frontend | Same | Same | 🤝 Tie |
| Database | Same MongoDB | Same MongoDB | 🤝 Tie |

---

## 🎯 Quick Commands

### **Prerequisites Check**
```powershell
# Check Node.js
node --version

# Check .NET
dotnet --version

# Check MongoDB
Get-Service -Name MongoDB*
```

### **Install Dependencies**

**For Node.js:**
```powershell
npm install
```

**For C#:**
```powershell
cd GymManagementAPI
dotnet restore
```

---

## 🌐 Access Points

Once server is running, access:

- **Login**: http://localhost:3000/login.html
- **Register**: http://localhost:3000/register.html
- **Dashboard**: http://localhost:3000/dashboard.html
- **Admin Panel**: http://localhost:3000/admin-users.html
- **Members**: http://localhost:3000/members.html
- **Coaches**: http://localhost:3000/coaches.html
- **Schedules**: http://localhost:3000/schedules.html
- **Payments**: http://localhost:3000/payments.html

---

## 📚 Documentation

- **C# Setup**: See `QUICK_START_CSHARP.md`
- **Migration**: See `MIGRATION_GUIDE.md`
- **Complete Guide**: See `CSHARP_BACKEND_COMPLETE.md`
- **Node.js Setup**: See `textfile/SETUP_GUIDE.md`

---

**Choose your backend and start coding! 🎉**
