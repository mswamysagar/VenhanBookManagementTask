// src/api/urls.js
const BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:5000/api';


const URLS = {
  BOOKS: {
    GET_ALL: `${BASE_URL}/books`,
    GET: (id) => `${BASE_URL}/books/${id}`,
    CREATE: `${BASE_URL}/books`,
    UPDATE: (id) => `${BASE_URL}/books/${id}`,
    DELETE: (id) => `${BASE_URL}/books/${id}`,
    SEARCH: (q) => `${BASE_URL}/books/search?${new URLSearchParams(q)}`
  },
  BORROWERS: {
    GET_ALL: `${BASE_URL}/borrowers`,
    GET: (id) => `${BASE_URL}/borrowers/${id}`,
    CREATE: `${BASE_URL}/borrowers`,
    UPDATE: (id) => `${BASE_URL}/borrowers/${id}`,
    DELETE: (id) => `${BASE_URL}/borrowers/${id}`
  },
  BORROW: {
    GET_ALL: `${BASE_URL}/borrow`,
    BORROW_BOOK: `${BASE_URL}/borrow/borrow`,
    RETURN_BOOK: (id) => `${BASE_URL}/borrow/return/${id}`
  }
};

export default URLS;
