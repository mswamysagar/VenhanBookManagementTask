import React, { useEffect, useState } from 'react';
import { getBooks, getBorrowers, getBorrowRecords, borrowBook, returnBook } from '../api/api';

export default function BorrowPage() {
  const [books, setBooks] = useState([]);
  const [borrowers, setBorrowers] = useState([]);
  const [records, setRecords] = useState([]);
  const [selectedBook, setSelectedBook] = useState('');
  const [selectedBorrower, setSelectedBorrower] = useState('');
  const [error, setError] = useState('');

  const loadAll = async () => {
    try {
      const [bks, brs, recs] = await Promise.all([getBooks(), getBorrowers(), getBorrowRecords()]);
      setBooks(Array.isArray(bks) ? bks : []);
      setBorrowers(Array.isArray(brs) ? brs : []);
      setRecords(Array.isArray(recs) ? recs : []);
    } catch (err) {
      setError(err.message || 'Failed to load data');
    }
  };

  useEffect(() => { loadAll() }, []);

  const handleBorrow = async () => {
    if (!selectedBook || !selectedBorrower) { alert('Choose book and borrower'); return }
    try {
      await borrowBook(selectedBook, selectedBorrower);
      alert('Borrowed successfully');
      await loadAll();
    } catch (err) { alert(err.message || 'Borrow failed'); }
  };

  const handleReturn = async (recordId) => {
    if (!confirm('Mark as returned?')) return;
    try {
      await returnBook(recordId);
      alert('Returned');
      await loadAll();
    } catch (err) { alert(err.message || 'Return failed'); }
  };

  return (
    <div>
      <h2>Borrow / Return</h2>
      {error && <div className="error">{error}</div>}

      <div>
        <h3>Borrow a Book</h3>
        <select value={selectedBook} onChange={e => setSelectedBook(e.target.value)}>
          <option value="">-- Select Book --</option>
          {books.filter(b => b.quantity > 0).map(b => <option key={b.bookId} value={b.bookId}>{b.title} ({b.quantity})</option>)}
        </select>
        <select value={selectedBorrower} onChange={e => setSelectedBorrower(e.target.value)}>
          <option value="">-- Select Borrower --</option>
          {borrowers.map(b => <option key={b.borrowerId} value={b.borrowerId}>{b.name}</option>)}
        </select>
        <button onClick={handleBorrow}>Borrow</button>
      </div>

      <div>
        <h3>Active Borrow Records</h3>
        <table>
          <thead>
            <tr><th>Book</th><th>Borrower</th><th>Borrowed</th><th>Due</th><th>Returned</th><th>Actions</th></tr>
          </thead>
          <tbody>
            {records.map(r => (
              <tr key={r.borrowRecordId}>
                <td>{r.book?.title}</td>
                <td>{r.borrower?.name}</td>
                <td>{new Date(r.borrowDate).toLocaleDateString()}</td>
                <td>{new Date(r.dueDate).toLocaleDateString()}</td>
                <td>{r.isReturned ? new Date(r.returnDate).toLocaleDateString() : '—'}</td>
                <td>{!r.isReturned && <button onClick={() => handleReturn(r.borrowRecordId)}>Return</button>}</td>
              </tr>
            ))}
            {records.length === 0 && <tr><td colSpan="6">No records</td></tr>}
          </tbody>
        </table>
      </div>
    </div>
  );
}
