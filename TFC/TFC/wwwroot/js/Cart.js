/*========================================= GLOBAL VARIABLES =========================================*/

// Cart and inventory management
let cart = [];
let inventoryData = {};
let comboItems = {};

// Timer management for payment flow
let paymentTimer = null;
let loadingTimer = null;
let countdownInterval = null;

// Modal instances
let cartModal = null;
let customerModal = null;


/*========================================= INITIALIZATION =========================================*/

document.addEventListener('DOMContentLoaded', function () {
    // Initialize Bootstrap modals
    cartModal = new bootstrap.Modal(document.getElementById('cart-modal'));
    customerModal = new bootstrap.Modal(document.getElementById('customer-modal'));

    // Load initial data from page
    loadInventoryFromPage();
    loadComboItemsFromPage();

    // Set up form event handlers
    setupFormHandlers();

    // Add custom CSS animations
    addCustomStyles();
});

function setupFormHandlers() {
    const customerForm = document.getElementById('customer-form');
    if (customerForm) {
        customerForm.addEventListener('submit', function (e) {
            e.preventDefault();
            if (!validateForm()) {
                return;
            }
            showPaymentQR();
        });
    }
}


/*========================================= DATA LOADING =========================================*/

function loadInventoryFromPage() {
    const productCards = document.querySelectorAll('.product-card');
    productCards.forEach(card => {
        const button = card.querySelector('button[onclick*="addToCart"]');
        if (button && !button.disabled) {
            const onclick = button.getAttribute('onclick');
            const matches = onclick.match(/addToCart\('([^']+)',\s*'[^']*',\s*\d+,\s*'[^']*',\s*'([^']*)'\)/);
            if (matches) {
                const id = matches[1];
                const type = matches[2];

                // Extract inventory from small elements
                const smallElements = card.querySelectorAll('small');
                let inventory = null;

                smallElements.forEach(small => {
                    if (small.textContent.includes('Còn lại:')) {
                        const inventoryText = small.textContent;
                        const inventoryMatch = inventoryText.match(/Còn lại:\s*(\d+)/);
                        if (inventoryMatch) {
                            inventory = parseInt(inventoryMatch[1]);
                        }
                    }
                });

                // Handle combo products (unlimited inventory)
                if (inventory === null && type === 'combo') {
                    const availableButton = card.querySelector('button:not([disabled])');
                    inventory = availableButton ? 999 : 0;
                }

                if (inventory !== null) {
                    inventoryData[`${type}_${id}`] = inventory;
                }
            }
        }
    });
}

function loadComboItemsFromPage() {
    const comboCards = document.querySelectorAll('.product-card');
    comboCards.forEach(card => {
        const button = card.querySelector('button[onclick*="addToCart"]');
        if (button && !button.disabled) {
            const onclick = button.getAttribute('onclick');
            const matches = onclick.match(/addToCart\('([^']+)',\s*'[^']*',\s*\d+,\s*'[^']*',\s*'([^']*)'\)/);
            if (matches && matches[2] === 'combo') {
                const comboId = matches[1];
                const itemsAttr = button.getAttribute('data-items');
                if (itemsAttr) {
                    const items = {};
                    itemsAttr.split(',').forEach(item => {
                        const [productId, quantity] = item.split(':');
                        items[`product_${productId}`] = parseInt(quantity);
                    });
                    comboItems[`combo_${comboId}`] = items;
                }
            }
        }
    });
}


/*========================================= INVENTORY VALIDATION =========================================*/

function canAddComboToCart(comboUniqueId, quantityToAdd = 1) {
    const items = comboItems[comboUniqueId];
    if (!items) {
        return true;
    }

    // Calculate current item usage in cart
    const comboItemUsage = {};

    // Count individual products in cart
    cart.forEach(item => {
        if (item.type === 'product') {
            const productKey = item.uniqueId;
            comboItemUsage[productKey] = (comboItemUsage[productKey] || 0) + item.quantity;
        }
    });

    // Count combo items in cart
    cart.forEach(item => {
        if (item.type === 'combo') {
            const comboItemList = comboItems[item.uniqueId];
            if (comboItemList) {
                Object.entries(comboItemList).forEach(([productKey, neededQuantity]) => {
                    comboItemUsage[productKey] = (comboItemUsage[productKey] || 0) + (neededQuantity * item.quantity);
                });
            }
        }
    });

    // Validate availability for new combo
    for (const [productKey, neededQuantity] of Object.entries(items)) {
        const currentUsage = comboItemUsage[productKey] || 0;
        const availableInventory = inventoryData[productKey] || 0;
        const requiredForNewCombo = neededQuantity * quantityToAdd;

        if (currentUsage + requiredForNewCombo > availableInventory) {
            return false;
        }
    }

    return true;
}

function getInsufficientitems(comboUniqueId, quantityToAdd = 1) {
    const items = comboItems[comboUniqueId];
    if (!items) {
        return [];
    }

    const insufficientitems = [];
    const comboItemUsage = {};

    // Calculate current usage
    cart.forEach(item => {
        if (item.type === 'product') {
            const productKey = item.uniqueId;
            comboItemUsage[productKey] = (comboItemUsage[productKey] || 0) + item.quantity;
        }
    });

    cart.forEach(item => {
        if (item.type === 'combo') {
            const comboItemList = comboItems[item.uniqueId];
            if (comboItemList) {
                Object.entries(comboItemList).forEach(([productKey, neededQuantity]) => {
                    comboItemUsage[productKey] = (comboItemUsage[productKey] || 0) + (neededQuantity * item.quantity);
                });
            }
        }
    });

    // Check each item in combo
    for (const [productKey, neededQuantity] of Object.entries(items)) {
        const currentUsage = comboItemUsage[productKey] || 0;
        const availableInventory = inventoryData[productKey] || 0;
        const requiredForNewCombo = neededQuantity * quantityToAdd;

        if (currentUsage + requiredForNewCombo > availableInventory) {
            const productName = getProductNameFromKey(productKey);
            insufficientitems.push({
                name: productName,
                needed: requiredForNewCombo,
                available: Math.max(0, availableInventory - currentUsage)
            });
        }
    }

    return insufficientitems;
}

function getProductNameFromKey(productKey) {
    const productCards = document.querySelectorAll('.product-card');
    for (const card of productCards) {
        const button = card.querySelector('button[onclick*="addToCart"]');
        if (button) {
            const onclick = button.getAttribute('onclick');
            const matches = onclick.match(/addToCart\('([^']+)',\s*'([^']*)',\s*\d+,\s*'[^']*',\s*'([^']*)'\)/);
            if (matches) {
                const id = matches[1];
                const name = matches[2];
                const type = matches[3] || 'product';
                if (`${type}_${id}` === productKey) {
                    return name;
                }
            }
        }
    }
    return productKey;
}

function validateInventoryBeforeCheckout() {
    // Check inventory for individual products
    for (const item of cart) {
        if (item.type === 'product') {
            const availableInventory = inventoryData[item.uniqueId];
            if (item.quantity > availableInventory) {
                showErrorToast(`Số lượng "${item.name}" trong giỏ hàng (${item.quantity}) vượt quá tồn kho (${availableInventory}). Vui lòng điều chỉnh.`);
                return false;
            }
        }
    }

    // Check inventory for combo items
    for (const item of cart) {
        if (item.type === 'combo') {
            if (!canAddComboToCart(item.uniqueId, item.quantity)) {
                const insufficientitems = getInsufficientitems(item.uniqueId, item.quantity);
                if (insufficientitems.length > 0) {
                    const itemNames = insufficientitems.map(ing => ing.name).join(', ');
                    showErrorToast(`Combo "${item.name}" không thể đặt vì thiếu nguyên liệu: ${itemNames}`);
                    return false;
                }
            }
        }
    }

    return true;
}


/*========================================= CART MANAGEMENT =========================================*/

function addToCart(id, name, price, description, type = 'product') {
    const uniqueId = `${type}_${id}`;

    // Validate inventory availability
    const availableInventory = inventoryData[uniqueId];
    if (availableInventory === undefined) {
        showErrorToast('Không thể xác định số lượng tồn kho của sản phẩm này');
        return;
    }

    if (availableInventory <= 0) {
        showErrorToast('Sản phẩm này đã hết hàng');
        return;
    }

    // Special validation for combo products
    if (type === 'combo') {
        if (!canAddComboToCart(uniqueId, 1)) {
            const insufficientitems = getInsufficientitems(uniqueId, 1);
            if (insufficientitems.length > 0) {
                const itemNames = insufficientitems.map(ing => ing.name).join(', ');
                showErrorToast(`Không thể thêm combo "${name}" vì không đủ nguyên liệu: ${itemNames}`);
                return;
            }
        }
    }

    // Check current quantity in cart
    const existingItem = cart.find(item => item.uniqueId === uniqueId);
    const currentQuantityInCart = existingItem ? existingItem.quantity : 0;

    if (type === 'product' && currentQuantityInCart >= availableInventory) {
        showErrorToast(`Chỉ còn ${availableInventory} sản phẩm trong kho. Bạn đã thêm đủ số lượng có thể.`);
        return;
    }

    // Add or update item in cart
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

    // Update displays and show confirmation
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
    if (item) {
        const newQuantity = item.quantity + change;

        // Validate quantity increase
        if (change > 0) {
            if (item.type === 'combo') {
                if (!canAddComboToCart(uniqueId, change)) {
                    const insufficientitems = getInsufficientitems(uniqueId, change);
                    if (insufficientitems.length > 0) {
                        const itemNames = insufficientitems.map(ing => ing.name).join(', ');
                        showErrorToast(`Không thể tăng số lượng combo vì không đủ nguyên liệu: ${itemNames}`);
                        return;
                    }
                }
            } else {
                const availableInventory = inventoryData[uniqueId];
                if (newQuantity > availableInventory) {
                    showErrorToast(`Chỉ còn ${availableInventory} sản phẩm trong kho`);
                    return;
                }
            }
        }

        item.quantity = newQuantity;

        // Remove item if quantity becomes 0 or negative
        if (item.quantity <= 0) {
            removeFromCart(uniqueId);
        } else {
            updateCartDisplay();
            updateCartCount();
        }
    }
}

function clearCart() {
    cart = [];
    updateCartDisplay();
    updateCartCount();
}


/*========================================= CART DISPLAY =========================================*/

function updateCartCount() {
    const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
    const cartCountElement = document.getElementById('cart-count');
    if (cartCountElement) {
        cartCountElement.textContent = totalItems;
        cartCountElement.style.display = totalItems === 0 ? 'none' : 'inline-block';
    }
}

function updateCartDisplay() {
    const cartItemsContainer = document.getElementById('cart-items');
    const cartFooter = document.getElementById('cart-total');
    const totalAmount = document.getElementById('total-amount');

    if (cart.length === 0) {
        cartItemsContainer.innerHTML = `
            <div class="empty-cart text-center py-5">
                <p class="text-muted fs-5">Giỏ hàng trống</p>
            </div>
        `;
        cartFooter.classList.add('d-none');
    } else {
        let itemsHTML = '';
        let total = 0;

        cart.forEach(item => {
            const itemTotal = item.price * item.quantity;
            total += itemTotal;

            // Determine if quantity can be increased
            let canIncrease = false;
            let inventoryInfo = '';

            if (item.type === 'combo') {
                canIncrease = canAddComboToCart(item.uniqueId, 1);
                inventoryInfo = 'Combo';
            } else {
                const availableInventory = inventoryData[item.uniqueId] || 0;
                canIncrease = item.quantity < availableInventory;
                inventoryInfo = `Còn lại: ${availableInventory}`;
            }

            itemsHTML += `
                <div class="cart-item d-flex justify-content-between align-items-center py-3 border-bottom">
                    <div class="cart-item-info flex-grow-1">
                        <h6 class="cart-item-name mb-1 fw-bold">${item.name}</h6>
                        <small class="cart-item-price text-danger fw-bold">${item.price.toLocaleString()} đ</small>
                        <br><small class="text-muted">${inventoryInfo}</small>
                    </div>
                    <div class="quantity-controls d-flex align-items-center">
                        <button class="btn btn-outline-secondary btn-sm" onclick="updateQuantity('${item.uniqueId}', -1)">-</button>
                        <span class="mx-3 fw-bold">${item.quantity}</span>
                        <button class="btn btn-outline-secondary btn-sm" onclick="updateQuantity('${item.uniqueId}', 1)" ${!canIncrease ? 'disabled title="Không thể tăng thêm"' : ''}>+</button>
                        <button class="btn btn-outline-danger btn-sm ms-2" onclick="removeFromCart('${item.uniqueId}')">
                            <i class="bi bi-trash"></i> Xóa
                        </button>
                    </div>
                </div>
            `;
        });

        cartItemsContainer.innerHTML = itemsHTML;
        totalAmount.textContent = total.toLocaleString();
        cartFooter.classList.remove('d-none');
    }
}

function toggleCart() {
    if (cartModal) {
        cartModal.toggle();
    }
}


/*========================================= CUSTOMER FORM =========================================*/


function showCustomerForm() {
    if (cart.length === 0) {
        alert('Giỏ hàng trống!');
        return;
    }

    if (!validateInventoryBeforeCheckout()) {
        return;
    }

    resetModalStates();
    updateOrderSummary();

    if (cartModal) {
        cartModal.hide();
    }
    if (customerModal) {
        customerModal.show();
    }
}

function closeCustomerForm() {
    clearPaymentTimers();

    if (customerModal) {
        customerModal.hide();
    }

    const customerForm = document.getElementById('customer-form');
    if (customerForm) {
        customerForm.reset();
    }

    clearErrors();
    resetModalStates();
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
            <div class="order-item d-flex justify-content-between align-items-center py-2">
                <span>${item.name} x${item.quantity}</span>
                <span class="fw-bold text-danger">${itemTotal.toLocaleString()} đ</span>
            </div>
        `;
    });

    summaryContainer.innerHTML = summaryHTML;
    totalAmountSpan.textContent = total.toLocaleString();
}


/*========================================= FORM VALIDATION =========================================*/

function validateForm() {
    clearErrors();
    let isValid = true;

    const name = document.getElementById('customer-name').value.trim();
    const email = document.getElementById('customer-email').value.trim();
    const nameInput = document.getElementById('customer-name');
    const emailInput = document.getElementById('customer-email');

    if (!name) {
        document.getElementById('name-error').textContent = 'Vui lòng nhập họ tên';
        nameInput.classList.add('is-invalid');
        isValid = false;
    }

    if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        document.getElementById('email-error').textContent = 'Email không hợp lệ';
        emailInput.classList.add('is-invalid');
        isValid = false;
    }

    return isValid;
}

function clearErrors() {
    const nameError = document.getElementById('name-error');
    const emailError = document.getElementById('email-error');
    const nameInput = document.getElementById('customer-name');
    const emailInput = document.getElementById('customer-email');

    if (nameError) nameError.textContent = '';
    if (emailError) emailError.textContent = '';
    if (nameInput) nameInput.classList.remove('is-invalid');
    if (emailInput) emailInput.classList.remove('is-invalid');
}


/*========================================= PAYMENT FLOW =========================================*/

function createPaymentQRElement() {
    const existingQR = document.getElementById('payment-qr');
    if (existingQR) {
        existingQR.remove();
    }

    const totalAmount = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const qrUrl = `https://img.vietqr.io/image/tpbank-00000104077-7s5kxFV.jpg?amount=${totalAmount}&addInfo=thanh%20toan%20TFC&accountName=NGUYEN%20LAM%20TUNG`;

    const paymentQR = document.createElement('div');
    paymentQR.id = 'payment-qr';
    paymentQR.className = 'payment-qr';
    paymentQR.style.cssText = `
        display: none;
        text-align: center;
        background: white;
        padding: 30px;
        border-radius: 10px;
        box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        margin-top: 20px;
    `;

    paymentQR.innerHTML = `
        <h3 class="text-primary mb-4">🏦 Quét mã QR để thanh toán</h3>
        <div id="payment-timer" class="fs-5 fw-bold text-danger mb-3">
            Thời gian còn lại: 10s
        </div>
        <img src="${qrUrl}" alt="QR Code thanh toán" class="img-fluid mb-3" style="max-width: 300px; border: 2px solid #ddd; border-radius: 10px;"/>
        <div class="text-muted mb-4">
            <p><strong>Số tiền:</strong> ${totalAmount.toLocaleString()} VNĐ</p>
            <p><strong>Nội dung:</strong> thanh toan TFC</p>
            <p>Vui lòng quét mã QR bằng ứng dụng ngân hàng để thanh toán</p>
        </div>
        <button class="btn btn-secondary" onclick="closeCustomerForm()">Hủy thanh toán</button>
    `;

    const modalBody = document.querySelector('#customer-modal .modal-body');
    modalBody.appendChild(paymentQR);
}

function showPaymentQR() {
    createPaymentQRElement();

    // Hide form elements
    const customerForm = document.getElementById('customer-form');
    const formActions = document.getElementById('form-actions');
    const orderSummary = document.querySelector('.order-summary');

    if (customerForm) customerForm.style.display = 'none';
    if (formActions) formActions.style.display = 'none';
    if (orderSummary) orderSummary.style.display = 'none';

    // Show QR payment
    const paymentQR = document.getElementById('payment-qr');
    if (paymentQR) {
        paymentQR.style.display = 'block';
    }

    startPaymentCountdown();

    // Auto proceed to loading after 10 seconds
    paymentTimer = setTimeout(() => {
        showCartLoading();
    }, 10000);
}

function hidePaymentQR() {
    const paymentQR = document.getElementById('payment-qr');
    if (paymentQR) {
        paymentQR.style.display = 'none';
    }
}

function startPaymentCountdown() {
    let timeLeft = 10;
    const timerElement = document.getElementById('payment-timer');

    if (timerElement) {
        timerElement.textContent = `Thời gian còn lại: ${timeLeft}s`;
    }

    countdownInterval = setInterval(() => {
        timeLeft--;
        if (timerElement) {
            timerElement.textContent = `Thời gian còn lại: ${timeLeft}s`;
        }

        if (timeLeft <= 0) {
            clearInterval(countdownInterval);
        }
    }, 1000);
}

function showCartLoading() {
    hidePaymentQR();
    const loadingElement = document.getElementById('loading');
    if (loadingElement) {
        loadingElement.classList.remove('d-none');
    }

    // Show success after 3 seconds
    loadingTimer = setTimeout(() => {
        processOrderWithSuccess();
    }, 3000);
}

function hideCartLoading() {
    const loadingElement = document.getElementById('loading');
    if (loadingElement) {
        loadingElement.classList.add('d-none');
    }
}
function hideSuccessMessage() {
    const successMessage = document.getElementById('success-message');
    if (successMessage) {
        successMessage.classList.add('d-none');
    }
}

function clearPaymentTimers() {
    if (paymentTimer) {
        clearTimeout(paymentTimer);
        paymentTimer = null;
    }
    if (loadingTimer) {
        clearTimeout(loadingTimer);
        loadingTimer = null;
    }
    if (countdownInterval) {
        clearInterval(countdownInterval);
        countdownInterval = null;
    }
}

function resetModalStates() {
    const customerForm = document.getElementById('customer-form');
    const formActions = document.getElementById('form-actions');
    const orderSummary = document.querySelector('.order-summary');
    if (customerForm) {
        customerForm.style.display = 'block';
    }
    if (formActions) {
        formActions.style.display = 'flex';
    }
    if (orderSummary) {
        orderSummary.style.display = 'block'; 
    }

    hidePaymentQR();
    hideCartLoading();
    hideSuccessMessage();
}

/*========================================= ORDER PROCESSING =========================================*/

function addProductToCart(id, name, price, description) {
    addToCart(id, name, price, description, 'product');
}

function addComboToCart(id, name, price, description) {
    addToCart(id, name, price, description, 'combo');
}

/*==================== ORDER PROCESSING FUNCTIONS ====================*/

async function processOrderWithSuccess() {
    const orderData = {
        customer: {
            name: document.getElementById('customer-name').value.trim(),
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
        const response = await fetch('http://localhost:5014/OrderApi/CreateOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(orderData)
        });

        const result = await response.json();

        if (response.ok && result.success) {
            updateInventoryAfterOrder();
            showSuccessMessage(result.orderCode);
            clearCart();
        } else {
            throw new Error(result.message || 'Có lỗi xảy ra khi đặt món');
        }
    } catch (error) {
        showErrorMessage('Không thể kết nối đến máy chủ. Vui lòng kiểm tra mạng và thử lại sau.');
    }
}

/*==================== INVENTORY MANAGEMENT FUNCTIONS ====================*/

function updateInventoryAfterOrder() {
    cart.forEach(item => {
        if (item.type === 'product') {
            // Update inventory for regular products
            if (inventoryData[item.uniqueId] !== undefined) {
                inventoryData[item.uniqueId] -= item.quantity;
                if (inventoryData[item.uniqueId] < 0) {
                    inventoryData[item.uniqueId] = 0;
                }
            }
        } else if (item.type === 'combo') {
            // Update inventory for combo items (decrease component products)
            const items = comboItems[item.uniqueId];
            if (items) {
                Object.entries(items).forEach(([productKey, neededQuantity]) => {
                    if (inventoryData[productKey] !== undefined) {
                        inventoryData[productKey] -= neededQuantity * item.quantity;
                        if (inventoryData[productKey] < 0) {
                            inventoryData[productKey] = 0;
                        }
                    }
                });
            }
        }
    });

    // Update UI to reflect new inventory levels
    updateInventoryDisplayOnPage();
}

function updateInventoryDisplayOnPage() {
    // Update inventory display for all affected products
    Object.keys(inventoryData).forEach(uniqueId => {
        const newInventory = inventoryData[uniqueId];

        // Find and update product cards on the page
        const productCards = document.querySelectorAll('.product-card');
        productCards.forEach(card => {
            const button = card.querySelector('button[onclick*="addToCart"]');
            if (button && !button.disabled) {
                const onclick = button.getAttribute('onclick');
                const matches = onclick.match(/addToCart\('([^']+)',\s*'[^']*',\s*\d+,\s*'[^']*',\s*'([^']*)'\)/);
                if (matches) {
                    const cardId = matches[1];
                    const cardType = matches[2] || 'product';

                    if (`${cardType}_${cardId}` === uniqueId) {
                        // Update inventory display for products
                        if (cardType === 'product') {
                            const inventoryElement = card.querySelector('small');
                            if (inventoryElement && inventoryElement.textContent.includes('Còn lại:')) {
                                inventoryElement.textContent = `Còn lại: ${newInventory}`;
                            }

                            // Disable button and add overlay if out of stock
                            if (newInventory <= 0) {
                                button.disabled = true;
                                button.textContent = 'Hết hàng';
                                button.className = 'btn btn-secondary w-100 mt-auto';

                                // Add out of stock overlay
                                const imgContainer = card.querySelector('.position-relative');
                                if (imgContainer && !imgContainer.querySelector('.position-absolute')) {
                                    const overlay = document.createElement('div');
                                    overlay.className = 'position-absolute top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center bg-dark bg-opacity-50';
                                    overlay.innerHTML = '<span class="badge bg-danger fs-6 px-3 py-2">HẾT HÀNG</span>';
                                    imgContainer.appendChild(overlay);
                                }

                                card.classList.add('out-of-stock');
                            }
                        }
                    }
                }
            }
        });
    });

    // Check and disable combo buttons if ingredients are insufficient
    const comboCards = document.querySelectorAll('.product-card');
    comboCards.forEach(card => {
        const button = card.querySelector('button[onclick*="addToCart"]');
        if (button && !button.disabled) {
            const onclick = button.getAttribute('onclick');
            const matches = onclick.match(/addToCart\('([^']+)',\s*'[^']*',\s*\d+,\s*'[^']*',\s*'([^']*)'\)/);
            if (matches && matches[2] === 'combo') {
                const comboId = matches[1];
                const comboUniqueId = `combo_${comboId}`;

                // Check if combo can be added to cart
                if (!canAddComboToCart(comboUniqueId, 1)) {
                    button.disabled = true;
                    button.textContent = 'Hết nguyên liệu';
                    button.className = 'btn btn-secondary w-100 mt-auto';

                    // Add out of ingredients overlay
                    const imgContainer = card.querySelector('.position-relative');
                    if (imgContainer && !imgContainer.querySelector('.position-absolute')) {
                        const overlay = document.createElement('div');
                        overlay.className = 'position-absolute top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center bg-dark bg-opacity-50';
                        overlay.innerHTML = '<span class="badge bg-warning fs-6 px-3 py-2">HẾT NGUYÊN LIỆU</span>';
                        imgContainer.appendChild(overlay);
                    }

                    card.classList.add('out-of-stock');
                }
            }
        }
    });
}

/*==================== UI FEEDBACK FUNCTIONS ====================*/

function showSuccessMessage(orderCode) {
    hideCartLoading();

    const successMessage = document.getElementById('success-message');
    const orderCodeElement = document.getElementById('order-code');
    const formActions = document.getElementById('form-actions');

    if (successMessage) {
        successMessage.classList.remove('d-none');
    }
    if (orderCodeElement) {
        orderCodeElement.textContent = orderCode;
    }
    if (formActions) {
        formActions.style.display = 'none';
    }
}

function showErrorMessage(message) {
    hideCartLoading();

    // Show error alert
    alert('Lỗi: ' + message);

    // Create error message element similar to success message
    const errorMessage = document.createElement('div');
    errorMessage.className = 'alert alert-danger mt-3';
    errorMessage.innerHTML = `
        <h5><i class="bi bi-exclamation-triangle-fill me-2"></i>Đặt hàng thất bại!</h5>
        <p class="mb-0">${message}</p>
        <button class="btn btn-outline-danger mt-2" onclick="closeCustomerForm()">Đóng</button>
    `;

    const modalBody = document.querySelector('#customer-modal .modal-body');
    modalBody.appendChild(errorMessage);
}

/*==================== TOAST NOTIFICATION FUNCTIONS ====================*/

function showAddToCartMessage(productName) {
    // Remove existing messages to prevent overlap
    const existingMessages = document.querySelectorAll('.cart-toast-message');
    existingMessages.forEach(msg => msg.remove());

    const message = document.createElement('div');
    message.className = 'cart-toast-message';
    message.innerHTML = `
        <div class="toast-body">
            <i class="bi bi-check-circle-fill text-success me-2"></i>
            Đã thêm "${productName}" vào giỏ hàng!
        </div>
    `;
    message.style.cssText = `
        position: fixed;
        top: 120px;
        right: 20px;
        background: #28a745;
        color: white;
        padding: 15px 20px;
        border-radius: 8px;
        z-index: 9999;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        animation: slideInRight 0.3s ease, fadeOut 0.3s ease 2.7s;
        font-weight: 500;
        max-width: 300px;
        word-wrap: break-word;
    `;

    document.body.appendChild(message);
    setTimeout(() => {
        if (message.parentNode) {
            message.remove();
        }
    }, 3000);
}

function showErrorToast(message) {
    // Remove existing messages to prevent overlap
    const existingMessages = document.querySelectorAll('.cart-toast-message');
    existingMessages.forEach(msg => msg.remove());

    const errorMessage = document.createElement('div');
    errorMessage.className = 'cart-toast-message';
    errorMessage.innerHTML = `
        <div class="toast-body">
            <i class="bi bi-exclamation-triangle-fill text-danger me-2"></i>
            ${message}
        </div>
    `;
    errorMessage.style.cssText = `
        position: fixed;
        top: 120px;
        right: 20px;
        background: #dc3545;
        color: white;
        padding: 15px 20px;
        border-radius: 8px;
        z-index: 9999;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        animation: slideInRight 0.3s ease, fadeOut 0.3s ease 3.7s;
        font-weight: 500;
        max-width: 300px;
        word-wrap: break-word;
    `;

    document.body.appendChild(errorMessage);
    setTimeout(() => {
        if (errorMessage.parentNode) {
            errorMessage.remove();
        }
    }, 4000);
}


/*==================== CSS STYLING ====================*/
function addCustomStyles() {
    const style = document.createElement('style');
    style.textContent = `
        @keyframes slideInRight {
            from {
                transform: translateX(100%);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }

        @keyframes fadeOut {
            from {
                opacity: 1;
            }
            to {
                opacity: 0;
            }
        }

        .cart-item:hover {
            background-color: #f8f9fa;
        }

        .order-item {
            border-bottom: 1px solid #e9ecef;
        }

        .order-item:last-child {
            border-bottom: none;
        }
    `;
    document.head.appendChild(style);
}
