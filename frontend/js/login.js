// Login Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
  const loginForm = document.getElementById('loginForm');
  const messageDiv = document.getElementById('message');
  const emailInput = document.getElementById('email');
  const rememberMeCheckbox = document.getElementById('rememberMe');

  // Load saved email if "Remember me" was previously checked
  const savedEmail = localStorage.getItem('savedEmail');
  if (savedEmail) {
    emailInput.value = savedEmail;
    rememberMeCheckbox.checked = true;
  }

  // Handle traditional login
  loginForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const rememberMe = document.getElementById('rememberMe').checked;

    try {
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email, password, rememberMe })
      });

      const data = await response.json();

      if (response.ok) {
        // Store token
        if (rememberMe) {
          localStorage.setItem('token', data.token);
          // Save email for next login
          localStorage.setItem('savedEmail', email);
        } else {
          sessionStorage.setItem('token', data.token);
          // Clear saved email if "Remember me" is not checked
          localStorage.removeItem('savedEmail');
        }
        
        // Store user info
        localStorage.setItem('user', JSON.stringify(data.user));
        
        showMessage('Login successful! Redirecting...', 'success');
        
        // Redirect based on role
        setTimeout(() => {
          window.location.href = getDashboardUrl(data.user.role);
        }, 1000);
      } else {
        showMessage(data.error || 'Login failed. Please try again.', 'error');
      }
    } catch (error) {
      console.error('Login error:', error);
      showMessage('An error occurred. Please try again.', 'error');
    }
  });

  // Helper function to show messages
  function showMessage(message, type) {
    messageDiv.textContent = message;
    messageDiv.className = `message ${type}`;
    messageDiv.classList.remove('hidden');
    
    // Auto-hide after 5 seconds for non-success messages
    if (type !== 'success') {
      setTimeout(() => {
        messageDiv.classList.add('hidden');
      }, 5000);
    }
  }

  // Get dashboard URL based on role
  function getDashboardUrl(role) {
    switch(role) {
      case 'admin':
        return '/dashboard.html';
      case 'coach':
        return '/schedules.html';
      case 'member':
        return '/coaches.html';
      default:
        return '/coaches.html';
    }
  }

  // Check if already logged in
  const token = localStorage.getItem('token') || sessionStorage.getItem('token');
  if (token) {
    // Verify token is still valid
    fetch('/api/auth/verify', {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    })
    .then(response => {
      if (response.ok) {
        // Already logged in, redirect to dashboard
        const user = JSON.parse(localStorage.getItem('user') || '{}');
        window.location.href = getDashboardUrl(user.role);
      }
    })
    .catch(err => {
      console.error('Token verification failed:', err);
    });
  }
});
