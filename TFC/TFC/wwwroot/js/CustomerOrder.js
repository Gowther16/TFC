document.addEventListener('DOMContentLoaded', function () {
    // Sử dụng Bootstrap Modal API
    const modal = new bootstrap.Modal(document.getElementById('orderModal'));
    const cusOrderBtn = document.getElementById('cusOrder');
    const orderForm = document.getElementById('orderForm');
    const loadingDiv = document.getElementById('loadingSpinner'); 
    const errorMessage = document.getElementById('errorMessage');
    const ordersContainer = document.getElementById('ordersContainer');
    const customerNameDiv = document.getElementById('customerName');
    const ordersListDiv = document.getElementById('ordersList');

    // Mở modal khi click "Your Order"
    cusOrderBtn.addEventListener('click', function (e) {
        e.preventDefault();
        modal.show();
        orderForm.reset();
        hideMessage();
        ordersContainer.classList.add('d-none'); 
    });

    // Xử lý form submit
    orderForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const orderCode = document.getElementById('orderCode').value.trim();

        if (!orderCode) {
            showError('Vui lòng nhập mã đơn hàng');
            return;
        }

        searchOrders(orderCode);
    });

    function searchOrders(orderCode) {
        showLoading();
        hideMessage();
        ordersContainer.classList.add('d-none');

        fetch('http://localhost:5014/OrderApi/GetOrdersByOrderCode', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ orderCode: orderCode })
        })
            .then(response => {
                console.log('Response status:', response.status);
                return response.json();
            })
            .then(data => {
                console.log('Response data:', data);
                hideLoading();

                if (data.success) {
                    displayOrders(data.customerName, data.orders);
                } else {
                    showError(data.message || 'Không tìm thấy đơn hàng');
                }
            })
            .catch(error => {
                console.error('Fetch error:', error);
                hideLoading();
                showError('Có lỗi xảy ra khi tìm kiếm đơn hàng. Vui lòng kiểm tra kết nối mạng.');
            });
    }

    function displayOrders(customerName, orders) {
        console.log('Displaying orders:', customerName, orders);

        customerNameDiv.textContent = `Đơn hàng của: ${customerName}`;
        ordersListDiv.innerHTML = '';

        if (!orders || orders.length === 0) {
            ordersListDiv.innerHTML = '<p class="text-muted">Không có đơn hàng nào.</p>';
            ordersContainer.classList.remove('d-none');
            return;
        }

        orders.forEach(order => {
            const orderDiv = document.createElement('div');
            orderDiv.className = 'card mb-3 order-item';

            const statusClass = order.status && order.status.toLowerCase() === 'pending' ? 'badge bg-warning text-dark' : 'badge bg-success';

            let itemsHtml = '';
            if (order.items && order.items.length > 0) {
                order.items.forEach(item => {
                    const itemName = item.productName || item.comboName || 'Sản phẩm';
                    itemsHtml += `
                        <div class="d-flex justify-content-between align-items-center border-bottom py-2">
                            <span>${itemName} x${item.quantity}</span>
                            <span class="fw-bold">${formatCurrency(item.unitPrice * item.quantity)}</span>
                        </div>
                    `;
                });
            }

            orderDiv.innerHTML = `
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h6 class="card-title mb-0">Mã đơn: ${order.orderCode}</h6>
                        <span class="${statusClass}">${order.status || 'Đang xử lý'}</span>
                    </div>
                    <p class="text-muted small mb-3">Thời gian đặt: ${formatDate(order.createdAt)}</p>
                    <div class="order-items mb-3">
                        ${itemsHtml}
                    </div>
                    <div class="d-flex justify-content-between align-items-center">
                        <strong>Tổng tiền:</strong>
                        <strong class="text-danger">${formatCurrency(order.totalAmount)}</strong>
                    </div>
                </div>
            `;

            ordersListDiv.appendChild(orderDiv);
        });

        ordersContainer.classList.remove('d-none');
    }

    function formatCurrency(amount) {
        if (!amount && amount !== 0) return '0 ₫';
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
            minimumFractionDigits: 0
        }).format(amount);
    }

    function formatDate(dateString) {
        if (!dateString) return 'Không xác định';
        try {
            const date = new Date(dateString);
            return date.toLocaleDateString('vi-VN', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit'
            });
        } catch (error) {
            console.error('Date formatting error:', error);
            return 'Không xác định';
        }
    }

    function showLoading() {
        loadingDiv.classList.remove('d-none');
    }

    function hideLoading() {
        loadingDiv.classList.add('d-none');
    }

    function showError(message) {
        errorMessage.textContent = message;
        errorMessage.classList.remove('d-none');
    }

    function hideMessage() {
        errorMessage.classList.add('d-none');
    }
});
