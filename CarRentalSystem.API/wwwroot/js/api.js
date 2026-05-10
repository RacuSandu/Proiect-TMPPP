const API_BASE = 'http://localhost:5071/api';
const UNSPLASH_KEY = 'BhMjBAXe9NHCwu7JtZP94J1r3pZe_hwXznJnXFkNT7M';

async function getVehicleImage(brand, model, type) {
    try {
        const query = `${brand} ${model} ${type} car`;
        const res = await fetch(
            `https://api.unsplash.com/photos/random?query=${encodeURIComponent(query)}&orientation=landscape&client_id=${UNSPLASH_KEY}`
        );
        if (!res.ok) return getDefaultImage(type);
        const data = await res.json();
        return data.urls.small;
    } catch {
        return getDefaultImage(type);
    }
}

function getDefaultImage(type) {
    const defaults = {
        'Car':        'https://images.unsplash.com/photo-1494976388531-d1058494cdd8?w=400',
        'Truck':      'https://images.unsplash.com/photo-1601584115197-04ecc0da31d7?w=400',
        'Motorcycle': 'https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=400'
    };
    return defaults[type] || defaults['Car'];
}

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