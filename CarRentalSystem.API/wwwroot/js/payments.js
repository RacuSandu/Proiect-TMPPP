async function loadPayments() {
    try {
        const payments = await apiGet('payments');
        const tbody = document.getElementById('paymentsTable');
        tbody.innerHTML = payments.map(p => `
            <tr>
                <td><small>${p.paymentId.substring(0, 8)}...</small></td>
                <td><small>${p.contractId.substring(0, 8)}...</small></td>
                <td><strong>${p.amount} MDL</strong></td>
                <td>${p.method}</td>
                <td>${getStatusBadge(p.status)}</td>
                <td>${new Date(p.paymentDate).toLocaleDateString('ro-RO')}</td>
            </tr>
        `).join('');
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function showAddModal() {
    try {
        const contracts = await apiGet('contracts');
        const active = contracts.filter(c => c.status === 'Active');

        document.getElementById('paymentContract').innerHTML =
            active.map(c => `<option value="${c.contractId}">${c.contractId.substring(0, 8)}... - ${c.totalCost} MDL</option>`).join('');

        document.getElementById('addModal').classList.remove('hidden');
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function processPayment() {
    try {
        await apiPost('payments', {
            contractId: document.getElementById('paymentContract').value,
            amount:     parseFloat(document.getElementById('paymentAmount').value),
            method:     document.getElementById('paymentMethod').value
        });
        showToast('Plata procesata cu succes!');
        hideModal();
        loadPayments();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

loadPayments();