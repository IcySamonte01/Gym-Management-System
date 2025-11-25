# 🚀 Authentication System - Quick Reference

## 🎯 TL;DR - What Was Added

A complete login system with **email/password** and **Google Sign-In** for your Gym Management System!

## 📋 Quick Access URLs

```
Login:        http://localhost:3000/login.html
Register:     http://localhost:3000/register.html
Dashboard:    http://localhost:3000/dashboard.html
```

## 🔑 Test Account

Already created for you:
- **Email:** `testuser@gym.com`
- **Password:** `test123456`
- **Role:** Member

## ⚡ Quick Start (3 Steps)

1. **Start MongoDB** (if not running)
   ```bash
   mongod
   ```

2. **Start Server**
   ```bash
   npm start
   ```

3. **Open Browser**
   - Go to: `http://localhost:3000/login.html`
   - Login with test account or register new user

## 📁 New Files You Should Know

| File | Purpose |
|------|---------|
| `login.html` | Login page (email/password + Google) |
| `register.html` | User registration |
| `auth-check.js` | Protects all pages (auto-included) |
| `.env` | **⚠️ Change JWT_SECRET before production!** |

## 🔐 How It Works

### For Users:
```
1. Visit any page → Redirects to login (if not authenticated)
2. Login/Register → Get JWT token
3. Token stored → Access all pages
4. Click Logout → Token removed, back to login
```

### For Developers:
```javascript
// All pages are now protected automatically!
// auth-check.js runs on every page load

// Making authenticated requests:
authenticatedFetch('/api/members')
  .then(res => res.json())
  .then(data => console.log(data));

// Getting current user:
const user = JSON.parse(localStorage.getItem('user'));
```

## 🎨 UI Changes

**Every page now has:**
```
┌─────────────────────────────────────────┐
│  ☰ Page Title     [User Name] [Logout] │ ← New user menu
└─────────────────────────────────────────┘
```

## 🛡️ Security Features

✅ Passwords hashed (bcrypt)
✅ JWT tokens (7-day expiration)
✅ Protected API routes
✅ Role-based access (Member/Coach/Admin)
✅ Automatic token verification

## 📊 Available Roles

| Role | Permissions |
|------|-------------|
| **Member** | View own data, dashboard access |
| **Coach** | + Manage schedules, view members |
| **Admin** | + Full system access, manage users |

## 🔧 Common Tasks

### Register New User
```bash
POST http://localhost:3000/api/auth/register
{
  "name": "John Doe",
  "email": "john@gym.com",
  "password": "password123",
  "role": "member"
}
```

### Login
```bash
POST http://localhost:3000/api/auth/login
{
  "email": "john@gym.com",
  "password": "password123"
}
```

### Logout
Just click the **Logout** button in any page header!

## 🐛 Troubleshooting

| Problem | Solution |
|---------|----------|
| Can't connect to MongoDB | Run `mongod` or check Windows Services |
| Google Sign-In not working | Need to set up Google OAuth (see SETUP_GUIDE.md) |
| Token expired | Normal after 7 days - just login again |
| Forgot password | Register new account (reset feature not yet implemented) |

## ⚙️ Important Configuration

### Before Production:
1. **Change JWT Secret** in `.env`:
   ```env
   JWT_SECRET=your_long_random_secret_here
   ```
   Generate one: 
   ```bash
   node -e "console.log(require('crypto').randomBytes(32).toString('hex'))"
   ```

2. **Enable HTTPS**
3. **Use MongoDB Atlas** (not local MongoDB)
4. **Set up Google OAuth** (optional)

## 📖 Full Documentation

- **SETUP_GUIDE.md** - Complete setup instructions
- **README_LOGIN.md** - API docs and detailed features
- **AUTHENTICATION_SUMMARY.md** - Test results and overview

## 🎉 What Works Right Now

✅ Email/Password Login
✅ User Registration
✅ JWT Token Authentication
✅ Protected Pages (all redirect to login)
✅ User Menu & Logout Button
✅ Role-Based Access Control
✅ Remember Me Functionality
✅ Google OAuth Ready (needs setup)

## 💡 Tips

1. **Local Development**: Everything works out of the box!
2. **Google Sign-In**: Optional - requires Google Cloud setup
3. **Test Account**: Use `testuser@gym.com` / `test123456` to test
4. **Protected Pages**: Try accessing dashboard without login - auto redirects!
5. **Token Storage**: Check browser localStorage to see token

## 🚀 Next Actions

**Immediate:**
- ✅ Test login/logout flow
- ✅ Create different user roles (member, coach, admin)
- ✅ Access all pages while logged in

**Optional:**
- [ ] Set up Google OAuth (SETUP_GUIDE.md)
- [ ] Customize login page design
- [ ] Add password reset feature
- [ ] Add email verification

## 📞 Need Help?

1. Server not starting? Check MongoDB is running
2. Google OAuth? See SETUP_GUIDE.md section "Setting Up Google Sign-In"
3. API questions? Check README_LOGIN.md
4. Something broken? Check server logs in terminal

---

**You're all set! 🎊**

Start the server with `npm start` and visit:
👉 **http://localhost:3000/login.html**
