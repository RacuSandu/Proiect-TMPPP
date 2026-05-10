const API_BASE = 'http://localhost:5071/api';

async function apiGet(endpoint) {
    const res = await fetch(`${API_BASE}/${endpoint}`);
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}

async function apiPost(endpoint, data) {
    const res = await fetch(`${API_BASE}/${endpoint}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}

async function apiPut(endpoint) {
    const res = await fetch(`${API_BASE}/${endpoint}`, { method: 'PUT' });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}

async function apiDelete(endpoint) {
    const res = await fetch(`${API_BASE}/${endpoint}`, { method: 'DELETE' });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}

function showToast(message, type = 'success') {
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.textContent = message;
    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 3000);
}

function showAddModal() {
    document.getElementById('addModal').classList.remove('hidden');
}

function hideModal() {
    document.getElementById('addModal').classList.add('hidden');
}

function getStatusBadge(status) {
    const map = {
        'Available':        '<span class="badge badge-success">Disponibil</span>',
        'Rented':           '<span class="badge badge-warning">Inchiriat</span>',
        'UnderMaintenance': '<span class="badge badge-danger">Mentenanta</span>',
        'Active':           '<span class="badge badge-success">Activ</span>',
        'Completed':        '<span class="badge badge-info">Finalizat</span>',
        'Cancelled':        '<span class="badge badge-danger">Anulat</span>',
        'Pending':          '<span class="badge badge-warning">In asteptare</span>',
    };
    return map[status] || `<span class="badge">${status}</span>`;
}