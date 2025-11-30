# 🏋️ Schedule Join/Leave Feature for Members

## Overview
Members can now join and leave scheduled classes directly from the schedules table and calendar views, making it easy to manage their class enrollments.

---

## 🎯 Purpose

**Problem**: Members needed a way to enroll in gym classes and manage their attendance.

**Solution**: Added Join/Leave buttons in the schedules table and calendar views that allow members to:
- Enroll in upcoming classes
- Leave classes they've enrolled in
- See class capacity and availability
- View their attendance history

---

## 👥 User Access Levels

### **Members** 👤
- Can **Join** upcoming and ongoing classes
- Can **Leave** classes they've enrolled in
- See "Class Full" indicator when capacity is reached
- View "Attended" badge on completed classes they attended
- Cannot join completed classes

### **Coaches** 🏃
- View all schedules (no Join/Leave buttons)
- Can create schedules
- Can see enrollment numbers

### **Admins** 🔧
- See **Edit** and **Delete** buttons instead of Join/Leave
- Full schedule management
- Can view enrollment data

---

## ✨ Features

### 1. **Join Class Button** 🟢
- Appears on **Current & Upcoming** schedules
- Only for members
- Shows when:
  - Class is upcoming or ongoing
  - Class is not full
  - Member is not already enrolled

### 2. **Leave Class Button** 🔴
- Appears when member is enrolled
- Confirmation prompt before leaving
- Updates enrollment count immediately

### 3. **Class Full Indicator** 🚫
- Shows when capacity is reached
- Button is disabled
- Prevents enrollment

### 4. **Attended Badge** ✅
- Appears on **Completed Schedules**
- Shows for members who attended
- Green checkmark indicator

---

## 📍 Where Join/Leave Appears

### **Table View** (List View)
- **Current & Upcoming Schedules Table**:
  - Join button (green) for available classes
  - Leave button (red) for enrolled classes
  - Full indicator (gray) for capacity-reached classes

- **Completed Schedules Table**:
  - "Attended" badge for members who were enrolled
  - No action buttons (classes are completed)

### **Calendar View** (Day Timeline)
- Click any day to see detailed schedule
- Join/Leave buttons in each class card
- Shows enrollment count: "5/10 enrolled"

---

## 🎨 Button Design

### Join Button
```
┌────────────────┐
│ 🚪 Join        │  Green background (#4CAF50)
└────────────────┘
```

### Leave Button
```
┌────────────────┐
│ 🚪 Leave       │  Red background (from btn-delete)
└────────────────┘
```

### Full Indicator
```
┌────────────────┐
│ 👥 Full        │  Gray background (#95a5a6), disabled
└────────────────┘
```

### Attended Badge
```
✓ Attended        Green text (#4CAF50)
```

---

## 🔄 User Flow

### Joining a Class:

1. Member logs in
2. Navigates to **Schedules** page
3. Switches to **Table View** or views **Calendar**
4. Finds a class to join
5. Clicks **"Join"** button
6. ✅ Success message: "Successfully enrolled in class!"
7. Button changes to **"Leave"**
8. Enrollment count updates

### Leaving a Class:

1. Member finds a class they're enrolled in
2. Clicks **"Leave"** button
3. Confirmation: "Are you sure you want to leave this class?"
4. Confirms
5. ✅ Success message: "Successfully left the class"
6. Button changes to **"Join"** (if not full)
7. Enrollment count updates

---

## 🔒 Business Rules

### Enrollment Rules:
- ✅ Can join: Upcoming or ongoing classes with available spots
- ❌ Cannot join: Completed classes
- ❌ Cannot join: Full classes (at capacity)
- ❌ Cannot join: Already enrolled classes
- ✅ Can leave: Any class you're enrolled in (even if started)

### Capacity Rules:
- If capacity is set (e.g., 10), enrollment is limited
- Once `enrolledMembers.length >= capacity`, "Join" button becomes "Full"
- If capacity is not set, unlimited enrollment

### Status-Based Display:
| Status | Member Can Join? | Member Can Leave? |
|--------|------------------|-------------------|
| Upcoming | ✅ Yes | ✅ Yes (if enrolled) |
| Ongoing | ✅ Yes | ✅ Yes (if enrolled) |
| Completed | ❌ No | ❌ No (shows "Attended" if was enrolled) |

---

## 📊 Enrollment Display

### In Table View:
```
| Class Name | Coach | Date | Time | Duration | Capacity | Status | Actions |
|------------|-------|------|------|----------|----------|--------|---------|
| Yoga 101   | Lisa  | Jan 15 | 9:00 AM | 1h | 5/10 | upcoming | [Join] |
| HIIT       | John  | Jan 15 | 10:00 AM | 1h | 10/10 | upcoming | [Full] |
| Spinning   | Mike  | Jan 15 | 2:00 PM | 1h | 3/8 | upcoming | [Leave] |
```

### In Calendar Day View:
```
┌─────────────────────────────────┐
│ Yoga 101               upcoming │
├─────────────────────────────────┤
│ 👤 Lisa                         │
│ 🕐 9:00 AM - 10:00 AM           │
│ 👥 5/10 enrolled                │
│                                 │
│      [    Join Class    ]       │
└─────────────────────────────────┘
```

---

## 🛠️ Technical Implementation

### Files Modified:

1. **`frontend/js/schedules.js`**
   - Updated `renderSchedulesTable()` to add Join/Leave buttons for members
   - Modified current schedules table rendering
   - Modified completed schedules table rendering
   - Updated `enrollInClass()` to refresh table view
   - Updated `unenrollFromClass()` to refresh table view

### Key Changes:

#### Current Schedules Table:
- Check user role and enrollment status
- Show different buttons based on:
  - Admin: Edit/Delete buttons
  - Member (not enrolled, not full): Join button
  - Member (enrolled): Leave button
  - Member (not enrolled, full): Full indicator
  - Coach: No buttons

#### Completed Schedules Table:
- Admin: Edit/Delete buttons
- Member (was enrolled): "Attended" badge
- Others: Empty

### Backend API Endpoints Used:

```
POST /api/schedules/:id/enroll
- Body: { userId: string }
- Response: { message: string, schedule: object }

POST /api/schedules/:id/unenroll
- Body: { userId: string }
- Response: { message: string, schedule: object }
```

---

## 💡 Use Cases

### Use Case 1: Member Joins a Class
```
Scenario: Sarah wants to join a Yoga class
1. Sarah logs in as a member
2. Goes to Schedules → Table View
3. Finds "Yoga 101" with 5/10 capacity
4. Clicks "Join" button
5. Gets confirmation: "Successfully enrolled in class!"
6. Button changes to "Leave"
7. Capacity shows 6/10
8. Sarah can now attend the class
```

### Use Case 2: Member Leaves a Class
```
Scenario: Mike can't make it to a class he joined
1. Mike logs in
2. Goes to Schedules → Table View
3. Finds "HIIT Training" with "Leave" button (he's enrolled)
4. Clicks "Leave"
5. Confirms the action
6. Gets message: "Successfully left the class"
7. Button changes to "Join"
8. Capacity updates from 10/10 to 9/10
9. Spot opens for someone else
```

### Use Case 3: Class Full
```
Scenario: Emma tries to join a full class
1. Emma logs in
2. Goes to Schedules
3. Sees "Spinning" class with 15/15 capacity
4. Button shows "Full" (grayed out)
5. Cannot click the button
6. Emma can wait or join another class
```

### Use Case 4: Viewing Attendance History
```
Scenario: John checks his past attendance
1. John logs in
2. Goes to Schedules → Table View
3. Scrolls to "Completed Schedules" section
4. Sees classes with "✓ Attended" badge
5. Knows which classes he attended
```

---

## 🧪 Testing Checklist

### As Member:
- [ ] Log in as member
- [ ] Go to Schedules → Table View
- [ ] **Test Join:**
  - [ ] Find upcoming class with available spots
  - [ ] Click "Join"
  - [ ] Verify success message
  - [ ] Button changes to "Leave"
  - [ ] Capacity count increases
- [ ] **Test Leave:**
  - [ ] Click "Leave" on enrolled class
  - [ ] Confirm the action
  - [ ] Verify success message
  - [ ] Button changes to "Join" (if not full)
  - [ ] Capacity count decreases
- [ ] **Test Full Class:**
  - [ ] Find a class at capacity
  - [ ] Verify "Full" button is disabled
  - [ ] Cannot join
- [ ] **Test Completed Classes:**
  - [ ] Scroll to completed schedules
  - [ ] Verify "Attended" badge on enrolled classes
  - [ ] No action buttons visible

### As Admin:
- [ ] Log in as admin
- [ ] Go to Schedules → Table View
- [ ] Verify Edit/Delete buttons appear (not Join/Leave)
- [ ] Can edit schedules
- [ ] Can delete schedules

### As Coach:
- [ ] Log in as coach
- [ ] Go to Schedules
- [ ] Verify no Join/Leave buttons
- [ ] Can view all schedules
- [ ] Can create new schedules

---

## 🎯 Benefits

### For Members:
- ✅ Easy class enrollment
- ✅ Manage schedule directly
- ✅ See availability in real-time
- ✅ Track attendance history

### For Gym:
- ✅ Better class capacity management
- ✅ Accurate attendance tracking
- ✅ Reduced administrative overhead
- ✅ Better member engagement

### For Coaches:
- ✅ Know how many members to expect
- ✅ Plan class difficulty based on enrollment
- ✅ Better preparation

---

## 🔮 Future Enhancements

Possible improvements:

- [ ] Waitlist for full classes
- [ ] Automatic notifications before class starts
- [ ] Member attendance statistics dashboard
- [ ] Class cancellation if low enrollment
- [ ] Member can add class to personal calendar
- [ ] Points/rewards for consistent attendance
- [ ] Social features (see which friends are enrolled)

---

## 🐛 Known Limitations

- Members can leave classes even after they start (by design)
- No penalty system for late cancellations
- No waitlist for full classes
- No automated reminders

---

## 📊 Summary

| Aspect | Details |
|--------|---------|
| **Purpose** | Allow members to join/leave classes |
| **Users** | Members (primary), Admins (manage), Coaches (view) |
| **Views** | Table View, Calendar Day View |
| **Real-time** | Updates immediately on action |
| **Capacity** | Enforces class limits |
| **Status** | ✅ Fully Implemented |

---

**Last Updated**: January 2025  
**Feature Status**: ✅ Active and Working  
**User Role**: Members  
**Security Level**: 🔒 Secure (Users can only enroll themselves)
