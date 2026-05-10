async function loadCustomers() {
    try {
        const customers = await apiGet('customers');
        const tbody = document.getElementById('customersTable');
        tbody.innerHTML = customers.map(c => `
            <tr>
                <td><strong>${c.fullName}</strong></td>
                <td>${c.email}</td>
                <td>${c.phone}</td>
                <td>${c.driverLicenseNumber}</td>
                <td><span class="badge badge-info">${c.rentalCount} inchirieri</span></td>
            </tr>
        `).join('');
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function addCustomer() {
    try {
        await apiPost('customers', {
            firstName:           document.getElementById('firstName').value,
            lastName:            document.getElementById('lastName').value,
            email:               document.getElementById('email').value,
            phone:               document.getElementById('phone').value,
            driverLicenseNumber: document.getElementById('license').value,
            dateOfBirth:         document.getElementById('dob').value
        });
        showToast('Client adaugat cu succes!');
        hideModal();
        loadCustomers();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

loadCustomers();