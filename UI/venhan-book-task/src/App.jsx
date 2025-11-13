import { Routes, Route, Navigate } from 'react-router-dom';
import BooksPage from './pages/BooksPage';
import EditBook from "./pages/EditBook";
import BookForm from './pages/BookForm';
import BorrowersPage from './pages/BorrowersPage';
import BorrowerForm from './pages/BorrowerForm';
import BorrowPage from './pages/BorrowPage';

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/books" replace />} />
      <Route path="/books" element={<BooksPage />} />
      {/* <Route path="/books/edit/:id" element={<EditBook />} /> */}
      <Route path="/books/new" element={<BookForm />} />
      <Route path="/books/edit/:id" element={<BookForm />} />
      <Route path="/borrowers" element={<BorrowersPage />} />
      <Route path="/borrowers/new" element={<BorrowerForm />} />
      <Route path="/borrow" element={<BorrowPage />} />
    </Routes>
  );
}
