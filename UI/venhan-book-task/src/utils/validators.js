export const validateBook = (b) => {
  const errors = {}
  if (!b.title || b.title.trim().length < 2) errors.title = 'Title required (min 2 chars)'
  if (!b.author || b.author.trim().length < 2) errors.author = 'Author required (min 2 chars)'
  if (!b.isbn || b.isbn.trim().length < 5) errors.isbn = 'ISBN required'
  if (b.quantity == null || isNaN(b.quantity) || Number(b.quantity) < 0) errors.quantity = 'Quantity must be 0 or more'
  return errors
}

export const validateBorrower = (b) => {
  const errors = {}
  if (!b.name || b.name.trim().length < 2) errors.name = 'Name required'
  if (!b.email || !/^\S+@\S+\.\S+$/.test(b.email)) errors.email = 'Valid email required'
  if (!b.membershipId || b.membershipId.trim().length < 3) errors.membershipId = 'Membership ID required'
  return errors
}
