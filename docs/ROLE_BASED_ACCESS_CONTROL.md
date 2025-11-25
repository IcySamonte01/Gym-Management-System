# 🔐 Role-Based Access Control (RBAC)

## ✅ Implementation Complete!

Role-based access control has been implemented for Members and Coaches management.

---

## 🎯 Access Control Rules

### **Admin Role**
✅ **Can view** all members and coaches  
✅ **Can add** new members and coaches  
✅ **Can edit** member and coach information  
✅ **Can delete** members and coaches  
✅ **Can search** through members and coaches  

### **Coach Role**
✅ **Can view** all members and coaches  
❌ **Cannot add** members or coaches  
❌ **Cannot edit** member or coach information  
❌ **Cannot delete** members or coaches  
✅ **Can search** through members and coaches  

### **Member Role**
✅ **Can view** all members and coaches  
❌ **Cannot add** members or coaches  
❌ **Cannot edit** member or coach information  
❌ **Cannot delete** members or coaches  
✅ **Can search** through members and coaches  

---

## 🔒 Backend Security

### **Members Controller (`/api/members`)**

| Endpoint | Method | Required Role | Description |
|----------|--------|---------------|-------------|
| `/api/members` | GET | Authenticated | View all members |
| `/api/members/:id` | GET | Authenticated | View member details |
| `/api/members` | POST | **Admin Only** | Add new member |
| `/api/members/:id` | PUT | **Admin Only** | Update member |
| `/api/members/:id` | DELETE | **Admin Only** | Delete member |

### **Coaches Controller (`/api/coaches`)**

| Endpoint | Method | Required Role | Description |
|----------|--------|---------------|-------------|
| `/api/coaches` | GET | Authenticated | View all coaches |
| `/api/coaches/:id` | GET | Authenticated | View coach details |
| `/api/coaches` | POST | **Admin Only** | Add new coach |
| `/api/coaches/:id` | PUT | **Admin Only** | Update coach |
| `/api/coaches/:id` | DELETE | **Admin Only** | Delete coach |

### **Implementation:**

```csharp
[Authorize] // All users must be authenticated
public class MembersController : ControllerBase
{
    [HttpGet] // Anyone can view
    public async Task<IActionResult> GetAll() { }
    
    [HttpPost]
    [Authorize(Roles = "admin")] // Only admin can create
    public async Task<IActionResult> Create() { }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")] // Only admin can update
    public async Task<IActionResult> Update() { }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")] // Only admin can delete
    public async Task<IActionResult> Delete() { }
}
```

---

## 🎨 Frontend UI Control

### **Members Page (`/members.html`)**

**For Admin Users:**
- ✅ "Add Member" button is **visible**
- ✅ "Edit" button is **visible** for each member
- ✅ "Delete" button is **visible** for each member
- ✅ Can fill and submit the member form

**For Coach/Member Users:**
- ❌ "Add Member" button is **hidden**
- ❌ "Edit" button is **hidden** for each member
- ❌ "Delete" button is **hidden** for each member
- ❌ Actions column is **hidden**
- ✅ Can **view** all members
- ✅ Can **search** members

### **Implementation:**

```javascript
// Check user role on page load
const currentUser = JSON.parse(localStorage.getItem('user'));

if (currentUser.role === 'admin') {
    // Show admin controls
    document.getElementById('addMemberBtn').style.display = 'inline-flex';
    document.querySelectorAll('.admin-only').forEach(el => {
        el.style.display = '';
    });
} else {
    // Hide admin controls
    document.getElementById('addMemberBtn').style.display = 'none';
    document.querySelectorAll('.admin-only').forEach(el => {
        el.style.display = 'none';
    });
}
```

---

## 📊 Members Module Features

### **✅ Add Member (Admin Only)**

1. Click "Add Member" button
2. Fill in the modal form:
   - Full Name (required)
   - Email (required)
   - Phone Number (required)
   - Membership Type (required)
   - Address (optional)
   - Emergency Contact (optional)
   - Status (active/inactive)
3. Click "Add Member"
4. Member is added to database
5. Table refreshes automatically
6. Member count updates

### **✅ Edit Member (Admin Only)**

1. Click "Edit" button on member row
2. Modal opens with pre-filled data
3. Modify the information
4. Click "Update Member"
5. Member is updated in database
6. Table refreshes automatically

### **✅ Delete Member (Admin Only)**

1. Click "Delete" button on member row
2. Confirmation dialog appears
3. Confirm deletion
4. Member is removed from database
5. Table refreshes automatically
6. Member count updates

### **✅ Search Members (All Users)**

1. Type in search box
2. Search works in real-time
3. Filters by:
   - Name
   - Email
   - Phone
   - Membership Type
4. Results update instantly

### **✅ View Statistics (All Users)**

- **Total Members**: Shows count of all members
- **Active Members**: Shows count of active members only
- Both stats update automatically when members are added/removed

---

## 🔢 Member Count Integration

### **Dashboard Display**

The member count is displayed on:
1. **Members Page** - "Total Members" stat card
2. **Dashboard Page** - "Total Members" stat card

### **Real-Time Updates**

Member count updates automatically when:
- ✅ New member is added
- ✅ Member is deleted
- ✅ Page is refreshed
- ✅ User navigates to members page

### **API Endpoint:**

```
GET /api/dashboard/stats

Response:
{
    "totalMembers": 25,
    "activeMembers": 23,
    "totalCoaches": 5,
    "totalRevenue": 12500.00
}
```

---

## 🛡️ Security Layers

### **Layer 1: Frontend UI**
- Admin-only buttons hidden for non-admin users
- Provides good UX

### **Layer 2: Frontend Logic**
- Role checks before API calls
- Prevents unnecessary requests

### **Layer 3: Backend Authorization**
- `[Authorize]` attribute requires authentication
- `[Authorize(Roles = "admin")]` restricts to admin
- Returns 403 Forbidden if role mismatch

### **Layer 4: Database**
- All operations require valid JWT token
- Token contains user role information
- Cannot be tampered with

---

## 🧪 Testing Role-Based Access

### **Test as Admin:**

1. Login as: `admin@gym.com` / `admin123456`
2. Navigate to Members page
3. Verify you can:
   - ✅ See "Add Member" button
   - ✅ Click "Add Member" and add a new member
   - ✅ See "Edit" buttons on each row
   - ✅ Edit a member
   - ✅ See "Delete" buttons on each row
   - ✅ Delete a member
   - ✅ Search members

### **Test as Coach/Member:**

1. Register as coach or member
2. Login with your credentials
3. Navigate to Members page
4. Verify you can:
   - ✅ View all members
   - ✅ Search members
   - ❌ See "Add Member" button (should be hidden)
   - ❌ See "Edit" buttons (should be hidden)
   - ❌ See "Delete" buttons (should be hidden)
   - ❌ See Actions column (should be hidden)

### **Test Backend Security:**

Try making API calls as non-admin:

```javascript
// This should fail with 403 Forbidden
fetch('/api/members', {
    method: 'POST',
    headers: {
        'Authorization': 'Bearer ' + nonAdminToken,
        'Content-Type': 'application/json'
    },
    body: JSON.stringify({ name: 'Test', email: 'test@test.com', ... })
})
// Expected: 403 Forbidden response
```

---

## 📝 Code Files Modified

### **Backend (C#):**
- ✅ `GymManagementAPI/Controllers/MembersController.cs` - Added `[Authorize]` attributes
- ✅ `GymManagementAPI/Controllers/CoachesController.cs` - Added `[Authorize]` attributes

### **Frontend (HTML):**
- ✅ `frontend/html/members.html` - Complete redesign with modal and role-based UI

### **Frontend (JavaScript):**
- ✅ `frontend/js/members.js` - New file with full CRUD functionality
- ✅ `frontend/js/dashboard.js` - New file to display member count

### **Frontend (CSS):**
- ✅ `frontend/css/members.css` - New file with complete styling

---

## 🎯 Features Summary

### **✅ Implemented:**

1. **Role-Based Access Control**
   - Admin can add/edit/delete
   - Coach/Member can only view
   
2. **Members CRUD Operations**
   - Create new members (admin)
   - Read/view all members (all)
   - Update member info (admin)
   - Delete members (admin)

3. **Search Functionality**
   - Real-time search
   - Multi-field search
   - Works for all users

4. **Member Statistics**
   - Total members count
   - Active members count
   - Displayed on dashboard
   - Updates in real-time

5. **Modal Dialog**
   - Clean UI for add/edit
   - Form validation
   - Responsive design

6. **Security**
   - JWT authentication required
   - Role-based authorization
   - Backend validation
   - Frontend UI control

---

## 🚀 Next Steps

### **To Apply Same Pattern to Coaches:**

The same implementation can be applied to the coaches page:
1. Create `frontend/js/coaches.js` (similar to members.js)
2. Create `frontend/css/coaches.css` (similar to members.css)
3. Update `frontend/html/coaches.html` (similar to members.html)
4. Backend is already secured with `[Authorize(Roles = "admin")]`

### **Additional Enhancements:**

- Add pagination for large member lists
- Add export functionality (CSV/PDF)
- Add bulk operations (delete multiple)
- Add member photos/avatars
- Add activity history per member
- Add email notifications

---

## 📚 Documentation

| Document | Description |
|----------|-------------|
| `ROLE_BASED_ACCESS_CONTROL.md` | This file - RBAC implementation |
| `FILE_ORGANIZATION_GUIDE.md` | Where files are located |
| `DEVELOPMENT_WORKFLOW.md` | How to add new features |

---

**Role-based access control is now fully implemented and functional! 🎉**

**Test it now:**
- Admin: http://localhost:3000/login.html (admin@gym.com)
- Members: http://localhost:3000/members.html
