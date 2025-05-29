document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('phoneModal');
    const cusOrderBtn = document.getElementById('cusOrder');
    const closeBtn = document.querySelector('.close');
    const phoneForm = document.getElementById('phoneForm');
    const loadingDiv = document.querySelector('.loading');
    const errorMessage = document.getElementById('errorMessage');
    const ordersContainer = document.getElementById('ordersContainer');
    const customerNameDiv = document.getElementById('customerName');
    const ordersListDiv = document.getElementById('ordersList');

    cusOrderBtn.addEventListener('click', function (e) {
        e.preventDefault();
        modal.style.display = 'block';
        phoneForm.reset();
        hideMessage();
        ordersContainer.style.display = 'none';
    });

    closeBtn.addEventListener('click', function () {
        modal.style.display = 'none';
    });

    window.addEventListener('click', function (e) {
        if (e.target === modal) {
            modal.style.display = 'none';
        }
    });

    phoneForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const phoneNumber = document.getElementById('phoneNumber').value.trim();

        if (!phoneNumber) {
            showError('Vui lòng nhập số điện thoại');
            return;
        }

        searchOrders(phoneNumber);
    });

    function searchOrders(phoneNumber) {
        showLoading();
        hideMessage();
        ordersContainer.style.display = 'none';

        fetch('/Order/GetOrdersByPhone', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ phoneNumber: phoneNumber })
        })
            .then(response => response.json())
            .then(data => {
                hideLoading();

                if (data.success) {
                    displayOrders(data.customerName, data.orders);
                } else {
                    showError(data.message || 'Không tìm thấy đơn hàng');
                }
            })
            .catch(error => {
                hideLoading();
                showError('Có lỗi xảy ra khi tìm kiếm đơn hàng');
                console.error('Error:', error);
            });
    }

    function displayOrders(customerName, orders) {
        customerNameDiv.textContent = `Đơn hàng của: ${customerName}`;
        ordersListDiv.innerHTML = '';

        orders.forEach(order => {
            const orderDiv = document.createElement('div');
            orderDiv.className = 'order-item';

            const statusClass = order.status.toLowerCase() === 'pending' ? 'status-pending' : 'status-completed';

            let itemsHtml = '';
            order.items.forEach(item => {
                const itemName = item.productName || item.comboName || 'Sản phẩm';
                itemsHtml += `
                            <div class="item">
                                <span>${itemName} x${item.quantity}</span>
                                <span>${formatCurrency(item.unitPrice * item.quantity)}</span>
                            </div>
                        `;
            });

            orderDiv.innerHTML = `
                        <div class="order-header">
                            <div class="order-code">Mã đơn: ${order.orderCode}</div>
                            <div class="order-status ${statusClass}">${order.status}</div>
                        </div>
                        <div class="order-time">Thời gian đặt: ${formatDate(order.createdAt)}</div>
                        <div class="order-items">
                            ${itemsHtml}
                        </div>
                        <div class="total-amount">
                            Tổng tiền: ${formatCurrency(order.totalAmount)}
                        </div>
                    `;

            ordersListDiv.appendChild(orderDiv);
        });

        ordersContainer.style.display = 'block';
    }

    function formatCurrency(amount) {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
            minimumFractionDigits: 0
        }).format(amount);
    }

    function formatDate(dateString) {
        const date = new Date(dateString);
        return date.toLocaleDateString('vi-VN', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    function showLoading() {
        loadingDiv.style.display = 'block';
    }

    function hideLoading() {
        loadingDiv.style.display = 'none';
    }

    function showError(message) {
        errorMessage.textContent = message;
        errorMessage.style.display = 'block';
    }

    function hideMessage() {
        errorMessage.style.display = 'none';
    }
});