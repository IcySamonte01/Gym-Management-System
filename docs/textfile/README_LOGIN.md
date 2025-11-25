# Login Module Documentation

## Overview
This gym management system now includes a complete authentication system with both traditional email/password login and Google OAuth integration.

## Features

### 1. **Traditional Login**
- Email and password authentication
- Password hashing with bcryptjs
- JWT token-based authentication
- Remember me functionality
- Session management

### 2. **Google OAuth Login**
- Sign in with Google account
- Automatic user creation/linking
- Profile picture integration
- One-click authentication

### 3. **User Registration**
- New user signup
- Email validation
- Password strength requirements
- Role-based account types (Member, Coach, Admin)

### 4. **Security Features**
- Password hashing with bcrypt
- JWT token authentication
- Token expiration (7 days default)
- Protected routes with middleware
- Role-based access control

## Setup Instructions

### 1. Install Dependencies
```bash
npm install
```

### 2. Configure Environment Variables
Edit the `.env` file with your configuration:

```env
# MongoDB Configuration
MONGODB_URI=mongodb://localhost:27017/gym_management

# JWT Configuration
JWT_SECRET=your_super_secret_jwt_key_change_this_in_production
JWT_EXPIRE=7d

# Google OAuth Configuration
GOOGLE_CLIENT_ID=your_google_client_id_here
GOOGLE_CLIENT_SECRET=your_google_client_secret_here
GOOGLE_CALLBACK_URL=http://localhost:3000/api/auth/google/callback

# Server Configuration
PORT=3000
NODE_ENV=development
```

### 3. Set Up Google OAuth (Optional)

To enable Google Sign-In:

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing one
3. Enable Google+ API
4. Go to "Credentials" → "Create Credentials" → "OAuth 2.0 Client ID"
5. Configure OAuth consent screen
6. Add authorized redirect URIs:
   - `http://localhost:3000/api/auth/google/callback`
   - `http://localhost:3000` (for development)
7. Copy Client ID and Client Secret to `.env` file
8. Update `login.html` with your Client ID:
   ```html
   <div id="g_id_onload"
        data-client_id="YOUR_GOOGLE_CLIENT_ID"
        ...>
   </div>
   ```

### 4. Start MongoDB
Make sure MongoDB is running:
```bash
# If using MongoDB locally
mongod
```

### 5. Start the Server
```bash
npm start
```

The application will be available at `http://localhost:3000`

## File Structure

```
├── login.html              # Login page
├── register.html           # Registration page
├── login-style.css         # Login/Register page styles
├── login.js               # Login page JavaScript
├── register.js            # Registration page JavaScript
├── auth-check.js          # Authentication middleware for frontend
├── server.js              # Updated with authentication routes
├── .env                   # Environment variables
└── README_LOGIN.md        # This file
```

## API Endpoints

### Authentication Routes

#### Register New User
```
POST /api/auth/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "securePassword123",
  "role": "member"
}
```

#### Login
```
POST /api/auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "securePassword123",
  "rememberMe": true
}
```

Response:
```json
{
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "...",
    "name": "John Doe",
    "email": "john@example.com",
    "role": "member"
  }
}
```

#### Verify Token
```
GET /api/auth/verify
Authorization: Bearer <token>
```

#### Get User Profile
```
GET /api/auth/profile
Authorization: Bearer <token>
```

#### Logout
```
POST /api/auth/logout
Authorization: Bearer <token>
```

#### Google OAuth
```
GET /api/auth/google
```
Redirects to Google authentication

```
GET /api/auth/google/callback
```
Google OAuth callback URL

## Usage

### For Users

1. **First Time Users:**
   - Go to `/register.html`
   - Fill in your details
   - Choose account type (Member/Coach/Admin)
   - Click "Create Account"
   - You'll be redirected to login page

2. **Existing Users:**
   - Go to `/login.html`
   - Enter email and password
   - Or click "Sign in with Google"
   - Check "Remember me" to stay logged in

3. **Accessing Protected Pages:**
   - All pages except login and register require authentication
   - Token is automatically sent with requests
   - Invalid/expired tokens redirect to login

### For Developers

#### Protecting Routes in Backend
```javascript
// Require authentication
app.get('/api/protected-route', authenticateToken, (req, res) => {
  // req.user contains user info from token
  res.json({ message: 'Protected data' });
});

// Require specific role
app.get('/api/admin-only', 
  authenticateToken, 
  authorizeRole('admin'), 
  (req, res) => {
    res.json({ message: 'Admin data' });
  }
);
```

#### Making Authenticated Requests from Frontend
```javascript
// Use the authenticatedFetch helper function
authenticatedFetch('/api/members')
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error(error));
```

#### Getting Current User Info
```javascript
const user = JSON.parse(localStorage.getItem('user'));
console.log(user.name, user.role);
```

## User Roles

- **Member**: Regular gym members, can view their own data
- **Coach**: Gym trainers, can manage schedules and classes
- **Admin**: Full access to all features and data

## Security Notes

⚠️ **Important Security Considerations:**

1. **Change Default JWT Secret**: Update `JWT_SECRET` in `.env` with a strong, random string
2. **Use HTTPS in Production**: Set `NODE_ENV=production` and use SSL certificates
3. **Secure Cookie Settings**: Enable `secure: true` for cookies in production
4. **Rate Limiting**: Consider adding rate limiting to prevent brute force attacks
5. **Password Policy**: Enforce strong password requirements
6. **Keep Dependencies Updated**: Regularly update npm packages
7. **Environment Variables**: Never commit `.env` file to version control

## Troubleshooting

### Google Sign-In Not Working
- Check if Client ID is correctly set in both `.env` and `login.html`
- Verify redirect URIs in Google Console match your application URL
- Check browser console for errors
- Ensure cookies are enabled

### Token Expired Error
- Tokens expire after 7 days by default
- User will be automatically redirected to login
- Adjust `JWT_EXPIRE` in `.env` to change expiration time

### Cannot Connect to MongoDB
- Ensure MongoDB is running
- Check connection string in `.env`
- Verify MongoDB port (default: 27017)

### User Already Exists Error
- Email addresses must be unique
- User may have registered with Google previously
- Try password reset or use Google Sign-In

## Testing

### Create Test Users

You can create test users via the registration page or using a tool like Postman:

```bash
# Admin user
curl -X POST http://localhost:3000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Admin User",
    "email": "admin@gym.com",
    "password": "admin123",
    "role": "admin"
  }'

# Member user
curl -X POST http://localhost:3000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Member",
    "email": "member@gym.com",
    "password": "member123",
    "role": "member"
  }'
```

## Future Enhancements

- Password reset via email
- Two-factor authentication (2FA)
- Social login (Facebook, GitHub)
- Account lockout after failed attempts
- Email verification
- User profile management
- Change password functionality

## Support

For issues or questions, please check:
1. This documentation
2. Server logs for errors
3. Browser console for frontend errors
4. MongoDB logs for database issues
