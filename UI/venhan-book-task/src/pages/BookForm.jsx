import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { createBook, getBook, updateBook } from '../api/api'
import { validateBook } from '../utils/validators'

export default function BookForm() {
  const { id } = useParams()
  const isEdit = !!id
  const [form, setForm] = useState({ title:'', author:'', isbn:'', genre:'', quantity:0 })
  const [errors, setErrors] = useState({})
  const [message, setMessage] = useState('')
  const nav = useNavigate()

  useEffect(() => {
    if (!isEdit) return
    (async () => {
      try {
        const data = await getBook(id)
        setForm({ title: data.title, author: data.author, isbn: data.isbn, genre: data.genre || '', quantity: data.quantity || 0, bookId: data.bookId })
      } catch (err) {
        alert(err.message || 'Failed to load')
      }
    })()
  }, [id])

  const handleSubmit = async (e) => {
    e.preventDefault()
    const v = validateBook(form)
    setErrors(v)
    if (Object.keys(v).length) return

    try {
      if (isEdit) {
        await updateBook(id, form)
        setMessage('Updated successfully')
      } else {
        await createBook(form)
        setMessage('Created successfully')
        setForm({ title:'', author:'', isbn:'', genre:'', quantity:0 })
      }
      setTimeout(() => nav('/books'), 800)
    } catch (err) {
      alert(err.message || 'Save failed')
    }
  }

  return (
    <div>
      <div className="page-header">
        <h2>{isEdit ? 'Edit Book' : 'New Book'}</h2>
      </div>

      <form className="form" onSubmit={handleSubmit}>
        <label>Title
          <input value={form.title} onChange={e => setForm({...form, title: e.target.value})} />
          {errors.title && <div className="error">{errors.title}</div>}
        </label>

        <label>Author
          <input value={form.author} onChange={e => setForm({...form, author: e.target.value})} />
          {errors.author && <div className="error">{errors.author}</div>}
        </label>

        <label>ISBN
          <input value={form.isbn} onChange={e => setForm({...form, isbn: e.target.value})} />
          {errors.isbn && <div className="error">{errors.isbn}</div>}
        </label>

        <label>Genre
          <input value={form.genre} onChange={e => setForm({...form, genre: e.target.value})} />
        </label>

        <label>Quantity
          <input type="number" value={form.quantity} onChange={e => setForm({...form, quantity: Number(e.target.value)})} />
          {errors.quantity && <div className="error">{errors.quantity}</div>}
        </label>

        <div className="form-actions">
          <button type="submit">{isEdit ? 'Update' : 'Create'}</button>
          <button type="button" onClick={() => nav('/books')}>Cancel</button>
        </div>
        {message && <div className="message">{message}</div>}
      </form>
    </div>
  )
}
