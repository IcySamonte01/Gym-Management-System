// Script to create or promote admin users
require('dotenv').config();
const mongoose = require('mongoose');
const bcrypt = require('bcryptjs');
const readline = require('readline');

const MONGODB_URI = process.env.MONGODB_URI || 'mongodb://localhost:27017/gym_management';

const userSchema = new mongoose.Schema({
  name: String,
  email: String,
  password: String,
  role: String,
  authProvider: String,
  isActive: Boolean,
  lastLogin: Date
}, { timestamps: true });

// Hash password before saving
userSchema.pre('save', async function(next) {
  if (!this.isModified('password') || !this.password) return next();
  
  try {
    const salt = await bcrypt.genSalt(10);
    this.password = await bcrypt.hash(this.password, salt);
    next();
  } catch (error) {
    next(error);
  }
});

const User = mongoose.model('User', userSchema);

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

function question(query) {
  return new Promise(resolve => rl.question(query, resolve));
}

async function promoteToAdmin() {
  try {
    await mongoose.connect(MONGODB_URI);
    console.log('✅ Connected to MongoDB\n');

    // Get email to promote
    const email = await question('Enter email address to promote to admin: ');
    
    const user = await User.findOne({ email: email.trim() });
    
    if (!user) {
      console.log('❌ User not found with email:', email);
      process.exit(1);
    }

    if (user.role === 'admin') {
      console.log('ℹ️  User is already an admin!');
      console.log('Name:', user.name);
      console.log('Email:', user.email);
      process.exit(0);
    }

    user.role = 'admin';
    await user.save();

    console.log('\n✅ User promoted to admin successfully!');
    console.log('Name:', user.name);
    console.log('Email:', user.email);
    console.log('Role:', user.role);
    
    process.exit(0);
  } catch (error) {
    console.error('❌ Error:', error.message);
    process.exit(1);
  }
}

async function createNewAdmin() {
  try {
    await mongoose.connect(MONGODB_URI);
    console.log('✅ Connected to MongoDB\n');

    const name = await question('Enter admin name: ');
    const email = await question('Enter admin email: ');
    const password = await question('Enter admin password (min 6 characters): ');

    if (password.length < 6) {
      console.log('❌ Password must be at least 6 characters long');
      process.exit(1);
    }

    // Check if user already exists
    const existingUser = await User.findOne({ email: email.trim() });
    if (existingUser) {
      console.log('❌ User with this email already exists!');
      const promote = await question('Do you want to promote this user to admin? (yes/no): ');
      
      if (promote.toLowerCase() === 'yes' || promote.toLowerCase() === 'y') {
        existingUser.role = 'admin';
        await existingUser.save();
        console.log('\n✅ User promoted to admin!');
        process.exit(0);
      }
      
      process.exit(1);
    }

    // Create new admin
    const admin = await User.create({
      name: name.trim(),
      email: email.trim(),
      password: password,
      role: 'admin',
      authProvider: 'local',
      isActive: true
    });

    console.log('\n✅ Admin created successfully!');
    console.log('Name:', admin.name);
    console.log('Email:', admin.email);
    console.log('Role:', admin.role);
    console.log('\n⚠️  Keep these credentials safe!');
    
    process.exit(0);
  } catch (error) {
    console.error('❌ Error:', error.message);
    process.exit(1);
  }
}

async function main() {
  console.log('╔════════════════════════════════════════╗');
  console.log('║     Admin Account Management Tool     ║');
  console.log('╚════════════════════════════════════════╝\n');
  
  console.log('1. Create new admin account');
  console.log('2. Promote existing user to admin');
  console.log('3. Exit\n');
  
  const choice = await question('Choose an option (1-3): ');
  
  switch(choice.trim()) {
    case '1':
      await createNewAdmin();
      break;
    case '2':
      await promoteToAdmin();
      break;
    case '3':
      console.log('Goodbye!');
      process.exit(0);
      break;
    default:
      console.log('Invalid option');
      process.exit(1);
  }
}

main();
