import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";

const EditBook = () => {
  const { id } = useParams(); // ✅ Get the BookId from URL
  const navigate = useNavigate();

  const [book, setBook] = useState({
    title: "",
    author: "",
    genre: "",
    isbn: "",
    quantity: 0,
  });

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // ✅ Fetch the book by ID from API
  useEffect(() => {
    const fetchBook = async () => {
      try {
        const res = await fetch(`https://localhost:7033/api/Books/${id}`);
        if (!res.ok) throw new Error("Book not found");

        const data = await res.json();
        setBook(data); // ✅ Fill state with book details
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchBook();
  }, [id]);

  const handleChange = (e) => {
    setBook({ ...book, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const res = await fetch(`https://localhost:7033/api/Books/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(book),
    });
    if (res.ok) {
      alert("Book updated successfully!");
      navigate("/books");
    } else {
      alert("Failed to update book");
    }
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;

  return (
    <div className="container">
      <h2>Edit Book</h2>
      <form className="form" onSubmit={handleSubmit}>
        <label>Title</label>
        <input
          type="text"
          name="title"
          value={book.title}
          onChange={handleChange}
        />
        <label>Author</label>
        <input
          type="text"
          name="author"
          value={book.author}
          onChange={handleChange}
        />
        <label>Genre</label>
        <input
          type="text"
          name="genre"
          value={book.genre}
          onChange={handleChange}
        />
        <label>ISBN</label>
        <input
          type="text"
          name="isbn"
          value={book.isbn}
          onChange={handleChange}
        />
        <label>Quantity</label>
        <input
          type="number"
          name="quantity"
          value={book.quantity}
          onChange={handleChange}
        />
        <button type="submit">Update</button>
      </form>
    </div>
  );
};

export default EditBook;
