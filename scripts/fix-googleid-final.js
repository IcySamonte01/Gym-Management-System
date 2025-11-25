// Final fix for googleId duplicate key issue
// Run this with: node fix-googleid-final.js

const { MongoClient } = require('mongodb');

const MONGODB_URI = 'mongodb://localhost:27017';
const DATABASE_NAME = 'gym_management';

async function fixGoogleIdIndex() {
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
            console.log(`  - ${index.name}: ${JSON.stringify(index.key)}, unique: ${index.unique || false}, sparse: ${index.sparse || false}`);
        });
        
        // Drop ALL googleId indexes
        console.log('\n🔧 Removing all googleId indexes...');
        try {
            await users.dropIndex('googleId_1');
            console.log('✅ Dropped googleId_1');
        } catch (error) {
            if (error.code === 27) {
                console.log('ℹ️  googleId_1 does not exist');
            }
        }
        
        try {
            await users.dropIndex('googleId_1_sparse');
            console.log('✅ Dropped googleId_1_sparse');
        } catch (error) {
            if (error.code === 27) {
                console.log('ℹ️  googleId_1_sparse does not exist');
            }
        }
        
        // Create a NON-UNIQUE index for googleId (for query performance only)
        console.log('\n🔧 Creating non-unique index for googleId...');
        await users.createIndex(
            { googleId: 1 }, 
            { name: 'googleId_1_nonunique', sparse: true, unique: false }
        );
        console.log('✅ Created non-unique sparse index for googleId');
        
        // Add a partial unique index only for non-null googleId values
        console.log('\n🔧 Creating partial unique index for non-null googleId...');
        await users.createIndex(
            { googleId: 1 },
            { 
                name: 'googleId_1_unique_notnull',
                unique: true,
                partialFilterExpression: { googleId: { $exists: true, $type: 'string' } }
            }
        );
        console.log('✅ Created partial unique index (only for non-null googleId)');
        
        // List indexes again
        console.log('\n📋 Updated indexes:');
        const newIndexes = await users.indexes();
        newIndexes.forEach(index => {
            console.log(`  - ${index.name}: ${JSON.stringify(index.key)}, unique: ${index.unique || false}, sparse: ${index.sparse || false}`);
        });
        
        console.log('\n✅ Final fix complete!');
        console.log('✅ Multiple users can have null googleId');
        console.log('✅ Google users with actual googleId still have uniqueness enforced');
        
    } catch (error) {
        console.error('❌ Error:', error.message);
        console.error(error);
    } finally {
        await client.close();
    }
}

fixGoogleIdIndex();
