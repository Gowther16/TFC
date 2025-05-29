let cart = [];

function addToCart(id, name, price, description, type = 'product') {
    const uniqueId = `${type}_${id}`;
    
    const existingItem = cart.find(item => item.uniqueId === uniqueId);
    // Check if existing item in the cart
    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cart.push({
            uniqueId: uniqueId,
            id: id,
            type: type, 
            name: name,
            price: price,
            description: description,
            quantity: 1
        });
    }

    updateCartDisplay();
    updateCartCount();
    showAddToCartMessage(name);
}

function removeFromCart(uniqueId) {
    cart = cart.filter(item => item.uniqueId !== uniqueId);
    updateCartDisplay();
    updateCartCount();
}

function updateQuantity(uniqueId, change) {
    const item = cart.find(item => item.uniqueId === uniqueId);
    // Check if item exists in the cart
    if (item) {
        item.quantity += change;
        // Ensure quantity not small than 0
        if (item.quantity <= 0) {
            removeFromCart(uniqueId);
        } else {
            updateCartDisplay();
            updateCartCount();
        }
    }
}

function updateCartCount() {
    const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
    document.getElementById('cart-count').textContent = totalItems;
}

function updateCartDisplay() {
    const cartItemsContainer = document.getElementById('cart-items');
    const cartTotal = document.getElementById('cart-total');
    const totalAmount = document.getElementById('total-amount');
    // Case cart empty
    if (cart.length === 0) {
        cartItemsContainer.innerHTML = '<div class="empty-cart"><p>Giỏ hàng trống</p></div>';
        cartTotal.style.display = 'none';
    } else {
        let itemsHTML = '';
        let total = 0;

        cart.forEach(item => {
            const itemTotal = item.price * item.quantity;
            total += itemTotal;

            itemsHTML += `
                <div class="cart-item">
                    <div class="cart-item-info">
                        <div class="cart-item-name">${item.name}</div>
                        <div class="cart-item-price">${item.price.toLocaleString()} đ</div>
                    </div>
                    <div class="quantity-controls">
                        <button class="quantity-btn" onclick="updateQuantity('${item.uniqueId}', -1)">-</button>
                        <span>${item.quantity}</span>
                        <button class="quantity-btn" onclick="updateQuantity('${item.uniqueId}', 1)">+</button>
                        <button class="btn btn-secondary" onclick="removeFromCart('${item.uniqueId}')" style="margin-left: 10px;">Xóa</button>
                    </div>
                </div>
            `;
        });

        cartItemsContainer.innerHTML = itemsHTML;
        totalAmount.textContent = total.toLocaleString();
        cartTotal.style.display = 'block';
    }
}
function toggleCart() {
    const cartModal = document.getElementById('cart-modal');
    cartModal.style.display = cartModal.style.display === 'none' || cartModal.style.display === '' ? 'block' : 'none';
}

function showCustomerForm() {
    // Check if cart is empty before showing customer form
    if (cart.length === 0) {
        alert('Giỏ hàng trống!');
        return;
    }
    updateOrderSummary();

    document.getElementById('customer-modal').style.display = 'block';
    document.getElementById('cart-modal').style.display = 'none';
}

function updateOrderSummary() {
    const summaryContainer = document.getElementById('order-summary-items');
    const totalAmountSpan = document.getElementById('order-total-amount');

    let summaryHTML = '';
    let total = 0;
    cart.forEach(item => {
        const itemTotal = item.price * item.quantity;
        total += itemTotal;
        summaryHTML += `
            <div class="order-item">
                <span> ${item.name} x${item.quantity}</span>
                <span>${itemTotal.toLocaleString()} đ</span>
            </div>
        `;
    });

    summaryContainer.innerHTML = summaryHTML;
    totalAmountSpan.textContent = total.toLocaleString();
}

function closeCustomerForm() {
    document.getElementById('customer-modal').style.display = 'none';
    document.getElementById('customer-form').reset();
    clearErrors();
    document.getElementById('customer-form').style.display = 'block';
    document.getElementById('loading').style.display = 'none';
    document.getElementById('success-message').style.display = 'none';
}

function clearCart() {
    cart = [];
    updateCartDisplay();
    updateCartCount();
}

function clearErrors() {
    document.getElementById('name-error').textContent = '';
    document.getElementById('phone-error').textContent = '';
    document.getElementById('email-error').textContent = '';
}

function validateForm() {
    clearErrors();
    let isValid = true;

    const name = document.getElementById('customer-name').value.trim();
    const phone = document.getElementById('customer-phone').value.trim();
    const email = document.getElementById('customer-email').value.trim();
    // User dont input name
    if (!name) {
        document.getElementById('name-error').textContent = 'Vui lòng nhập họ tên';
        isValid = false;
    }
    // User dont input phone
    if (!phone) {
        document.getElementById('phone-error').textContent = 'Vui lòng nhập số điện thoại';
        isValid = false
    }
    // Phone invalid
    else if (!/^[0-9]{10,11}$/.test(phone)) {
        document.getElementById('phone-error').textContent = 'Số điện thoại không hợp lệ';
        isValid = false;
    }
    // User input email invalid
    if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        document.getElementById('email-error').textContent = 'Email không hợp lệ';
        isValid = false;
    }

    return isValid;
}

document.getElementById('customer-form').addEventListener('submit', async function (e) {
    e.preventDefault();
    // Validate form before submitting
    if (!validateForm()) {
        return;
    }

    document.getElementById('customer-form').style.display = 'none';
    document.getElementById('loading').style.display = 'block';

    const orderData = {
        customer: {
            name: document.getElementById('customer-name').value.trim(),
            phone: document.getElementById('customer-phone').value.trim(),
            email: document.getElementById('customer-email').value.trim() || null
        },
        items: cart.map(item => ({
            id: item.id,
            type: item.type,
            name: item.name,
            price: item.price,
            quantity: item.quantity
        })),
        totalAmount: cart.reduce((sum, item) => sum + (item.price * item.quantity), 0)
    };

    try {
        const response = await fetch('/Order/CreateOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(orderData)
        });

        const result = await response.json();
        // Check if response is ok and result success
        if (response.ok && result.success) {
            document.getElementById('loading').style.display = 'none';
            document.getElementById('success-message').style.display = 'block';
            document.getElementById('order-code').textContent = result.orderCode;
            
            clearCart();
        } else {
            throw new Error(result.message || 'Có lỗi xảy ra khi đặt món');
        }
    } catch (error) {
        alert('Có lỗi xảy ra: ' + error.message);
        document.getElementById('loading').style.display = 'none';
        document.getElementById('customer-form').style.display = 'block';
    }
});

function showAddToCartMessage(productName) {
    const message = document.createElement('div');
    message.textContent = `Đã thêm "${productName}" vào giỏ hàng!`;
    message.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #28a745;
        color: white;
        padding: 10px 20px;
        border-radius: 5px;
        z-index: 3000;
        animation: slideIn 0.3s ease;
    `;

    document.body.appendChild(message);
    setTimeout(() => message.remove(), 3000);
}

document.getElementById('cart-modal').addEventListener('click', function (e) {
    if (e.target === this) toggleCart();
});

document.getElementById('customer-modal').addEventListener('click', function (e) {
    if (e.target === this) closeCustomerForm();
});

function addProductToCart(id, name, price, description) {
    addToCart(id, name, price, description, 'product');
}

function addComboToCart(id, name, price, description) {
    addToCart(id, name, price, description, 'combo');
}