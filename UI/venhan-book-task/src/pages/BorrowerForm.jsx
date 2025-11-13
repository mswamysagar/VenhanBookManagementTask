import React, { useEffect, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { createBorrower, getBorrower, updateBorrower } from '../api/api'
import { validateBorrower } from '../utils/validators'

export default function BorrowerForm() {
  const [search] = useSearchParams()
  const editId = search.get('edit')
  const isEdit = !!editId
  const [form, setForm] = useState({ name:'', email:'', membershipId:'', contactNumber:'' })
  const [errors, setErrors] = useState({})
  const nav = useNavigate()

  useEffect(() => {
    if (!isEdit) return
    (async () => {
      try {
        const data = await getBorrower(editId)
        setForm({ name: data.name, email: data.email, membershipId: data.membershipId, contactNumber: data.contactNumber, borrowerId: data.borrowerId })
      } catch (err) { alert(err.message || 'Load failed') }
    })()
  }, [editId])

  const handleSubmit = async (e) => {
    e.preventDefault()
    const v = validateBorrower(form)
    setErrors(v)
    if (Object.keys(v).length) return

    try {
      if (isEdit) {
        await updateBorrower(editId, form)
      } else {
        await createBorrower(form)
      }
      nav('/borrowers')
    } catch (err) {
      alert(err.message || 'Save failed')
    }
  }

  return (
    <div>
      <div className="page-header"><h2>{isEdit ? 'Edit Borrower' : 'New Borrower'}</h2></div>

      <form className="form" onSubmit={handleSubmit}>
        <label>Name<input value={form.name} onChange={e => setForm({...form, name: e.target.value})} />{errors.name && <div className="error">{errors.name}</div>}</label>
        <label>Email<input value={form.email} onChange={e => setForm({...form, email: e.target.value})} />{errors.email && <div className="error">{errors.email}</div>}</label>
        <label>Membership ID<input value={form.membershipId} onChange={e => setForm({...form, membershipId: e.target.value})} />{errors.membershipId && <div className="error">{errors.membershipId}</div>}</label>
        <label>Contact<input value={form.contactNumber} onChange={e => setForm({...form, contactNumber: e.target.value})} /></label>

        <div className="form-actions">
          <button type="submit">{isEdit ? 'Update' : 'Create'}</button>
          <button type="button" onClick={() => nav('/borrowers')}>Cancel</button>
        </div>
      </form>
    </div>
  )
}
