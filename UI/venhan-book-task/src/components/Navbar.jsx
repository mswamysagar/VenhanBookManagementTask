import React from 'react'
import { Link, useLocation } from 'react-router-dom'

export default function Navbar() {
  const loc = useLocation()
  return (
    <nav className="nav">
      <div className="nav-brand">Venhan Library</div>
      <div className="nav-links">
        <Link className={loc.pathname.startsWith('/books') ? 'active' : ''} to="/books">Books</Link>
        <Link className={loc.pathname.startsWith('/borrowers') ? 'active' : ''} to="/borrowers">Borrowers</Link>
        <Link className={loc.pathname === '/borrow' ? 'active' : ''} to="/borrow">Borrow/Return</Link>
      </div>
    </nav>
  )
}
