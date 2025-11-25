// Dashboard JavaScript
document.addEventListener('DOMContentLoaded', async function() {
    // Get current user info
    const userStr = localStorage.getItem('user');
    if (userStr) {
        const currentUser = JSON.parse(userStr);
        
        // Show user info if elements exist
        const userNameEl = document.getElementById('userName');
        if (userNameEl) {
            userNameEl.textContent = currentUser.name || 'User';
        }

        // Hide admin menu items for non-admin users
        if (currentUser.role !== 'admin') {
            document.querySelectorAll('.nav-item-admin').forEach(el => {
                el.style.display = 'none';
            });
        }
    }

    // Load dashboard statistics
    await loadDashboardStats();
});

// Load dashboard statistics
async function loadDashboardStats() {
    try {
        // Get dashboard stats from API
        const response = await authenticatedFetch('/api/dashboard/stats');
        
        if (!response.ok) {
            throw new Error('Failed to load dashboard stats');
        }

        const stats = await response.json();
        
        // Update total members
        const totalMembersEl = document.getElementById('totalMembers');
        if (totalMembersEl) {
            totalMembersEl.textContent = stats.totalMembers || 0;
        }

        // Update active members
        const activeMembersEl = document.getElementById('activeMembers');
        if (activeMembersEl) {
            activeMembersEl.textContent = stats.activeMembers || 0;
        }

        // Update total coaches
        const totalCoachesEl = document.getElementById('totalCoaches');
        if (totalCoachesEl) {
            totalCoachesEl.textContent = stats.totalCoaches || 0;
        }

        // Update total revenue
        const totalRevenueEl = document.getElementById('totalRevenue');
        if (totalRevenueEl) {
            totalRevenueEl.textContent = '$' + (stats.totalRevenue || 0).toFixed(2);
        }

    } catch (error) {
        console.error('Error loading dashboard stats:', error);
        showError('Failed to load dashboard statistics');
    }
}

// Show error message
function showError(message) {
    const alertDiv = document.createElement('div');
    alertDiv.className = 'alert alert-error';
    alertDiv.innerHTML = `
        <i class="ph ph-warning-circle"></i>
        <span>${message}</span>
    `;
    
    const content = document.querySelector('.content');
    if (content) {
        content.insertBefore(alertDiv, content.firstChild);
        
        // Auto remove after 5 seconds
        setTimeout(() => {
            alertDiv.remove();
        }, 5000);
    }
}

// Refresh dashboard data
function refreshDashboard() {
    loadDashboardStats();
}
