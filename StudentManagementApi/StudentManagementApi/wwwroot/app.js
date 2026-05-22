const API = 'https://localhost:7152/api';
let token = localStorage.getItem('token');

// Səhifə yükləndikdə
window.onload = () => {
    if (token) showMain();
    else showLogin();
};

// Səhifə göstərənlər
function showLogin() {
    document.getElementById('loginPage').style.display = 'flex';
    document.getElementById('registerPage').style.display = 'none';
    document.getElementById('mainPage').style.display = 'none';
}

function showRegister() {
    document.getElementById('loginPage').style.display = 'none';
    document.getElementById('registerPage').style.display = 'flex';
    document.getElementById('mainPage').style.display = 'none';
}

function showMain() {
    document.getElementById('loginPage').style.display = 'none';
    document.getElementById('registerPage').style.display = 'none';
    document.getElementById('mainPage').style.display = 'block';
    loadGroups();
    loadStudents();
}

function logout() {
    localStorage.removeItem('token');
    token = null;
    showLogin();
}

// Sorğu başlığı
function authHeader() {
    return {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };
}

// ========================
// AUTH
// ========================

async function register() {
    const username = document.getElementById('regUsername').value.trim();
    const password = document.getElementById('regPassword').value.trim();

    if (!username || !password) {
        alert('İstifadəçi adı və şifrə boş ola bilməz!');
        return;
    }

    try {
        const res = await fetch(`${API}/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        const data = await res.text();

        if (res.ok) {
            alert('Qeydiyyat uğurlu oldu! Daxil olun.');
            showLogin();
        } else {
            alert('Xəta: ' + data);
        }
    } catch (err) {
        alert('Server ilə əlaqə qurulmadı: ' + err.message);
    }
}

async function login() {
    const username = document.getElementById('loginUsername').value.trim();
    const password = document.getElementById('loginPassword').value.trim();

    if (!username || !password) {
        alert('İstifadəçi adı və şifrə boş ola bilməz!');
        return;
    }

    try {
        const res = await fetch(`${API}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (!res.ok) {
            alert('İstifadəçi adı və ya şifrə yanlışdır!');
            return;
        }

        const data = await res.json();
        token = data.token;
        localStorage.setItem('token', token);
        showMain();
    } catch (err) {
        alert('Server ilə əlaqə qurulmadı: ' + err.message);
    }
}

// ========================
// QRUPLAR
// ========================

async function loadGroups() {
    try {
        const res = await fetch(`${API}/groups`, { headers: authHeader() });

        if (res.status === 401) { logout(); return; }

        const groups = await res.json();

        const list = document.getElementById('groupList');
        const select = document.getElementById('stuGroup');

        list.innerHTML = '';
        select.innerHTML = '<option value="">Qrup seç</option>';

        groups.forEach(g => {
            list.innerHTML += `
                <li>
                    <span>${g.name}</span>
                    <button onclick="deleteGroup(${g.id})">Sil</button>
                </li>`;
            select.innerHTML += `<option value="${g.id}">${g.name}</option>`;
        });
    } catch (err) {
        alert('Qruplar yüklənmədi: ' + err.message);
    }
}

async function addGroup() {
    const name = document.getElementById('groupName').value.trim();

    if (!name) {
        alert('Qrup adı boş ola bilməz!');
        return;
    }

    try {
        const res = await fetch(`${API}/groups`, {
            method: 'POST',
            headers: authHeader(),
            body: JSON.stringify({ name })
        });

        const data = await res.text();

        if (res.ok) {
            document.getElementById('groupName').value = '';
            loadGroups();
        } else {
            alert('Xəta: ' + data);
        }
    } catch (err) {
        alert('Xəta: ' + err.message);
    }
}

async function deleteGroup(id) {
    if (!confirm('Qrupu silmək istədiyinizə əminsiniz?')) return;

    try {
        const res = await fetch(`${API}/groups/${id}`, {
            method: 'DELETE',
            headers: authHeader()
        });

        const data = await res.text();

        if (res.ok) {
            loadGroups();
            loadStudents();
        } else {
            alert('Xəta: ' + data);
        }
    } catch (err) {
        alert('Xəta: ' + err.message);
    }
}

// ========================
// TƏLƏBƏLƏR
// ========================

async function loadStudents() {
    const search = document.getElementById('stuSearch') ?
        document.getElementById('stuSearch').value.trim() : '';

    try {
        const res = await fetch(`${API}/students?search=${search}&page=1&pageSize=50`, {
            headers: authHeader()
        });

        if (res.status === 401) { logout(); return; }

        const students = await res.json();
        const list = document.getElementById('studentList');
        list.innerHTML = '';

        if (students.length === 0) {
            list.innerHTML = '<li style="justify-content:center; color:#888;">Tələbə tapılmadı</li>';
            return;
        }

        students.forEach(s => {
            list.innerHTML += `
                <li>
                    <span><b>${s.fullName}</b> — ${s.email} — Yaş: ${s.age} — ${s.groupName}</span>
                    <button onclick="deleteStudent(${s.id})">Sil</button>
                </li>`;
        });
    } catch (err) {
        alert('Tələbələr yüklənmədi: ' + err.message);
    }
}

async function addStudent() {
    const fullName = document.getElementById('stuName').value.trim();
    const age = parseInt(document.getElementById('stuAge').value);
    const email = document.getElementById('stuEmail').value.trim();
    const groupId = parseInt(document.getElementById('stuGroup').value);

    if (!fullName || !email || !groupId) {
        alert('Bütün sahələri doldurun!');
        return;
    }

    if (isNaN(age) || age < 16 || age > 60) {
        alert('Yaş 16 ilə 60 arasında olmalıdır!');
        return;
    }

    try {
        const res = await fetch(`${API}/students`, {
            method: 'POST',
            headers: authHeader(),
            body: JSON.stringify({ fullName, age, email, groupId })
        });

        const data = await res.text();

        if (res.ok) {
            document.getElementById('stuName').value = '';
            document.getElementById('stuAge').value = '';
            document.getElementById('stuEmail').value = '';
            loadStudents();
        } else {
            alert('Xəta: ' + data);
        }
    } catch (err) {
        alert('Xəta: ' + err.message);
    }
}

async function deleteStudent(id) {
    if (!confirm('Tələbəni silmək istədiyinizə əminsiniz?')) return;

    try {
        const res = await fetch(`${API}/students/${id}`, {
            method: 'DELETE',
            headers: authHeader()
        });

        const data = await res.text();

        if (res.ok) {
            loadStudents();
        } else {
            alert('Xəta: ' + data);
        }
    } catch (err) {
        alert('Xəta: ' + err.message);
    }
}