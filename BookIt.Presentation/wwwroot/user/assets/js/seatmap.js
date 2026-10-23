// Seat map + hold/confirm flow on the event details page. Talks only to /api.
(function () {
    const root = document.getElementById('seat-map');
    if (!root) return;

    const eventId = parseInt(root.dataset.eventId, 10);
    const isAuthenticated = root.dataset.authenticated === 'true';
    const loginUrl = root.dataset.loginUrl;
    const profileUrl = root.dataset.profileUrl;

    const grid = document.getElementById('seat-grid');
    const holdsList = document.getElementById('seat-holds');
    const message = document.getElementById('seat-message');
    const confirmBtn = document.getElementById('seat-confirm');

    const holds = new Map(); // eventSeatId -> { seatId, name, price, heldUntil }
    let timerHandle = null;

    const api = {
        headers: { 'Content-Type': 'application/json', 'X-Requested-With': 'XMLHttpRequest' },
        async get(url) { return fetch(url, { credentials: 'same-origin' }); },
        async post(url, body) { return fetch(url, { method: 'POST', credentials: 'same-origin', headers: this.headers, body: body ? JSON.stringify(body) : null }); },
        async del(url) { return fetch(url, { method: 'DELETE', credentials: 'same-origin', headers: this.headers }); }
    };

    function say(text, kind) {
        message.textContent = text || '';
        message.className = 'seat-message ' + (kind || '');
    }

    async function loadMap() {
        const res = await api.get(`/api/events/${eventId}/seats`);
        if (!res.ok) { say('Could not load the seat map.', 'error'); return; }
        const seats = await res.json();
        render(seats);
    }

    function render(seats) {
        grid.innerHTML = '';
        const rows = new Map();
        seats.forEach(s => { if (!rows.has(s.row)) rows.set(s.row, []); rows.get(s.row).push(s); });

        [...rows.keys()].sort((a, b) => a - b).forEach(rowNo => {
            const rowEl = document.createElement('div');
            rowEl.className = 'seat-row';
            const label = document.createElement('span');
            label.className = 'seat-row-label';
            label.textContent = rowNo;
            rowEl.appendChild(label);

            rows.get(rowNo).sort((a, b) => a.column - b.column).forEach(s => {
                const btn = document.createElement('button');
                btn.type = 'button';
                btn.className = 'seat seat-' + (s.heldByMe ? 'mine' : s.status.toLowerCase());
                btn.title = `${s.name} · ${s.seatType} · ${s.price.toFixed(2)}`;
                btn.dataset.seatId = s.seatId;
                btn.dataset.eventSeatId = s.eventSeatId;
                btn.disabled = s.status === 'Sold' || (s.status === 'Held' && !s.heldByMe);
                btn.addEventListener('click', () => onSeatClick(s, btn));
                rowEl.appendChild(btn);

                if (s.heldByMe) holds.set(s.eventSeatId, { seatId: s.seatId, name: s.name, price: s.price, heldUntil: s.heldUntil });
            });
            grid.appendChild(rowEl);
        });
        renderHolds();
    }

    async function onSeatClick(seat, btn) {
        if (!isAuthenticated) { window.location.href = loginUrl; return; }

        if (holds.has(seat.eventSeatId)) {
            const res = await api.del(`/api/holds/${seat.eventSeatId}`);
            if (res.ok || res.status === 410) { holds.delete(seat.eventSeatId); btn.className = 'seat seat-available'; renderHolds(); }
            return;
        }

        const res = await api.post(`/api/events/${eventId}/holds`, { seatId: seat.seatId });
        if (res.status === 201) {
            const hold = await res.json();
            holds.set(hold.holdId, { seatId: seat.seatId, name: seat.name, price: seat.price, heldUntil: hold.heldUntil });
            btn.className = 'seat seat-mine';
            say(`${seat.name} is yours for 10 minutes.`, 'ok');
        } else if (res.status === 409) {
            btn.className = 'seat seat-held'; btn.disabled = true;
            say(`${seat.name} was just taken by someone else.`, 'error');
        } else if (res.status === 429) {
            say('Too many requests, slow down a little.', 'error');
        } else if (res.status === 401) {
            window.location.href = loginUrl;
        } else {
            say('Something went wrong, please try again.', 'error');
        }
        renderHolds();
    }

    function renderHolds() {
        holdsList.innerHTML = '';
        let total = 0;
        holds.forEach((h, id) => {
            total += h.price;
            const li = document.createElement('li');
            li.innerHTML = `<span>${h.name}</span><span class="seat-hold-price">${h.price.toFixed(2)}</span><span class="seat-hold-timer" data-until="${h.heldUntil}"></span>`;
            holdsList.appendChild(li);
        });
        confirmBtn.disabled = holds.size === 0;
        confirmBtn.textContent = holds.size === 0 ? 'select seats' : `confirm ${holds.size} seat(s) · ${total.toFixed(2)}`;
        tick();
        if (!timerHandle) timerHandle = setInterval(tick, 1000);
    }

    function tick() {
        const now = Date.now();
        document.querySelectorAll('.seat-hold-timer').forEach(el => {
            const left = Math.max(0, Math.floor((new Date(el.dataset.until).getTime() - now) / 1000));
            el.textContent = `${Math.floor(left / 60)}:${String(left % 60).padStart(2, '0')}`;
            if (left === 0) el.classList.add('expired');
        });
    }

    async function confirmAll() {
        confirmBtn.disabled = true;
        const results = [];
        for (const [id, h] of holds) {
            const res = await api.post(`/api/holds/${id}/confirm`);
            results.push({ h, ok: res.status === 200, status: res.status });
        }
        const ok = results.filter(r => r.ok).length;
        const failed = results.filter(r => !r.ok).map(r => r.h.name);
        holds.clear();
        await loadMap();
        if (failed.length === 0) {
            say(`Booked ${ok} seat(s). See your reservations in your profile.`, 'ok');
            if (profileUrl) setTimeout(() => window.location.href = profileUrl, 1500);
        } else {
            say(`Booked ${ok} seat(s); ${failed.join(', ')} could not be confirmed (hold expired or seat sold).`, 'error');
        }
    }

    confirmBtn.addEventListener('click', confirmAll);
    loadMap();
})();
