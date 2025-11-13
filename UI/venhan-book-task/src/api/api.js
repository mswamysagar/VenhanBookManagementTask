import axios from 'axios';

// ✅ Set your backend base URL
const api = axios.create({
  baseURL: 'https://localhost:5000/api', // change port as per your backend
});

// ✅ Books APIs
export const getBooks = () => api.get('/books');
export const getBook = (id) => api.get(`/books/${id}`);
export const createBook = (data) => api.post('/books', data);
export const updateBook = (id, data) => api.put(`/books/${id}`, data);
export const deleteBook = (id) => api.delete(`/books/${id}`);
export const searchBooks = (query) =>
  api.get('/books/search', { params: query });

// ✅ Borrowers APIs
export const getBorrowers = () => api.get('/borrowers');
export const getBorrower = (id) => api.get(`/borrowers/${id}`);
export const createBorrower = (data) => api.post('/borrowers', data);
export const updateBorrower = (id, data) => api.put(`/borrowers/${id}`, data);
export const deleteBorrower = (id) => api.delete(`/borrowers/${id}`);

// ✅ Borrow APIs
export const getBorrowRecords = () => api.get('/borrow/records');
export const borrowBook = (bookId, borrowerId) =>
  api.post('/borrow', { bookId, borrowerId });
export const returnBook = (recordId) => api.post(`/borrow/return/${recordId}`);
