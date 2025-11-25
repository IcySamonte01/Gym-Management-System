# 🧪 Test Your Login

## 🚀 Server is Running

- **URL**: http://localhost:3000
- **PID**: 7820
- **Status**: ✅ Ready for testing

---

## 🔐 Test 1: Default Admin Login

### **Try This First:**
1. Go to http://localhost:3000/login.html
2. Enter:
   - **Email**: `admin@gym.com`
   - **Password**: `admin123456`
3. Click **Login**

**Expected Result**: Should redirect to dashboard

---

## 🔐 Test 2: Your Custom User

### **Try Your User:**
1. Go to http://localhost:3000/login.html
2. Enter the credentials you created via Node.js registration
3. Click **Login**

**Expected Result**: Should work if:
- ✅ Password was hashed correctly by Node.js
- ✅ User is active in database
- ✅ Email matches exactly (case-sensitive)

---

## 🐛 If Login Fails

### **Check 1: View Browser Console**
Press **F12** in browser and check Console tab for errors.

### **Check 2: Check Network Tab**
1. Press **F12** in browser
2. Go to **Network** tab
3. Try to login
4. Look at the response from `/api/auth/login`

### **Check 3: Server Logs**
Check the terminal where the C# server is running for error messages.

---

## 📊 Test API Directly

You can also test the login API directly with PowerShell:

### **Test Default Admin:**
```powershell
$body = @{
    email = "admin@gym.com"
    password = "admin123456"
    rememberMe = $false
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "http://localhost:3000/api/auth/login" `
    -Method POST `
    -Body $body `
    -ContentType "application/json"

$response
```

**Expected Output:**
```json
{
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "...",
    "name": "Admin User",
    "email": "admin@gym.com",
    "role": "admin"
  }
}
```

### **Test Your Custom User:**
```powershell
$body = @{
    email = "your-email@example.com"
    password = "your-password"
    rememberMe = $false
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "http://localhost:3000/api/auth/login" `
    -Method POST `
    -Body $body `
    -ContentType "application/json"

$response
```

---

## 🔍 Check Your User in Database

If login fails, let's check what's in the database:

```powershell
# This will show you what's in the database
# Replace with your email
mongosh gym_management --eval "db.users.find({email: 'your-email@example.com'}).pretty()"
```

**Look for:**
- ✅ `password` starts with `$2a$` or `$2b$` (BCrypt hash)
- ✅ `isActive` is `true`
- ✅ `email` matches exactly

---

## 📝 Report Back

After testing, let me know:

1. **Did admin login work?** (admin@gym.com)
2. **Did your custom user work?** (the one you created)
3. **What error message did you see?** (if any)
4. **What's in the browser console?** (F12 → Console)
5. **What's in the server logs?** (terminal output)

---

**I'm here to help fix any issues! Just tell me what happened when you tried to login.**
