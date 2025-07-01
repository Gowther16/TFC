// ============================== GLOBAL VARIABLES ==============================
let combos = [];
let products = [];
let currentComboId = null;
let comboProductCounter = 0;

// ============================== INITIALIZATION ==============================
document.addEventListener('DOMContentLoaded', function () {
    loadProducts();
    loadCombos();
    bindComboEvents();
});

// ============================== EVENT BINDING ==============================
function bindComboEvents() {
    $('#searchInput').on('input', filterCombos);
    $('#statusFilter').on('change', filterCombos);
    $('#clearFilters').on('click', clearFilters);
    $('#addComboForm').on('submit', handleAddComboSubmit);
    $('#editComboForm').on('submit', handleEditComboSubmit);

    $('#addComboImage').on('input', () => previewImage($('#addComboImage').val(), 'previewImg', 'imagePreview'));
    $('#editComboImage').on('input', () => previewImage($('#editComboImage').val(), 'editPreviewImg', 'editImagePreview'));

    $(document).on('click', '.view-combo-btn', function () {
        viewCombo($(this).data('id'));
    });

    $(document).on('click', '.edit-combo-btn', function () {
        editCombo($(this).data('id'));
    });

    $(document).on('click', '.delete-combo-btn', function () {
        confirmDeleteCombo($(this).data('id'));
    });

    $('#confirmDeleteCombo').on('click', deleteCombo);

    // Add/Remove product buttons
    $('#addProductToCombo').on('click', addProductToCombo);
    $('#addProductToEditCombo').on('click', addProductToEditCombo);
}

// ============================== API & RENDER ==============================
async function loadProducts() {
    try {
        const res = await fetch('http://localhost:5014/AdminApi/products');
        const result = await res.json();
        products = result.data || [];
    } catch (err) {
        console.error('Error loading products:', err);
    }
}

async function loadCombos() {
    try {
        showLoading(true);
        const response = await fetch('http://localhost:5014/AdminApi/combos');
        if (!response.ok) throw new Error('Không thể tải danh sách combo');

        const result = await response.json();
        combos = (result.data || []).sort((a, b) => a.id - b.id);
        renderCombos(combos);
    } catch (error) {
        console.error(error);
        showAlert('Lỗi khi tải danh sách combo', 'danger');
    } finally {
        showLoading(false);
    }
}

function renderCombos(data) {
    console.log("Rendering", combos);
    const tbody = $('#combosTableBody');
    const emptyState = $('#emptyState');

    if (!Array.isArray(data) || data.length === 0) {
        tbody.empty();
        emptyState.removeClass('d-none');
        return;
    }

    emptyState.addClass('d-none');
    const html = data.map((combo, index) => `
        <tr>
            <td>${index + 1}</td>
            <td>
                <img src="${combo.imageUrl || 'https://via.placeholder.com/50x50?text=No+Image'}"
                     alt="${combo.name}" class="img-thumbnail" style="width: 50px; height: 50px;">
            </td>
            <td>
                <strong>${combo.name}</strong>
                ${combo.description ? `<br><small class="text-muted">${combo.description}</small>` : ''}
            </td>
            <td>
                <small class="text-muted">${combo.productCount || 0} sản phẩm</small>
            </td>
            <td><strong class="text-success">${formatCurrency(combo.price)}</strong></td>
            <td>
                <span class="badge ${combo.available ? 'bg-success' : 'bg-danger'}">
                    ${combo.available ? 'Còn hàng' : 'Hết hàng'}
                </span>
            </td>
            <td>
                <button class="btn btn-sm btn-outline-info view-combo-btn" data-id="${combo.id}" title="Xem chi tiết">
                    <i class="fas fa-eye"></i>
                </button>
                <button class="btn btn-sm btn-outline-primary edit-combo-btn" data-id="${combo.id}" title="Chỉnh sửa">
                    <i class="fas fa-edit"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger delete-combo-btn" data-id="${combo.id}" title="Xóa">
                    <i class="fas fa-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');

    tbody.html(html);
}

// ============================== COMBO PRODUCTS MANAGEMENT ==============================
function addProductToCombo() {
    comboProductCounter++;
    const productOptions = products.map(p =>
        `<option value="${p.id}">${p.name} - ${formatCurrency(p.price)}</option>`
    ).join('');

    const html = `
        <div class="row mb-2 combo-product-item" data-index="${comboProductCounter}">
            <div class="col-md-8">
                <select class="form-select" name="products[${comboProductCounter}][productId]" required>
                    <option value="">Chọn sản phẩm</option>
                    ${productOptions}
                </select>
            </div>
            <div class="col-md-3">
                <input type="number" class="form-control" name="products[${comboProductCounter}][quantity]" 
                       min="1" value="1" required placeholder="Số lượng">
            </div>
            <div class="col-md-1">
                <button type="button" class="btn btn-sm btn-outline-danger" onclick="removeComboProduct(${comboProductCounter})">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        </div>
    `;

    $('#comboProductsList').append(html);
}

function addProductToEditCombo() {
    comboProductCounter++;
    const productOptions = products.map(p =>
        `<option value="${p.id}">${p.name} - ${formatCurrency(p.price)}</option>`
    ).join('');

    const html = `
        <div class="row mb-2 combo-product-item" data-index="${comboProductCounter}">
            <div class="col-md-8">
                <select class="form-select" name="products[${comboProductCounter}][productId]" required>
                    <option value="">Chọn sản phẩm</option>
                    ${productOptions}
                </select>
            </div>
            <div class="col-md-3">
                <input type="number" class="form-control" name="products[${comboProductCounter}][quantity]" 
                       min="1" value="1" required placeholder="Số lượng">
            </div>
            <div class="col-md-1">
                <button type="button" class="btn btn-sm btn-outline-danger" onclick="removeComboProduct(${comboProductCounter})">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        </div>
    `;

    $('#editComboProductsList').append(html);
}

function removeComboProduct(index) {
    $(`.combo-product-item[data-index="${index}"]`).remove();
}

// ============================== FORM HANDLING ==============================
async function handleAddComboSubmit(e) {
    e.preventDefault();
    const formData = new FormData(e.target);

    // Extract combo data
    const comboData = {
        name: formData.get('name'),
        description: formData.get('description'),
        price: parseFloat(formData.get('price')),
        imageUrl: formData.get('imageUrl'),
        available: 1,
        products: []
    };

    // Extract products data
    const productEntries = [...formData.entries()].filter(([key]) => key.startsWith('products['));
    const productsByIndex = {};

    productEntries.forEach(([key, value]) => {
        const match = key.match(/products\[(\d+)\]\[(\w+)\]/);
        if (match) {
            const [, index, field] = match;
            if (!productsByIndex[index]) productsByIndex[index] = {};
            productsByIndex[index][field] = field === 'quantity' ? parseInt(value) : value;
        }
    });

    comboData.products = Object.values(productsByIndex).filter(p => p.productId && p.quantity);

    if (comboData.products.length === 0) {
        showAlert('Vui lòng chọn ít nhất một sản phẩm cho combo', 'warning');
        return;
    }

    try {
        showButtonLoading('#saveAddComboBtn', true);
        const res = await fetch('http://localhost:5014/AdminApi/combos', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(comboData)
        });

        const result = await res.json();

        if (!res.ok) {
            showAlert(result.message || 'Thêm combo thất bại', 'danger');
            return;
        }

        $('#addComboModal').modal('hide');
        $('#addComboForm')[0].reset();
        $('#comboProductsList').empty();
        comboProductCounter = 0;
        showAlert('Thêm combo thành công', 'success');
        loadCombos();
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi thêm combo', 'danger');
    } finally {
        showButtonLoading('#saveAddComboBtn', false);
    }
}

async function handleEditComboSubmit(e) {
    e.preventDefault();
    const formData = new FormData(e.target);
    const id = $('#editComboId').val();

    // Extract combo data
    const comboData = {
        name: formData.get('name'),
        description: formData.get('description'),
        price: parseFloat(formData.get('price')),
        imageUrl: formData.get('imageUrl'),
        available: 1,
        products: []
    };

    // Extract products data
    const productEntries = [...formData.entries()].filter(([key]) => key.startsWith('products['));
    const productsByIndex = {};

    productEntries.forEach(([key, value]) => {
        const match = key.match(/products\[(\d+)\]\[(\w+)\]/);
        if (match) {
            const [, index, field] = match;
            if (!productsByIndex[index]) productsByIndex[index] = {};
            productsByIndex[index][field] = field === 'quantity' ? parseInt(value) : value;
        }
    });

    comboData.products = Object.values(productsByIndex).filter(p => p.productId && p.quantity);

    if (comboData.products.length === 0) {
        showAlert('Vui lòng chọn ít nhất một sản phẩm cho combo', 'warning');
        return;
    }

    try {
        showButtonLoading('#saveEditComboBtn', true);
        const res = await fetch(`http://localhost:5014/AdminApi/combos/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(comboData)
        });

        const result = await res.json();

        if (!res.ok) {
            showAlert(result.message || 'Cập nhật thất bại', 'danger');
            return;
        }

        $('#editComboModal').modal('hide');
        showAlert('Cập nhật combo thành công', 'success');
        loadCombos();
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi cập nhật combo', 'danger');
    } finally {
        showButtonLoading('#saveEditComboBtn', false);
    }
}

async function deleteCombo() {
    const id = $('#confirmDeleteCombo').data('comboId');
    try {
        showButtonLoading('#confirmDeleteCombo', true);
        const res = await fetch(`http://localhost:5014/AdminApi/combos/${id}`, {
            method: 'DELETE'
        });

        const result = await res.json();

        if (!res.ok) {
            showAlert(result.message || 'Xóa thất bại', 'danger');
            return;
        }

        $('#deleteComboModal').modal('hide');
        showAlert('Xóa combo thành công', 'success');
        loadCombos();
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi xóa combo', 'danger');
    } finally {
        showButtonLoading('#confirmDeleteCombo', false);
    }
}

async function viewCombo(id) {
    try {
        const res = await fetch(`http://localhost:5014/AdminApi/combos/${id}`);
        const result = await res.json();

        if (!res.ok) {
            showAlert('Không thể tải chi tiết combo', 'danger');
            return;
        }

        const combo = result.data;
        let productsHtml = '';

        if (combo.products && combo.products.length > 0) {
            productsHtml = combo.products.map(item => `
                <div class="d-flex justify-content-between align-items-center border-bottom py-2">
                    <div>
                        <strong>${item.productName}</strong>
                        <br><small class="text-muted">Giá: ${formatCurrency(item.price)}</small>
                    </div>
                    <span class="badge bg-primary">x${item.quantity}</span>
                </div>
            `).join('');
        } else {
            productsHtml = '<p class="text-muted">Không có sản phẩm nào trong combo</p>';
        }

        const detailsHtml = `
            <div class="row">
                <div class="col-md-4 text-center">
                    <img src="${combo.imageUrl || 'https://via.placeholder.com/200x200?text=No+Image'}" 
                         alt="${combo.name}" class="img-fluid rounded">
                </div>
                <div class="col-md-8">
                    <h4>${combo.name}</h4>
                    <p class="text-muted">${combo.description || 'Không có mô tả'}</p>
                    <p><strong>Giá: </strong><span class="text-success">${formatCurrency(combo.price)}</span></p>
                    <p><strong>Trạng thái: </strong>
                        <span class="badge ${combo.available ? 'bg-success' : 'bg-danger'}">
                            ${combo.available ? 'Đang bán' : 'Ngừng bán'}
                        </span>
                    </p>
                </div>
            </div>
            <hr>
            <h5>Sản phẩm trong combo:</h5>
            <div class="mt-3">
                ${productsHtml}
            </div>
        `;

        $('#comboDetailsContent').html(detailsHtml);
        $('#viewComboModal').modal('show');
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi tải chi tiết combo', 'danger');
    }
}

async function editCombo(id) {
    try {
        const res = await fetch(`http://localhost:5014/AdminApi/combos/${id}`);
        const result = await res.json();

        if (!res.ok) {
            showAlert('Không thể tải dữ liệu combo', 'danger');
            return;
        }

        const combo = result.data;
        currentComboId = id;

        // Fill basic info
        $('#editComboId').val(combo.id);
        $('#editComboName').val(combo.name);
        $('#editComboDescription').val(combo.description);
        $('#editComboPrice').val(combo.price);
        $('#editComboImage').val(combo.imageUrl);

        // Preview image
        previewImage(combo.imageUrl, 'editPreviewImg', 'editImagePreview');

        // Clear existing products
        $('#editComboProductsList').empty();
        comboProductCounter = 0;

        // Add existing products
        if (combo.products && combo.products.length > 0) {
            combo.products.forEach(item => {
                comboProductCounter++;
                const productOptions = products.map(p =>
                    `<option value="${p.id}" ${p.id == item.productId ? 'selected' : ''}>${p.name} - ${formatCurrency(p.price)}</option>`
                ).join('');

                const html = `
                    <div class="row mb-2 combo-product-item" data-index="${comboProductCounter}">
                        <div class="col-md-8">
                            <select class="form-select" name="products[${comboProductCounter}][productId]" required>
                                <option value="">Chọn sản phẩm</option>
                                ${productOptions}
                            </select>
                        </div>
                        <div class="col-md-3">
                            <input type="number" class="form-control" name="products[${comboProductCounter}][quantity]" 
                                   min="1" value="${item.quantity}" required placeholder="Số lượng">
                        </div>
                        <div class="col-md-1">
                            <button type="button" class="btn btn-sm btn-outline-danger" onclick="removeComboProduct(${comboProductCounter})">
                                <i class="fas fa-times"></i>
                            </button>
                        </div>
                    </div>
                `;

                $('#editComboProductsList').append(html);
            });
        }

        $('#editComboModal').modal('show');
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi tải combo', 'danger');
    }
}

function confirmDeleteCombo(id) {
    const combo = combos.find(c => c.id === id);
    if (combo) {
        $('#deleteComboName').text(combo.name);
        $('#confirmDeleteCombo').data('comboId', id);
        $('#deleteComboModal').modal('show');
    }
}

// ============================== UTILITIES ==============================
function filterCombos() {
    const search = $('#searchInput').val().toLowerCase();
    const status = $('#statusFilter').val();

    const filtered = combos.filter(c => {
        const matchSearch = !search || c.name.toLowerCase().includes(search);
        const matchStatus = status === ''
            || (status === '1' && c.available)
            || (status === '0' && !c.available);
        return matchSearch && matchStatus;
    });

    renderCombos(filtered);
}


function clearFilters() {
    $('#searchInput').val('');
    $('#statusFilter').val('');
    renderCombos(combos);
}

function showLoading(show) {
    $('#loadingSpinnerC').toggle(show);
    $('#combosTableBody').toggle(!show);
}

function showButtonLoading(selector, show) {
    const btn = $(selector);
    const spinner = btn.find('.spinner-border');
    spinner.toggleClass('d-none', !show);
    btn.prop('disabled', show);
}

function showAlert(message, type) {
    $('#admin-combo-alerts').remove();

    const alertHtml = `
        <div id="admin-combo-alerts" class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;

    $('.container-fluid:first').prepend(alertHtml);

    setTimeout(() => {
        $('#admin-combo-alerts').fadeOut(300, function () {
            $(this).remove();
        });
    }, 5000);
}

function previewImage(url, imgId, containerId) {
    const img = document.getElementById(imgId);
    const container = document.getElementById(containerId);
    if (url) {
        img.src = url;
        container.style.display = 'block';
        img.onerror = () => (container.style.display = 'none');
    } else {
        container.style.display = 'none';
    }
}

function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}