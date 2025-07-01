document.addEventListener('DOMContentLoaded', function () {
    // Initialize variables
    let currentOrders = [];

    // API Configuration
    const API_BASE_URL = 'http://localhost:5014/StaffApi';

    // DOM elements
    const loadingState = document.getElementById('loadingState');
    const emptyState = document.getElementById('emptyState');
    const ordersListStaff = document.getElementById('ordersListStaff');
    const orderDetailModal = document.getElementById('orderDetailModal');
    const orderDetailContent = document.getElementById('orderDetailContent');

    // Initialize page
    filterOrders();

    // Check for urgent orders every 30 seconds
    setInterval(checkUrgentOrders, 30000);

    // Filter listeners
    document.querySelectorAll('input[name="statusFilter"]').forEach(radio => {
        radio.addEventListener('change', filterOrders);
    });

    // Main functions
    async function loadOrders() {
        showLoading();

        try {
            const response = await fetch(`${API_BASE_URL}/orders`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();
                
            console.log('API Response:', result);
            console.log('Orders data:', result.data);

            if (result.success) {
                currentOrders = result.data || [];
                console.log('Current orders:', currentOrders);
                displayOrders(currentOrders);
                updateStats(currentOrders);
            } else {
                throw new Error(result.message || 'Failed to load orders');
            }
        } catch (error) {
            console.error('Error loading orders:', error);
            showError('Có lỗi xảy ra khi tải danh sách đơn hàng: ' + error.message);
            displayOrders([]);
        }
    }
    async function loadOrdersByStatus(status) {
        showLoading();

        try {
            const response = await fetch(`${API_BASE_URL}/orders/${status}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            if (result.success) {
                return result.data || [];
            } else {
                throw new Error(result.message || 'Failed to load orders');
            }
        } catch (error) {
            console.error('Error loading orders by status:', error);
            showError('Có lỗi xảy ra khi tải danh sách đơn hàng: ' + error.message);
            return [];
        }
    }

    async function viewOrderDetail(orderId) {
        selectedOrderId = orderId;

        try {
            const response = await fetch(`${API_BASE_URL}/order/${orderId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            if (result.success && result.data) {
                displayOrderDetail(result.data);
                const modal = new bootstrap.Modal(orderDetailModal);
                modal.show();
            } else {
                throw new Error(result.message || 'Failed to load order detail');
            }
        } catch (error) {
            console.error('Error loading order detail:', error);
            showError('Có lỗi xảy ra khi tải chi tiết đơn hàng: ' + error.message);
        }
    }

    async function acceptOrder(orderId) {
        if (!confirm('Bạn có chắc chắn muốn chấp nhận đơn hàng này?')) {
            return;
        }

        try {
            const response = await fetch(`${API_BASE_URL}/order/${orderId}/status`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    status: 'confirmed'
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            if (result.success) {
                showSuccess('Đã chấp nhận đơn hàng thành công!');

                const modal = bootstrap.Modal.getInstance(orderDetailModal);
                if (modal) {
                    modal.hide();
                }

                setTimeout(() => {
                    loadOrders();
                }, 1000);
            } else {
                throw new Error(result.message || 'Failed to accept order');
            }
        } catch (error) {
            console.error('Error accepting order:', error);
            showError('Có lỗi xảy ra khi chấp nhận đơn hàng: ' + error.message);
        }
    }

    async function completeOrder(orderId) {
        if (!confirm('Bạn có chắc chắn muốn hoàn thành đơn hàng này?')) {
            return;
        }

        try {
            const response = await fetch(`${API_BASE_URL}/order/${orderId}/status`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    status: 'completed'
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            if (result.success) {
                showSuccess('Đã hoàn thành đơn hàng thành công!');

                const modal = bootstrap.Modal.getInstance(orderDetailModal);
                if (modal) {
                    modal.hide();
                }

                setTimeout(() => {
                    loadOrders();
                }, 1500);
            } else {
                throw new Error(result.message || 'Failed to complete order');
            }
        } catch (error) {
            console.error('Error completing order:', error);
            showError('Có lỗi xảy ra khi hoàn thành đơn hàng: ' + error.message);
        }
    }

    async function filterOrders() {
        const filterValue = document.querySelector('input[name="statusFilter"]:checked').id;

        try {
            let filteredOrders = [];

            if (filterValue === 'all') {
                await loadOrders();
                return;
            } else if (filterValue === 'pending') {
                const pending = await loadOrdersByStatus('pending');
                const confirmed = await loadOrdersByStatus('confirmed');
                filteredOrders = pending.concat(confirmed);

            } else if (filterValue === 'urgent') {
                const pendingOrders = await loadOrdersByStatus('pending');
                filteredOrders = pendingOrders.filter(order => {
                    const minutesAgo = Math.floor((new Date() - new Date(order.createdAt)) / 60000);
                    return minutesAgo > 15;
                });
            }
            

            currentOrders = filteredOrders;
            displayOrders(filteredOrders);
            updateStats(filteredOrders);
        } catch (error) {
            console.error('Error filtering orders:', error);
            showError('Có lỗi xảy ra khi lọc đơn hàng: ' + error.message);
        }
    }

    // Display functions
    function showLoading() {
        loadingState.classList.remove('d-none');
        emptyState.classList.add('d-none');
        ordersListStaff.classList.add('d-none');
    }

    function displayOrders(orders) {
        console.log('Displaying orders:', orders);
        document.getElementById('ordersCountText').textContent = `Tổng cộng: ${orders.length} đơn hàng`;
        // Check if orders are empty display empty state
        if (!orders || orders.length === 0) {
            loadingState.classList.add('d-none');
            emptyState.classList.remove('d-none');
            ordersListStaff.classList.add('d-none');
            return;
        }
        // Hide loading and empty states, show orders container
        loadingState.classList.add('d-none');
        emptyState.classList.add('d-none');
        ordersListStaff.classList.remove('d-none');

        console.log('ordersListStaff:', ordersListStaff);
        const html = orders.map(order => createOrderCard(order)).join('');
        console.log(html);
        ordersListStaff.innerHTML = html;
    }

    function createOrderCard(order) {
        const createdAt = new Date(order.createdAt);
        const timeAgo = getTimeAgo(createdAt);
        const minutesAgo = Math.floor((new Date() - createdAt) / 60000);
        const isUrgent = minutesAgo > 15 && (order.status === 'Pending' || order.status === 'confirmed');
        const itemsCount = order.orderItems?.length || 0;
        const totalItems = order.orderItems?.reduce((sum, item) => sum + item.quantity, 0) || 0;

        return `
            <div class="col-12">
                <div class="card order-card ${isUrgent ? 'urgent' : ''}" onclick="viewOrderDetail(${order.id})">
                    <div class="order-header">
                        <div class="row align-items-center">
                            <div class="col-md-6">
                                <div class="d-flex align-items-center mb-2">
                                    <h5 class="mb-0 me-3">
                                        <i class="fas fa-hashtag text-primary me-2"></i>
                                        ${order.orderNumber || 'N/A'}
                                    </h5>
                                    ${isUrgent ? `
                                        <span class="urgent-indicator">
                                            <i class="fas fa-exclamation-triangle me-1"></i>
                                            Cần xử lý gấp!
                                        </span>
                                    ` : ''}
                                </div>
                                <div class="d-flex align-items-center text-muted">
                                    <i class="fas fa-user me-1"></i>
                                    <span class="me-3">${order.customerName || 'N/A'}</span>
                                    <i class="fas fa-clock me-1"></i>
                                    <span>${timeAgo}</span>
                                    ${isUrgent ? `<span class="text-danger ms-2">(${minutesAgo} phút)</span>` : ''}
                                </div>
                            </div>
                            <div class="col-md-6 text-md-end">
                                <span class="status-badge status-${order.status?.toLowerCase() || 'pending'}">
                                    ${getStatusText(order.status)}
                                </span>
                                <div class="mt-2">
                                    <strong class="text-success fs-5">
                                        ${formatCurrency(order.totalAmount)}
                                    </strong>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-8">
                                <h6 class="text-muted mb-2">
                                    <i class="fas fa-shopping-bag me-2"></i>
                                    Sản phẩm (${itemsCount} món, ${totalItems} phần)
                                </h6>
                                ${order.orderItems?.slice(0, 3).map(item => `
                                    <div class="order-item">
                                        <div class="d-flex justify-content-between">
                                            <div>
                                                <strong>${item.productName || item.comboName || 'N/A'}</strong>
                                                <span class="text-muted"> x${item.quantity || 0}</span>
                                            </div>
                                            <span class="text-success">
                                                ${formatCurrency(item.subTotal)}
                                            </span>
                                        </div>
                                    </div>
                                `).join('') || '<div class="text-muted">Không có sản phẩm</div>'}
                                ${itemsCount > 3 ? `
                                    <div class="text-muted">
                                        <small>... và ${itemsCount - 3} món khác</small>
                                    </div>
                                ` : ''}
                            </div>
                            <div class="col-md-4 text-md-end">
                                <div class="d-grid gap-2">
                                    <button class="btn btn-view btn-action" onclick="event.stopPropagation(); viewOrderDetail(${order.id})">
                                        <i class="fas fa-eye me-2"></i>
                                        Xem chi tiết
                                    </button>
                                    ${order.status === 'Pending' || order.status === 'pending' ? `
                                        <button class="btn btn-accept btn-action" onclick="event.stopPropagation(); acceptOrder(${order.id})">
                                            <i class="fas fa-check me-2"></i>
                                            Chấp nhận
                                        </button>
                                    ` : (order.status === 'Confirmed' || order.status === 'confirmed') ? `
                                        <button class="btn btn-complete btn-action" onclick="event.stopPropagation(); completeOrder(${order.id})">
                                            <i class="fas fa-check-double me-2"></i>
                                            Hoàn thành
                                        </button>
                                    ` : ''}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    function displayOrderDetail(order) {
        const createdAt = new Date(order.createdAt);
        const endTime = (order.status?.toLowerCase() === 'completed' && order.updatedAt)
            ? new Date(order.updatedAt)
            : new Date();

        const minutesAgo = Math.floor((endTime - createdAt) / 60000);

        const isUrgent = minutesAgo > 15;

        orderDetailContent.innerHTML = `
            <div class="row mb-4">
                <div class="col-md-6">
                    <h6 class="text-muted">Thông tin đơn hàng</h6>
                    <p><strong>Mã đơn:</strong> ${order.orderNumber}</p>
                    <p><strong>Khách hàng:</strong> ${order.customerName}</p>
                    <p><strong>Trạng thái:</strong>
                        <span class="status-badge status-${order.status?.toLowerCase() || 'Pending'}">
                            ${getStatusText(order.status)}
                        </span>
                        ${isUrgent && (order.status === 'Pending' || order.status === 'pending') ? `
                            <span class="urgent-indicator ms-2">
                                <i class="fas fa-exclamation-triangle me-1"></i>
                                Cần gấp!
                            </span>
                        ` : ''}
                    </p>
                </div>
                <div class="col-md-6">
                    <h6 class="text-muted">Thời gian & Giá trị</h6>
                    <p><strong>Thời gian đặt:</strong> ${createdAt.toLocaleString('vi-VN')}</p>
                    <p><strong>Thời gian xử lý:</strong> 
                        <span class="${isUrgent ? 'text-danger fw-bold' : ''}">${minutesAgo} phút</span>
                    </p>
                    <p><strong>Tổng tiền:</strong>
                        <span class="text-success fs-5">${formatCurrency(order.totalAmount)}</span>
                    </p>
                </div>
            </div>

            <h6 class="text-muted mb-3">Chi tiết sản phẩm</h6>
            <div class="table-responsive">
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th>Sản phẩm</th>
                            <th class="text-center">Số lượng</th>
                            <th class="text-end">Đơn giá</th>
                            <th class="text-end">Thành tiền</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${order.orderItems?.map(item => `
                            <tr>
                                <td>
                                    <strong>${item.productName || item.comboName || 'N/A'}</strong>
                                </td>
                                <td class="text-center">${item.quantity || 0}</td>
                                <td class="text-end">${formatCurrency(item.unitPrice)}</td>
                                <td class="text-end"><strong>${formatCurrency(item.subTotal)}</strong></td>
                            </tr>
                        `).join('') || '<tr><td colspan="4" class="text-center">Không có sản phẩm</td></tr>'}
                    </tbody>
                    <tfoot>
                        <tr class="table-success">
                            <th colspan="3">Tổng cộng</th>
                            <th class="text-end">${formatCurrency(order.totalAmount)}</th>
                        </tr>
                    </tfoot>
                </table>
            </div>
        `;

        // Update buttons based on order status
        const acceptBtn = document.getElementById('acceptOrderBtn');
        const completeBtn = document.getElementById('completeOrderBtn');

        if (order.status === 'Pending' || order.status === 'pending') {
            acceptBtn.style.display = 'inline-block';
            completeBtn.style.display = 'none';
            acceptBtn.onclick = () => acceptOrder(order.id);
        } else if (order.status === 'confirmed') {
            acceptBtn.style.display = 'none';
            completeBtn.style.display = 'inline-block';
            completeBtn.onclick = () => completeOrder(order.id);
        } else {
            acceptBtn.style.display = 'none';
            completeBtn.style.display = 'none';
        }
    }

    function updateStats(orders) {
        const today = new Date().toDateString();
        const todayOrders = orders.filter(order =>
            new Date(order.createdAt).toDateString() === today
        );
        const pendingOrders = orders.filter(order =>
            order.status === 'Pending' || order.status === 'pending'
        );
        const urgentOrders = orders.filter(order => {
            const minutesAgo = Math.floor((new Date() - new Date(order.createdAt)) / 60000);
            return minutesAgo > 15 && (order.status === 'Pending' || order.status === 'pending');
        });
        const totalAmount = orders.reduce((sum, order) => sum + (order.totalAmount || 0), 0);

        document.getElementById('pendingCount').textContent = pendingOrders.length;
        document.getElementById('urgentCount').textContent = urgentOrders.length;
        document.getElementById('todayCount').textContent = todayOrders.length;
        document.getElementById('totalAmount').textContent = formatCurrency(totalAmount);
    }

    function checkUrgentOrders() {
        const urgentOrders = currentOrders.filter(order => {
            const minutesAgo = Math.floor((new Date() - new Date(order.createdAt)) / 60000);
            return minutesAgo > 15 && (order.status === 'Pending' || order.status === 'pending');
        });

        if (urgentOrders.length > 0) {
            showUrgent(`Có ${urgentOrders.length} đơn hàng cần xử lý gấp! (>15 phút)`);
        }
    }

    // Utility functions
    function formatCurrency(amount) {
        if (!amount && amount !== 0) return '0 ₫';
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
            minimumFractionDigits: 0
        }).format(amount);
    }

    function getStatusText(status) {
        const statusMap = {
            'pending': 'Chờ xử lý',
            'confirmed': 'Đã xác nhận',
            'completed': 'Hoàn thành',
            'cancelled': 'Đã hủy'
        };
        return statusMap[status?.toLowerCase()] || 'Chờ xử lý';
    }

    function getTimeAgo(date) {
        if (!date) return 'Không xác định';

        try {
            const now = new Date();
            const diff = now - date;
            const minutes = Math.floor(diff / 60000);
            const hours = Math.floor(diff / 3600000);
            const days = Math.floor(diff / 86400000);

            if (days > 0) return `${days} ngày trước`;
            if (hours > 0) return `${hours} giờ trước`;
            if (minutes > 0) return `${minutes} phút trước`;
            return 'Vừa xong';
        } catch (error) {
            console.error('Date formatting error:', error);
            return 'Không xác định';
        }
    }

    function showSuccess(message) {
        const successMessage = document.getElementById('successMessage');
        const successToast = document.getElementById('successToast');

        if (successMessage && successToast) {
            successMessage.textContent = message;
            const toast = new bootstrap.Toast(successToast);
            toast.show();
        }
    }

    function showError(message) {
        const errorMessage = document.getElementById('errorMessage');
        const errorToast = document.getElementById('errorToast');

        if (errorMessage && errorToast) {
            errorMessage.textContent = message;
            const toast = new bootstrap.Toast(errorToast);
            toast.show();
        }
    }

    function showUrgent(message) {
        const urgentMessage = document.getElementById('urgentMessage');
        const urgentToast = document.getElementById('urgentToast');

        if (urgentMessage && urgentToast) {
            urgentMessage.textContent = message;
            const toast = new bootstrap.Toast(urgentToast, {
                autohide: false
            });
            toast.show();
        }
    }

    // Make functions globally accessible for onclick handlers
    window.viewOrderDetail = viewOrderDetail;
    window.acceptOrder = acceptOrder;
    window.completeOrder = completeOrder;
    window.loadOrders = loadOrders;
});