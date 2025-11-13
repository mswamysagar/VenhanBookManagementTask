using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenhanBookManagementTask.Models
{
    [Table("Borrowers")]
    public class BorrowerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BorrowerId { get; set; } = Guid.NewGuid();  

        [Required, StringLength(120)]
        public string Name { get; set; }=string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; }=string.Empty;

        [Required, StringLength(50)]
        public string MembershipId { get; set; }=string.Empty;

        [Phone]
        public string ContactNumber { get; set; }=string.Empty;


        public ICollection<BorrowRecordModel> BorrowRecords { get; set; } = new List<BorrowRecordModel>();
    }
}
