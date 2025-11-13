

import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { getBorrowers, deleteBorrower } from '../api/api'

export default function BorrowersPage() {
  const [list, setList] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const nav = useNavigate()

  const load = async () => {
    setLoading(true); setError('')
    try {
      const data = await getBorrowers()
      setList(Array.isArray(data) ? data : [])
    } catch (err) {
      setError(err.message || 'Failed to load borrowers')
    } finally { setLoading(false) }
  }

  useEffect(() => { load() }, [])

  const handleDelete = async (id) => {
    if (!confirm('Delete borrower?')) return
    try {
      await deleteBorrower(id)
      setList(prev => prev.filter(b => b.borrowerId !== id))
    } catch (err) {
      alert(err.message || 'Delete failed')
    }
  }

  return (
    <div>
      <div className="page-header">
        <h2>Borrowers</h2>
        <div><button onClick={() => nav('/borrowers/new')}>+ New Borrower</button></div>
      </div>

      {error && <div className="error">{error}</div>}
      {loading ? <p>Loading...</p> : (
        <table className="list">
          <thead><tr><th>Name</th><th>Email</th><th>Membership</th><th>Contact</th><th>Actions</th></tr></thead>
          <tbody>
            {list.map(b => (
              <tr key={b.borrowerId}>
                <td>{b.name}</td>
                <td>{b.email}</td>
                <td>{b.membershipId}</td>
                <td>{b.contactNumber}</td>
                <td>
                  <button onClick={() => nav(`/borrowers/new?edit=${b.borrowerId}`)}>Edit</button>
                  <button onClick={() => handleDelete(b.borrowerId)}>Delete</button>
                </td>
              </tr>
            ))}
            {list.length === 0 && <tr><td colSpan="5">No borrowers</td></tr>}
          </tbody>
        </table>
      )}
    </div>
  )
}
