// Create default admin account if it doesn't exist
require('dotenv').config();
const mongoose = require('mongoose');
const bcrypt = require('bcryptjs');

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

const User = mongoose.model('User', userSchema);

async function createDefaultAdmin() {
  try {
    await mongoose.connect(MONGODB_URI);
    console.log('✅ Connected to MongoDB\n');

    // Check if admin already exists
    const existingAdmin = await User.findOne({ email: 'admin@gym.com' });
    
    if (existingAdmin) {
      console.log('ℹ️  Default admin account already exists:');
      console.log('   Email: admin@gym.com');
      console.log('   Status: Active');
      process.exit(0);
    }

    // Create default admin
    const hashedPassword = await bcrypt.hash('admin123456', 10);
    
    const admin = await User.create({
      name: 'System Administrator',
      email: 'admin@gym.com',
      password: hashedPassword,
      role: 'admin',
      authProvider: 'local',
      isActive: true
    });

    console.log('✅ Default admin account created successfully!\n');
    console.log('╔═══════════════════════════════════════╗');
    console.log('║     DEFAULT ADMIN CREDENTIALS         ║');
    console.log('╠═══════════════════════════════════════╣');
    console.log('║  Email:    admin@gym.com              ║');
    console.log('║  Password: admin123456                ║');
    console.log('╚═══════════════════════════════════════╝\n');
    console.log('⚠️  IMPORTANT: Change this password after first login!\n');
    
    process.exit(0);
  } catch (error) {
    console.error('❌ Error:', error.message);
    process.exit(1);
  }
}

createDefaultAdmin();
