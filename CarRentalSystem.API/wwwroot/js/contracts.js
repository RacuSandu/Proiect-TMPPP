async function loadContracts() {
    try {
        const contracts = await apiGet('contracts');
        const tbody = document.getElementById('contractsTable');
        tbody.innerHTML = contracts.map(c => `
            <tr>
                <td><small>${c.contractId.substring(0, 8)}...</small></td>
                <td>${c.customerId}</td>
                <td>${c.vehicleId}</td>
                <td>${new Date(c.startDate).toLocaleDateString('ro-RO')}</td>
                <td>${new Date(c.endDate).toLocaleDateString('ro-RO')}</td>
                <td>${c.rentalDays} zile</td>
                <td><strong>${c.totalCost} MDL</strong></td>
                <td>${getStatusBadge(c.status)}</td>
                <td>
                    ${c.status === 'Active'
                        ? `<button class="btn btn-danger btn-sm" onclick="cancelContract('${c.contractId}')">Anuleaza</button>`
                        : ''}
                </td>
            </tr>
        `).join('');
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function showAddModal() {
    try {
        const [customers, vehicles] = await Promise.all([
            apiGet('customers'),
            apiGet('vehicles/available')
        ]);

        document.getElementById('contractCustomer').innerHTML =
            customers.map(c => `<option value="${c.id}">${c.fullName}</option>`).join('');

        document.getElementById('contractVehicle').innerHTML =
            vehicles.map(v => `<option value="${v.id}">${v.brand} ${v.model} - ${v.dailyRate} MDL/zi</option>`).join('');

        const today = new Date().toISOString().split('T')[0];
        document.getElementById('startDate').value = today;

        document.getElementById('addModal').classList.remove('hidden');
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function createContract() {
    try {
        const startDate = document.getElementById('startDate').value;
        const endDate   = document.getElementById('endDate').value;

        if (!endDate || endDate <= startDate) {
            showToast('Data sfarsit trebuie sa fie dupa data inceput!', 'error');
            return;
        }

        const days = Math.ceil((new Date(endDate) - new Date(startDate)) / (1000 * 60 * 60 * 24));

        await apiPost('contracts', {
            customerId: document.getElementById('contractCustomer').value,
            vehicleId:  document.getElementById('contractVehicle').value,
            startDate:  startDate,
            endDate:    endDate,
            days:       days
        });

        showToast('Contract creat cu succes!');
        hideModal();
        loadContracts();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function cancelContract(id) {
    if (!confirm('Esti sigur ca vrei sa anulezi acest contract?')) return;
    try {
        await apiPut(`contracts/${id}/cancel`);
        showToast('Contract anulat!');
        loadContracts();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

loadContracts();