const API_BASE = 'http://localhost:5071/api';
const PEXELS_KEY = 'g55YRdyCQeMfIcujxQGPSvuTP5veICO5yVdkIV9HHwTQuKcNE3c2G7IQ';

async function getVehicleImage(brand, model, type, year) {
    try {
        // Query cu brand + model + an + tip
        const queries = [
            `${year} ${brand} ${model}`,      // prima incercare: cu an
            `${brand} ${model}`,               // a doua: fara an
            `${brand} ${type}`,                // a treia: doar brand + tip
        ];

        for (const query of queries) {
            const res = await fetch(
                `https://api.pexels.com/v1/search?query=${encodeURIComponent(query)}&per_page=5&orientation=landscape`,
                { headers: { 'Authorization': PEXELS_KEY } }
            );

            if (!res.ok) continue;
            const data = await res.json();

            if (data.photos && data.photos.length > 0) {
                // Alege poza random din primele 5 rezultate
                const random = Math.floor(Math.random() * data.photos.length);
                return data.photos[random].src.medium;
            }
        }

        return await getVehicleImageByType(type);

    } catch {
        return getDefaultImage(type);
    }
}

async function getVehicleImageByType(type) {
    try {
        const queries = {
            'Car':        'sedan car',
            'Truck':      'truck vehicle',
            'Motorcycle': 'motorcycle bike'
        };
        const query = queries[type] || 'car vehicle';
        const res = await fetch(
            `https://api.pexels.com/v1/search?query=${encodeURIComponent(query)}&per_page=1&orientation=landscape`,
            { headers: { 'Authorization': PEXELS_KEY } }
        );
        if (!res.ok) return getDefaultImage(type);
        const data = await res.json();
        if (data.photos && data.photos.length > 0)
            return data.photos[0].src.medium;
        return getDefaultImage(type);
    } catch {
        return getDefaultImage(type);
    }
}

function getDefaultImage(type) {
    const defaults = {
        'Car':        'https://images.pexels.com/photos/170811/pexels-photo-170811.jpeg?w=400',
        'Truck':      'https://images.pexels.com/photos/1638459/pexels-photo-1638459.jpeg?w=400',
        'Motorcycle': 'https://images.pexels.com/photos/2611686/pexels-photo-2611686.jpeg?w=400'
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