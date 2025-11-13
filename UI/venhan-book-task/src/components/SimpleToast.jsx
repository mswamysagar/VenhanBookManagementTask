import React from 'react'
export default function SimpleToast({ message, type = 'info' }) {
  if (!message) return null
  return <div className={`toast ${type}`}>{message}</div>
}
