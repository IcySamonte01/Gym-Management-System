// Script to fix the duplicate googleId index issue
// Run this with: node fix-duplicate-index.js

const { MongoClient } = require('mongodb');

const MONGODB_URI = 'mongodb://localhost:27017';
const DATABASE_NAME = 'gym_management';

async function fixDuplicateIndex() {
    const client = new MongoClient(MONGODB_URI);
    
    try {
        await client.connect();
        console.log('✅ Connected to MongoDB');
        
        const db = client.db(DATABASE_NAME);
        const users = db.collection('users');
        
        // List all indexes
        console.log('\n📋 Current indexes:');
        const indexes = await users.indexes();
        indexes.forEach(index => {
            console.log(`  - ${index.name}: ${JSON.stringify(index.key)}`);
        });
        
        // Drop the problematic googleId index
        try {
            console.log('\n🔧 Dropping googleId_1 index...');
            await users.dropIndex('googleId_1');
            console.log('✅ Dropped googleId_1 index');
        } catch (error) {
            if (error.code === 27) {
                console.log('ℹ️  Index googleId_1 does not exist (already fixed)');
            } else {
                throw error;
            }
        }
        
        // Create a sparse index for googleId (allows multiple nulls)
        console.log('\n🔧 Creating sparse index for googleId...');
        await users.createIndex(
            { googleId: 1 }, 
            { unique: true, sparse: true, name: 'googleId_1_sparse' }
        );
        console.log('✅ Created sparse index for googleId');
        
        // List indexes again
        console.log('\n📋 Updated indexes:');
        const newIndexes = await users.indexes();
        newIndexes.forEach(index => {
            console.log(`  - ${index.name}: ${JSON.stringify(index.key)}`);
        });
        
        console.log('\n✅ Index fix complete!');
        console.log('You can now update users without googleId conflicts.');
        
    } catch (error) {
        console.error('❌ Error:', error.message);
    } finally {
        await client.close();
    }
}

fixDuplicateIndex();
