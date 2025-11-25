// Members Management JavaScript
let allMembers = [];
let currentUser = null;

// Initialize page
document.addEventListener('DOMContentLoaded', async function() {
    // Get current user info
    const userStr = localStorage.getItem('user');
    if (userStr) {
        currentUser = JSON.parse(userStr);
        
        // Show/hide admin-only elements
        if (currentUser.role === 'admin') {
            document.querySelectorAll('.admin-only').forEach(el => {
                el.style.display = '';
            });
            document.getElementById('addMemberBtn').style.display = 'inline-flex';
            document.querySelectorAll('.nav-item-admin').forEach(el => {
                el.style.display = '';
            });
        } else {
            // Hide admin menu items for members and coaches
            document.querySelectorAll('.nav-item-admin').forEach(el => {
                el.style.display = 'none';
            });
        }
    }

    // Load members
    await loadMembers();

    // Setup modal
    setupModal();

    // Setup search
    setupSearch();
});

// Load all members from API
async function loadMembers() {
    try {
        const response = await authenticatedFetch('/api/members');
        
        if (!response.ok) {
            throw new Error('Failed to load members');
        }

        allMembers = await response.json();
        displayMembers(allMembers);
        updateStats();
    } catch (error) {
        console.error('Error loading members:', error);
        showMessage('Failed to load members: ' + error.message, 'error');
        document.getElementById('membersTableBody').innerHTML = `
            <tr>
                <td colspan="8" style="text-align: center; padding: 40px; color: #f44336;">
                    <i class="ph ph-warning" style="font-size: 32px;"></i>
                    <p>Failed to load members. Please try again.</p>
                </td>
            </tr>
        `;
    }
}

// Display members in table
function displayMembers(members) {
    const tbody = document.getElementById('membersTableBody');
    
    if (members.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" style="text-align: center; padding: 40px;">
                    <i class="ph ph-users" style="font-size: 32px; color: #999;"></i>
                    <p>No members found</p>
                </td>
            </tr>
        `;
        return;
    }

    tbody.innerHTML = members.map((member, index) => `
        <tr>
            <td>${index + 1}</td>
            <td>${escapeHtml(member.name)}</td>
            <td>${escapeHtml(member.email)}</td>
            <td>${escapeHtml(member.phone)}</td>
            <td><span class="badge badge-${member.membershipType?.toLowerCase()}">${escapeHtml(member.membershipType || 'N/A')}</span></td>
            <td><span class="status-badge status-${member.status}">${escapeHtml(member.status || 'active')}</span></td>
            <td>${formatDate(member.joinDate)}</td>
            <td class="actions admin-only" style="${currentUser?.role === 'admin' ? '' : 'display: none;'}">
                <button class="btn-icon btn-edit" onclick="editMember('${member.id}')" title="Edit">
                    <i class="ph ph-pencil"></i>
                </button>
                <button class="btn-icon btn-delete" onclick="deleteMember('${member.id}', '${escapeHtml(member.name)}')" title="Delete">
                    <i class="ph ph-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');
}

// Update statistics
function updateStats() {
    const totalMembers = allMembers.length;
    const activeMembers = allMembers.filter(m => m.status === 'active').length;
    
    document.getElementById('totalMembers').textContent = totalMembers;
    document.getElementById('activeMembers').textContent = activeMembers;
}

// Search members
function setupSearch() {
    const searchInput = document.getElementById('searchInput');
    
    searchInput.addEventListener('input', function() {
        searchMembers();
    });

    searchInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            searchMembers();
        }
    });
}

function searchMembers() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase().trim();
    
    if (!searchTerm) {
        displayMembers(allMembers);
        return;
    }

    const filtered = allMembers.filter(member => {
        return member.name.toLowerCase().includes(searchTerm) ||
               member.email.toLowerCase().includes(searchTerm) ||
               member.phone.toLowerCase().includes(searchTerm) ||
               (member.membershipType && member.membershipType.toLowerCase().includes(searchTerm));
    });

    displayMembers(filtered);
}

// Modal functions
function setupModal() {
    const modal = document.getElementById('memberModal');
    const addBtn = document.getElementById('addMemberBtn');
    const closeBtn = document.querySelector('.close');
    const form = document.getElementById('memberForm');

    // Open modal for adding
    if (addBtn) {
        addBtn.onclick = function() {
            openMemberModal();
        };
    }

    // Close modal
    closeBtn.onclick = function() {
        closeMemberModal();
    };

    // Close on outside click
    window.onclick = function(event) {
        if (event.target == modal) {
            closeMemberModal();
        }
    };

    // Handle form submission
    form.onsubmit = async function(e) {
        e.preventDefault();
        await saveMember();
    };
}

function openMemberModal(memberId = null) {
    const modal = document.getElementById('memberModal');
    const form = document.getElementById('memberForm');
    const title = document.getElementById('modalTitle');
    const submitBtn = document.getElementById('submitBtnText');

    // Reset form
    form.reset();
    document.getElementById('memberId').value = '';

    if (memberId) {
        // Edit mode
        const member = allMembers.find(m => m.id === memberId);
        if (member) {
            title.textContent = 'Edit Member';
            submitBtn.textContent = 'Update Member';
            
            document.getElementById('memberId').value = member.id;
            document.getElementById('memberName').value = member.name;
            document.getElementById('memberEmail').value = member.email;
            document.getElementById('memberPassword').value = ''; // Don't show existing password
            document.getElementById('memberPassword').placeholder = 'Leave blank to keep current password';
            document.getElementById('memberPassword').required = false; // Not required for edit
            document.getElementById('memberPhone').value = member.phone;
            document.getElementById('membershipType').value = member.membershipType;
            document.getElementById('memberAddress').value = member.address || '';
            document.getElementById('emergencyContact').value = member.emergencyContact || '';
            document.getElementById('memberStatus').value = member.status || 'active';
        }
    } else {
        // Add mode
        title.textContent = 'Add New Member';
        submitBtn.textContent = 'Add Member';
        document.getElementById('memberStatus').value = 'active';
        document.getElementById('memberPassword').required = true;
        document.getElementById('memberPassword').placeholder = 'Minimum 6 characters';
    }

    modal.style.display = 'block';
}

function closeMemberModal() {
    document.getElementById('memberModal').style.display = 'none';
}

// Save member (Add or Update)
async function saveMember() {
    const memberId = document.getElementById('memberId').value;
    const password = document.getElementById('memberPassword').value;
    
    const memberData = {
        name: document.getElementById('memberName').value.trim(),
        email: document.getElementById('memberEmail').value.trim(),
        phone: document.getElementById('memberPhone').value.trim(),
        membershipType: document.getElementById('membershipType').value,
        address: document.getElementById('memberAddress').value.trim(),
        emergencyContact: document.getElementById('emergencyContact').value.trim(),
        status: document.getElementById('memberStatus').value
    };

    // Add password only if provided (required for new, optional for edit)
    if (password) {
        memberData.password = password;
    }

    // Validation
    if (!memberData.name || !memberData.email || !memberData.phone || !memberData.membershipType) {
        showMessage('Please fill in all required fields', 'error');
        return;
    }

    // Password validation for new members
    if (!memberId && !password) {
        showMessage('Password is required for new members', 'error');
        return;
    }

    if (password && password.length < 6) {
        showMessage('Password must be at least 6 characters', 'error');
        return;
    }

    try {
        const url = memberId ? `/api/members/${memberId}` : '/api/members';
        const method = memberId ? 'PUT' : 'POST';

        const response = await authenticatedFetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(memberData)
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.error || 'Failed to save member');
        }

        const message = memberId ? 'Member updated successfully!' : 'Member added successfully!';
        showMessage(message, 'success');
        
        closeMemberModal();
        await loadMembers(); // Reload the list
    } catch (error) {
        console.error('Error saving member:', error);
        showMessage('Error: ' + error.message, 'error');
    }
}

// Edit member
function editMember(memberId) {
    if (currentUser?.role !== 'admin') {
        showMessage('Only administrators can edit members', 'error');
        return;
    }
    openMemberModal(memberId);
}

// Delete member
async function deleteMember(memberId, memberName) {
    if (currentUser?.role !== 'admin') {
        showMessage('Only administrators can delete members', 'error');
        return;
    }

    if (!confirm(`Are you sure you want to delete ${memberName}?\n\nThis action cannot be undone.`)) {
        return;
    }

    try {
        const response = await authenticatedFetch(`/api/members/${memberId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.error || 'Failed to delete member');
        }

        showMessage('Member deleted successfully!', 'success');
        await loadMembers(); // Reload the list
    } catch (error) {
        console.error('Error deleting member:', error);
        showMessage('Error: ' + error.message, 'error');
    }
}

// Utility functions
function formatDate(dateString) {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
}

function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

function showMessage(message, type) {
    // Create message element
    const messageDiv = document.createElement('div');
    messageDiv.className = `alert alert-${type}`;
    messageDiv.innerHTML = `
        <i class="ph ph-${type === 'success' ? 'check-circle' : 'warning-circle'}"></i>
        <span>${message}</span>
    `;

    // Insert at top of content
    const content = document.querySelector('.content');
    content.insertBefore(messageDiv, content.firstChild);

    // Auto remove after 5 seconds
    setTimeout(() => {
        messageDiv.remove();
    }, 5000);
}
