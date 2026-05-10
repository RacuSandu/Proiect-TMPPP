let allVehicles = [];
let currentFilter = 'all';
let selectedVehicle = null;
let currentVehicleIndex = -1;
let currentImages = {}; // cache pentru poze

async function loadVehicles() {
    try {
        allVehicles = await apiGet('vehicles');
        await renderVehicles();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

async function renderVehicles() {
    const filtered = currentFilter === 'all'
        ? allVehicles
        : allVehicles.filter(v => (v.type || v.vehicleType) === currentFilter);

    const tbody = document.getElementById('vehiclesTable');
    tbody.innerHTML = '<tr><td colspan="9" style="text-align:center;padding:20px;color:#8b8d96;">Se incarca pozele...</td></tr>';

    const rows = await Promise.all(filtered.map(async v => {
        const type = v.type || v.vehicleType || 'Car';
        const imgUrl = await getVehicleImage(v.brand, v.model, type, v.year);
        return `
            <tr style="cursor:pointer;" onclick="showDetails('${v.id}', '${imgUrl}')">
                <td>
                    <img src="${imgUrl}" alt="${v.brand} ${v.model}"
                         style="width:90px;height:55px;object-fit:cover;border-radius:6px;">
                </td>
                <td>${v.brand}</td>
                <td>${v.model}</td>
                <td>${type}</td>
                <td>${v.year}</td>
                <td>${v.licensePlate}</td>
                <td><strong>${v.dailyRate} MDL</strong></td>
                <td>${getStatusBadge(v.status)}</td>
                <td onclick="event.stopPropagation()">
                    ${v.isAvailable
                        ? `<button class="btn btn-danger btn-sm" onclick="deleteVehicle('${v.id}')">Sterge</button>`
                        : '<span style="color:#6c757d;font-size:0.85rem">Inchiriat</span>'}
                </td>
            </tr>
        `;
    }));

    tbody.innerHTML = rows.join('');
}

async function showDetails(vehicleId, imgUrl) {
    const filtered = currentFilter === 'all'
        ? allVehicles
        : allVehicles.filter(v => (v.type || v.vehicleType) === currentFilter);

    currentVehicleIndex = filtered.findIndex(v => v.id === vehicleId);
    currentImages[vehicleId] = imgUrl; // salveaza poza in cache

    await renderDetails(filtered);
}

async function renderDetails(filtered) {
    const v = filtered[currentVehicleIndex];
    if (!v) return;

    selectedVehicle = v;
    const type = v.type || v.vehicleType || 'Car';

    // Foloseste poza din cache sau incarca una noua
    let imgUrl = currentImages[v.id];
    if (!imgUrl) {
        imgUrl = await getVehicleImage(v.brand, v.model, type, v.year);
        currentImages[v.id] = imgUrl;
    }

    document.getElementById('detailsTitle').textContent  = `${v.brand} ${v.model}`;
    document.getElementById('detailsImage').src          = imgUrl;
    document.getElementById('detailBrand').textContent   = v.brand;
    document.getElementById('detailModel').textContent   = v.model;
    document.getElementById('detailType').textContent    = type;
    document.getElementById('detailYear').textContent    = v.year;
    document.getElementById('detailPlate').textContent   = v.licensePlate;
    document.getElementById('detailFuel').textContent    = v.fuelType || '-';
    document.getElementById('detailRate').textContent    = `${v.dailyRate} MDL/zi`;
    document.getElementById('detailStatus').innerHTML    = getStatusBadge(v.status);

    // Sageti pe poza
    const prevBtn = document.getElementById('imgPrev');
    const nextBtn = document.getElementById('imgNext');
    const counter = document.getElementById('imgCounter');

    prevBtn.disabled = currentVehicleIndex <= 0;
    nextBtn.disabled = currentVehicleIndex >= filtered.length - 1;
    counter.textContent = `${currentVehicleIndex + 1} / ${filtered.length}`;

    document.getElementById('detailsNav').innerHTML = '';

    const actionsDiv = document.getElementById('detailsActions');
    if (v.isAvailable) {
        actionsDiv.innerHTML = `
    <div style="display:flex;gap:10px;margin-top:10px;">
        <button class="btn btn-primary" onclick="openRentModal()">
            🔑 Inchiriaza acest vehicul
        </button>
        <button class="btn btn-danger" onclick="deleteVehicle('${v.id}')">
            Sterge
        </button>
    </div>
`;
    } else {
        actionsDiv.innerHTML = `
            <span class="badge badge-warning" style="font-size:0.9rem;padding:8px 16px;">
                Vehicul inchiriat momentan
            </span>
        `;
    }

    document.getElementById('detailsModal').classList.remove('hidden');
}

async function navigateVehicle(direction) {
    const filtered = currentFilter === 'all'
        ? allVehicles
        : allVehicles.filter(v => (v.type || v.vehicleType) === currentFilter);

    currentVehicleIndex += direction;
    if (currentVehicleIndex < 0) currentVehicleIndex = 0;
    if (currentVehicleIndex >= filtered.length) currentVehicleIndex = filtered.length - 1;

    await renderDetails(filtered);
}

function hideDetailsModal() {
    document.getElementById('detailsModal').classList.add('hidden');
    selectedVehicle = null;
}

async function openRentModal() {
    if (!selectedVehicle) return;

    try {
        const customers = await apiGet('customers');
        document.getElementById('rentCustomer').innerHTML =
            customers.map(c => `<option value="${c.id}">${c.fullName}</option>`).join('');

        document.getElementById('rentVehicleName').textContent =
            `${selectedVehicle.brand} ${selectedVehicle.model} — ${selectedVehicle.dailyRate} MDL/zi`;

        const today = new Date().toISOString().split('T')[0];
        document.getElementById('rentStartDate').value = today;
        document.getElementById('rentEndDate').value   = '';
        document.getElementById('rentCostEstimate').textContent = '0 MDL';

        // Calcul cost estimat la schimbarea datei
        document.getElementById('rentEndDate').oninput = () => {
            const start = new Date(document.getElementById('rentStartDate').value);
            const end   = new Date(document.getElementById('rentEndDate').value);
            if (end > start) {
                const days = Math.ceil((end - start) / (1000 * 60 * 60 * 24));
                const cost = days * selectedVehicle.dailyRate;
                document.getElementById('rentCostEstimate').textContent = `${cost} MDL (${days} zile)`;
            }
        };

        document.getElementById('detailsModal').classList.add('hidden');
        document.getElementById('rentModal').classList.remove('hidden');
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

function hideRentModal() {
    document.getElementById('rentModal').classList.add('hidden');
    document.getElementById('detailsModal').classList.remove('hidden');
}

async function confirmRent() {
    if (!selectedVehicle) return;
    try {
        const startDate = document.getElementById('rentStartDate').value;
        const endDate   = document.getElementById('rentEndDate').value;

        if (!endDate || endDate <= startDate) {
            showToast('Data sfarsit trebuie sa fie dupa data inceput!', 'error');
            return;
        }

        const days = Math.ceil((new Date(endDate) - new Date(startDate)) / (1000 * 60 * 60 * 24));

        await apiPost('contracts', {
            customerId: document.getElementById('rentCustomer').value,
            vehicleId:  selectedVehicle.id,
            startDate:  startDate,
            endDate:    endDate,
            days:       days
        });

        showToast('Vehicul inchiriat cu succes!');
        hideRentModal();
        document.getElementById('detailsModal').classList.add('hidden');
        selectedVehicle = null;
        loadVehicles();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
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
        showToast('Vehicul sters!');
        hideDetailsModal();
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
        showToast('Masina adaugata!');
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
        showToast('Camion adaugat!');
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
        showToast('Motocicleta adaugata!');
        hideModal();
        loadVehicles();
    } catch (err) {
        showToast('Eroare: ' + err.message, 'error');
    }
}

loadVehicles();