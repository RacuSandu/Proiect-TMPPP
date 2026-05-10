let allVehicles = [];
let currentFilter = 'all';

async function loadVehicles() {
    try {
        allVehicles = await apiGet('vehicles');
        renderVehicles();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

function renderVehicles() {
    const filtered = currentFilter === 'all'
        ? allVehicles
        : allVehicles.filter(v => v.type === currentFilter);

    const tbody = document.getElementById('vehiclesTable');
    tbody.innerHTML = filtered.map(v => `
        <tr>
            <td>${v.brand}</td>
            <td>${v.model}</td>
            <td>${v.type}</td>
            <td>${v.year}</td>
            <td>${v.licensePlate}</td>
            <td><strong>${v.dailyRate} MDL</strong></td>
            <td>${getStatusBadge(v.status)}</td>
            <td>
                ${v.isAvailable
                    ? `<button class="btn btn-danger btn-sm" onclick="deleteVehicle('${v.id}')">Sterge</button>`
                    : '<span style="color:#6c757d;font-size:0.85rem">Inchiriat</span>'}
            </td>
        </tr>
    `).join('');
}

function filterVehicles(type) {
    currentFilter = type;
    document.querySelectorAll('.btn-filter').forEach(b => b.classList.remove('active'));
    event.target.classList.add('active');
    renderVehicles();
}

async function deleteVehicle(id) {
    if (!confirm('Esti sigur ca vrei sa stergi acest vehicul?')) return;
    try {
        await apiDelete(`vehicles/${id}`);
        showToast('Vehicul sters cu succes!');
        loadVehicles();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

function switchTab(type) {
    document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
    event.target.classList.add('active');
    document.getElementById('carForm').classList.add('hidden');
    document.getElementById('truckForm').classList.add('hidden');
    document.getElementById('motorcycleForm').classList.add('hidden');
    document.getElementById(`${type}Form`).classList.remove('hidden');
}

async function addCar() {
    try {
        await apiPost('vehicles/car', {
            brand:        document.getElementById('carBrand').value,
            model:        document.getElementById('carModel').value,
            year:         parseInt(document.getElementById('carYear').value),
            licensePlate: document.getElementById('carPlate').value,
            fuelType:     document.getElementById('carFuel').value,
            dailyRate:    parseFloat(document.getElementById('carRate').value),
            doors:        parseInt(document.getElementById('carDoors').value),
            transmission: document.getElementById('carTransmission').value,
            hasAC:        document.getElementById('carAC').value === 'true'
        });
        showToast('Masina adaugata cu succes!');
        hideModal();
        loadVehicles();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function addTruck() {
    try {
        await apiPost('vehicles/truck', {
            brand:                  document.getElementById('truckBrand').value,
            model:                  document.getElementById('truckModel').value,
            year:                   parseInt(document.getElementById('truckYear').value),
            licensePlate:           document.getElementById('truckPlate').value,
            fuelType:               document.getElementById('truckFuel').value,
            dailyRate:              parseFloat(document.getElementById('truckRate').value),
            cargoCapacity:          parseFloat(document.getElementById('truckCargo').value),
            requiresSpecialLicense: document.getElementById('truckLicense').value === 'true'
        });
        showToast('Camion adaugat cu succes!');
        hideModal();
        loadVehicles();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function addMotorcycle() {
    try {
        await apiPost('vehicles/motorcycle', {
            brand:          document.getElementById('motoBrand').value,
            model:          document.getElementById('motoModel').value,
            year:           parseInt(document.getElementById('motoYear').value),
            licensePlate:   document.getElementById('motoPlate').value,
            fuelType:       document.getElementById('motoFuel').value,
            dailyRate:      parseFloat(document.getElementById('motoRate').value),
            motorcycleType: document.getElementById('motoType').value,
            engineCC:       parseInt(document.getElementById('motoCC').value)
        });
        showToast('Motocicleta adaugata cu succes!');
        hideModal();
        loadVehicles();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

loadVehicles();