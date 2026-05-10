async function loadDashboard() {
    try {
        const [vehicles, available, customers, contracts] = await Promise.all([
            apiGet('vehicles'),
            apiGet('vehicles/available'),
            apiGet('customers'),
            apiGet('contracts')
        ]);

        document.getElementById('totalVehicles').textContent     = vehicles.length;
        document.getElementById('availableVehicles').textContent = available.length;
        document.getElementById('totalCustomers').textContent    = customers.length;
        document.getElementById('totalContracts').textContent    = contracts.length;

        const tbody = document.getElementById('availableVehiclesTable');
        tbody.innerHTML = '<tr><td colspan="7" style="text-align:center;padding:20px;color:#8b8d96;">Se incarca pozele...</td></tr>';

        const rows = await Promise.all(available.map(async v => {
            const imgUrl = await getVehicleImage(v.brand, v.model, v.type || v.vehicleType, v.year);
            return `
                <tr>
                    <td><img src="${imgUrl}" alt="${v.brand} ${v.model}"
                             style="width:90px;height:56px;object-fit:cover;border-radius:6px;display:block;"></td>
                    <td>${v.brand}</td>
                    <td>${v.model}</td>
                    <td>${v.type || v.vehicleType}</td>
                    <td>${v.year}</td>
                    <td>${v.licensePlate}</td>
                    <td><strong>${v.dailyRate} MDL</strong></td>
                </tr>
            `;
        }));

        tbody.innerHTML = rows.join('');

    } catch (err) {
        showToast('Eroare la incarcarea datelor: ' + err.message, 'error');
    }
}

loadDashboard();