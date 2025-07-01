// ============================== GLOBAL VARIABLES ==============================
let products = [];
let categories = [];
let currentProductId = null;

// ============================== INITIALIZATION ==============================
document.addEventListener('DOMContentLoaded', function () {
    loadCategories();
    loadProducts();
    bindProductEvents();
});

// ============================== EVENT BINDING ==============================
function bindProductEvents() {
    $('#searchInput').on('input', filterProducts);
    $('#categoryFilter, #statusFilter').on('change', filterProducts);
    $('#clearFilters').on('click', clearFilters);
    $('#addProductForm').on('submit', handleAddProductSubmit);
    $('#editProductForm').on('submit', handleEditProductSubmit);

    $('#addProductImage').on('input', () => previewImage($('#addProductImage').val(), 'previewImg', 'imagePreview'));
    $('#editProductImage').on('input', () => previewImage($('#editProductImage').val(), 'editPreviewImg', 'editImagePreview'));

    $(document).on('click', '.edit-product-btn', function () {
        editProduct($(this).data('id'));
    });

    $(document).on('click', '.delete-product-btn', function () {
        confirmDeleteProduct($(this).data('id'));
    });

    $('#confirmDeleteProduct').on('click', deleteProduct);
}

// ============================== API & RENDER ==============================
async function loadCategories() {
    try {
        const res = await fetch('http://localhost:5014/AdminApi/categories');
        const result = await res.json();
        categories = result.data || [];

        const selects = ['addProductCategory', 'editProductCategory', 'categoryFilter'];
        selects.forEach(id => {
            const select = document.getElementById(id);
            select.innerHTML = id === 'categoryFilter' ? '<option value="">Tất cả danh mục</option>' : '<option value="">Chọn danh mục</option>';
            categories.forEach(c => {
                select.innerHTML += `<option value="${c.id}">${c.name}</option>`;
            });
        });
    } catch (err) {
        console.error(err);
    }
}

async function loadProducts() {
    try {
        showLoading(true);
        const response = await fetch('http://localhost:5014/AdminApi/products');
        if (!response.ok) throw new Error('Không thể tải danh sách sản phẩm');

        const result = await response.json();
        products = (result.data || []).sort((a, b) => a.id - b.id);
        renderProducts(products);
    } catch (error) {
        console.error(error);
        showAlert('Lỗi khi tải danh sách sản phẩm', 'danger');
    } finally {
        showLoading(false);
    }
}

function renderProducts(data) {
    const tbody = $('#productsTableBody');
    const emptyState = $('#emptyState');

    if (!Array.isArray(data) || data.length === 0){
        tbody.empty();
        emptyState.removeClass('d-none');
        return;
    }

    emptyState.addClass('d-none');
    const html = data.map((product, index) => `
        <tr>
            <td>${index + 1}</td>
            <td>
                <img src="${product.imageUrl || 'https://via.placeholder.com/50x50?text=No+Image'}"
                     alt="${product.name}" class="img-thumbnail" style="width: 50px; height: 50px;">
            </td>
            <td>
                <strong>${product.name}</strong>
                ${product.description ? `<br><small class="text-muted">${product.description}</small>` : ''}
            </td>
            <td><span class="badge bg-info">${product.categoryName || 'Chưa phân loại'}</span></td>
            <td><strong class="text-success">${formatCurrency(product.price)}</strong></td>
            <td><span class="badge ${product.inventory > 0 ? 'bg-success' : 'bg-danger'}">${product.inventory}</span></td>
            <td><span class="badge ${product.inventory > 0 ? 'bg-success' : 'bg-danger'}">${product.inventory > 0 ? 'Còn hàng' : 'Hết hàng'}</span></td>
            <td>
                <button class="btn btn-sm btn-outline-primary edit-product-btn" data-id="${product.id}"><i class="fas fa-edit"></i></button>
                <button class="btn btn-sm btn-outline-danger delete-product-btn" data-id="${product.id}"><i class="fas fa-trash"></i></button>
            </td>
        </tr>
    `).join('');

    tbody.html(html);
}

// ============================== FORM HANDLING ==============================
async function handleAddProductSubmit(e) {
    e.preventDefault();
    const data = Object.fromEntries(new FormData(e.target).entries());

    try {
        showButtonLoading('#saveAddProductBtn', true);
        const res = await fetch('http://localhost:5014/AdminApi/products', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        const result = await res.json();

        if (!res.ok) {
            showAlert(result.message || 'Thêm sản phẩm thất bại', 'danger');
            return;
        }

        $('#addProductModal').modal('hide');
        showAlert('Thêm sản phẩm thành công', 'success');
        loadProducts();
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi thêm sản phẩm', 'danger');
    } finally {
        showButtonLoading('#saveAddProductBtn', false);
    }
}

async function handleEditProductSubmit(e) {
    e.preventDefault();
    const data = Object.fromEntries(new FormData(e.target).entries());
    const id = $('#editProductId').val();

    try {
        showButtonLoading('#saveEditProductBtn', true);
        const res = await fetch(`http://localhost:5014/AdminApi/products/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        const result = await res.json();

        if (!res.ok) {
            showAlert(result.message || 'Cập nhật thất bại', 'danger');
            return;
        }

        $('#editProductModal').modal('hide');
        showAlert('Cập nhật sản phẩm thành công', 'success');
        loadProducts();
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi cập nhật sản phẩm', 'danger');
    } finally {
        showButtonLoading('#saveEditProductBtn', false);
    }
}

async function deleteProduct() {
    const id = $('#confirmDeleteProduct').data('productId');
    try {
        showButtonLoading('#confirmDeleteProduct', true);
        const res = await fetch(`http://localhost:5014/AdminApi/products/${id}`, {
            method: 'DELETE'
        });

        const result = await res.json();

        if (!res.ok) {
            showAlert(result.message || 'Xóa thất bại', 'danger');
            return;
        }

        $('#deleteProductModal').modal('hide');
        showAlert('Xóa sản phẩm thành công', 'success');
        loadProducts();
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi xóa sản phẩm', 'danger');
    } finally {
        showButtonLoading('#confirmDeleteProduct', false);
    }
}

async function editProduct(id) {
    try {
        const res = await fetch(`http://localhost:5014/AdminApi/products/${id}`);
        const result = await res.json();

        if (!res.ok) {
            showAlert('Không thể tải dữ liệu sản phẩm', 'danger');
            return;
        }

        const product = result.data;
        currentProductId = id;
        $('#editProductId').val(product.id);
        $('#editProductName').val(product.name);
        $('#editProductDescription').val(product.description);
        $('#editProductPrice').val(product.price);
        $('#editProductStock').val(product.inventory);
        $('#editProductImage').val(product.imageUrl);
        $('#editProductCategory').val(product.categoryId);

        previewImage(product.imageUrl, 'editPreviewImg', 'editImagePreview');

        $('#editProductModal').modal('show');
    } catch (err) {
        console.error(err);
        showAlert('Lỗi khi tải sản phẩm', 'danger');
    }
}

function confirmDeleteProduct(id) {
    const product = products.find(p => p.id === id);
    if (product) {
        $('#deleteProductName').text(product.name);
        $('#confirmDeleteProduct').data('productId', id);
        $('#deleteProductModal').modal('show');
    }
}

// ============================== UTILITIES ==============================
function filterProducts() {
    const search = $('#searchInput').val().toLowerCase();
    const category = $('#categoryFilter').val();
    const status = $('#statusFilter').val();

    const filtered = products.filter(p => {
        const matchSearch = !search || p.name.toLowerCase().includes(search);
        const matchCategory = !category || p.categoryId == category;
        const matchStatus = status === '' || (status === 'available' ? p.inventory > 0 : p.inventory <= 0);
        return matchSearch && matchCategory && matchStatus;
    });

    renderProducts(filtered);
}

function clearFilters() {
    $('#searchInput').val('');
    $('#categoryFilter').val('');
    $('#statusFilter').val('');
    renderProducts(products);
}

function showLoading(show) {
    $('#loadingSpinnerP').toggle(show);
    $('#productsTableBody').toggle(!show);
}

function showButtonLoading(selector, show) {
    const btn = $(selector);
    const spinner = btn.find('.spinner-border');
    spinner.toggleClass('d-none', !show);
    btn.prop('disabled', show);
}

function showAlert(message, type) {
    $('#admin-product-alerts').remove();

    const alertHtml = `
        <div id="admin-product-alerts" class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;

    $('.container-fluid:first').prepend(alertHtml);

    setTimeout(() => {
        $('#admin-product-alerts').fadeOut(300, function () {
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
