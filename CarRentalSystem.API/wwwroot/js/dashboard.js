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
        tbody.innerHTML = available.map(v => `
            <tr>
                <td>${v.brand}</td>
                <td>${v.model}</td>
                <td>${v.type}</td>
                <td>${v.year}</td>
                <td>${v.licensePlate}</td>
                <td><strong>${v.dailyRate} MDL</strong></td>
            </tr>
        `).join('');

    } catch (err) {
        showToast('Eroare la incarcarea datelor: ' + err.message, 'error');
    }
}

loadDashboard();