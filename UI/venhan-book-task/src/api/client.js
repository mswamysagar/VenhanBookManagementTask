// src/api/client.js
export async function request(url, options = {}) {
  const opts = {
    headers: { 'Content-Type': 'application/json' },
    ...options
  };
  if (opts.body && typeof opts.body !== 'string') opts.body = JSON.stringify(opts.body);

  try {
    const res = await fetch(url, opts);
    const text = await res.text();
    let data = null;
    try { data = text ? JSON.parse(text) : null } catch { data = text }

    if (!res.ok) {
      const message = (data && data.error) || (data && data.message) || res.statusText;
      throw new Error(message || 'API Error');
    }
    return data;
  } catch (err) {
    throw err;
  }
}
