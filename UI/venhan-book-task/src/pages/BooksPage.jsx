import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getBooks, deleteBook, searchBooks } from '../api/api';
import SimpleToast from '../components/SimpleToast';

export default function BooksPage() {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [query, setQuery] = useState({ title: '', author: '', genre: '' });
  const nav = useNavigate();

  // ✅ Move useEffect INSIDE the component
  useEffect(() => {
    load();
  }, []);

  const load = async () => {
    setLoading(true);
    setError('');
    try {
      const response = await getBooks();
      // axios returns data under response.data
      setBooks(Array.isArray(response.data) ? response.data : []);
    } catch (err) {
      setError(err.message || 'Failed to fetch books');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete this book?')) return;
    try {
      await deleteBook(id);
      setBooks((prev) => prev.filter((b) => b.bookId !== id));
    } catch (err) {
      alert(err.message || 'Delete failed');
    }
  };

  const handleSearch = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      const res = await searchBooks(query);
      setBooks(Array.isArray(res.data) ? res.data : []);
    } catch (err) {
      alert(err.message || 'Search failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="page">
      <div className="page-header">
        <h2>Books</h2>
        <div>
          <button onClick={() => nav('/books/new')}>+ New Book</button>
        </div>
      </div>

      <form className="search" onSubmit={handleSearch}>
        <input
          placeholder="Title"
          value={query.title}
          onChange={(e) => setQuery({ ...query, title: e.target.value })}
        />
        <input
          placeholder="Author"
          value={query.author}
          onChange={(e) => setQuery({ ...query, author: e.target.value })}
        />
        <input
          placeholder="Genre"
          value={query.genre}
          onChange={(e) => setQuery({ ...query, genre: e.target.value })}
        />
        <button type="submit">Search</button>
        <button
          type="button"
          onClick={() => {
            setQuery({ title: '', author: '', genre: '' });
            load();
          }}
        >
          Reset
        </button>
      </form>

      {error && <SimpleToast message={error} type="error" />}

      {loading ? (
        <p>Loading...</p>
      ) : (
        <table className="list">
          <thead>
            <tr>
              <th>Title</th>
              <th>Author</th>
              <th>ISBN</th>
              <th>Genre</th>
              <th>Qty</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {books.map((b) => (
              <tr key={b.bookId}>
                <td>{b.title}</td>
                <td>{b.author}</td>
                <td>{b.isbn}</td>
                <td>{b.genre}</td>
                <td>{b.quantity}</td>
                <td>
  {/* ✅ Edit button - use nav (your useNavigate variable) and the current item 'b' */}
  <button
    onClick={() => nav(`/books/edit/${b.bookId}`)}
  >
    Edit
  </button>
  &nbsp;

  <button
    style={{ background: "red" }}
    onClick={() => handleDelete(b.bookId)}
  >
    Delete
  </button>
</td>
              </tr>
            ))}
            {books.length === 0 && (
              <tr>
                <td colSpan="6">No books</td>
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
}
