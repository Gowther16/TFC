// ============================== GLOBAL VARIABLES ==============================
let users = [];
let filteredUsers = [];
let currentUserId = null;

// ============================== INITIALIZATION ==============================
document.addEventListener('DOMContentLoaded', function () {
    loadUsers();
    bindUserEvents();
});

// ============================== EVENT BINDING ==============================
function bindUserEvents() {
    $('#searchInput').on('input', filterUsers);
    $('#searchBtn').on('click', filterUsers);
    $('#roleFilter, #statusFilter').on('change', filterUsers);

    $('#userForm').on('submit', handleUserFormSubmit);

    $(document).on('click', '.edit-user-btn', function () {
        editUser($(this).data('id'));
    });

    $(document).on('click', '.delete-user-btn', function () {
        confirmDeleteUser($(this).data('id'));
    });

    $('#confirmDeleteUserBtn').on('click', deleteUser);

    $('#userModal').on('hidden.bs.modal', resetUserForm);
}

// ============================== API & RENDER ==============================
async function loadUsers() {
    try {
        showLoading(true);
        const response = await fetch('http://localhost:5014/AdminApi/users');
        if (!response.ok) throw new Error('Không thể tải danh sách người dùng');

        const result = await response.json();
        users = (result.data || []).sort((a, b) => a.id - b.id);
        filteredUsers = [...users];
        renderUsers();
    } catch (error) {
        console.error(error);
        showAlert('Có lỗi xảy ra khi tải danh sách người dùng', 'danger');
    } finally {
        showLoading(false);
    }
}

function filterUsers() {
    const searchTerm = $('#searchInput').val().toLowerCase();
    const roleFilter = $('#roleFilter').val();
    const statusFilter = $('#statusFilter').val();

    filteredUsers = users.filter(user => {
        const matchesSearch =
            !searchTerm ||
            user.name.toLowerCase().includes(searchTerm) ||
            user.username.toLowerCase().includes(searchTerm);
        const matchesRole = !roleFilter || user.role === roleFilter;
        const matchesStatus = statusFilter === '' || user.active.toString() === statusFilter;

        return matchesSearch && matchesRole && matchesStatus;
    });

    renderUsers();
}

function renderUsers() {
    const tbody = $('#usersTableBody');
    const noDataMessage = $('#noDataMessage');

    if (filteredUsers.length === 0) {
        tbody.empty();
        noDataMessage.show();
        return;
    }

    noDataMessage.hide();
    const html = filteredUsers.map(user => `
        <tr>
            <td>${user.id}</td>
            <td>${escapeHtml(user.name)}</td>
            <td>${escapeHtml(user.username)}</td>
            <td><span class="badge bg-${getRoleBadgeColor(user.role)}">${escapeHtml(user.role)}</span></td>
            <td><span class="badge bg-${user.active ? 'success' : 'danger'}">${user.active ? 'Hoạt động' : 'Không hoạt động'}</span></td>
            <td>
                <button class="btn btn-sm btn-outline-primary edit-user-btn" data-id="${user.id}"><i class="fas fa-edit"></i></button>
                <button class="btn btn-sm btn-outline-danger delete-user-btn" data-id="${user.id}"><i class="fas fa-trash"></i></button>
            </td>
        </tr>
    `).join('');

    tbody.html(html);
}

// ============================== EDIT / DELETE ==============================
async function editUser(id) {
    try {
        const response = await fetch(`http://localhost:5014/AdminApi/users/${id}`);
        if (!response.ok) throw new Error('Không thể tải thông tin người dùng');
        const result = await response.json();
        const user = result.data;

        currentUserId = id;
        $('#userId').val(user.id);
        $('#userName').val(user.name);
        $('#userUsername').val(user.username);
        $('#userRole').val(user.role);
        $('#userStatus').val(user.active ? 'active' : 'inactive');

        $('#passwordField').hide();
        $('#userPassword').removeAttr('required');

        $('#userModalLabel').text('Chỉnh sửa người dùng');
        new bootstrap.Modal($('#userModal')[0]).show();
    } catch (error) {
        console.error(error);
        showAlert('Có lỗi xảy ra khi tải thông tin người dùng', 'danger');
    }
}

function confirmDeleteUser(id) {
    const user = users.find(u => u.id === id);
    if (user) {
        currentUserId = id;
        $('#deleteUserName').text(user.name);
        new bootstrap.Modal($('#deleteUserModal')[0]).show();
    }
}

async function deleteUser() {
    if (!currentUserId) return;

    try {
        showButtonLoading('#confirmDeleteUserBtn', true);
        const response = await fetch(`http://localhost:5014/AdminApi/users/${currentUserId}`, {
            method: 'DELETE'
        });
        if (!response.ok) throw new Error('Không thể xóa người dùng');

        bootstrap.Modal.getInstance($('#deleteUserModal')[0]).hide();
        showAlert('Xóa người dùng thành công', 'success');
        loadUsers();
    } catch (error) {
        console.error(error);
        showAlert('Có lỗi xảy ra khi xóa người dùng', 'danger');
    } finally {
        showButtonLoading('#confirmDeleteUserBtn', false);
    }
}

// ============================== FORM HANDLING ==============================
async function handleUserFormSubmit(e) {
    e.preventDefault();
    const formData = new FormData(e.target);
    const userData = Object.fromEntries(formData.entries());
    $('.is-invalid').removeClass('is-invalid');

    try {
        showButtonLoading('#saveUserBtn', true);

        const url = currentUserId
            ? `http://localhost:5014/AdminApi/users/${currentUserId}`
            : 'http://localhost:5014/AdminApi/users';

        const method = currentUserId ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
        });

        const result = await response.json();

        if (!response.ok) {
            if (result.errors) {
                showValidationErrors(result.errors);
                return;
            }
            throw new Error(result.message || 'Có lỗi xảy ra');
        }

        bootstrap.Modal.getInstance($('#userModal')[0]).hide();
        showAlert(currentUserId ? 'Cập nhật người dùng thành công' : 'Thêm người dùng thành công', 'success');
        loadUsers();
    } catch (error) {
        console.error(error);
        showAlert(error.message, 'danger');
    } finally {
        showButtonLoading('#saveUserBtn', false);
    }
}

function resetUserForm() {
    currentUserId = null;
    $('#userForm')[0].reset();
    $('#userId').val('');
    $('#userModalLabel').text('Thêm người dùng mới');
    $('#passwordField').show();
    $('#userPassword').attr('required', 'required');
    $('.is-invalid').removeClass('is-invalid');
}

// ============================== UTILITIES ==============================
function showValidationErrors(errors) {
    Object.keys(errors).forEach(field => {
        const input = $(`[name="${field}"]`);
        input.addClass('is-invalid');
        input.siblings('.invalid-feedback').text(errors[field][0]);
    });
}

function showLoading(show) {
    $('#loadingSpinnerA').toggle(show);
    $('#usersTableBody').toggle(!show);
}

function showButtonLoading(selector, show) {
    const btn = $(selector);
    const spinner = btn.find('.spinner-border');
    spinner.toggleClass('d-none', !show);
    btn.prop('disabled', show);
}

function showAlert(message, type) {
    $('#admin-user-alerts').remove();

    const alertHtml = `
        <div id="admin-user-alerts" class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;

    $('.container-fluid:first').prepend(alertHtml);

    setTimeout(() => {
        $('#admin-user-alerts').fadeOut(300, function () {
            $(this).remove();
        });
    }, 5000);
}


function getRoleBadgeColor(role) {
    const colors = {
        admin: 'danger',
        staff: 'warning',
        user: 'info'
    };
    return colors[role] || 'secondary';
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}
