using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VenhanBookManagementTask.Models
{
    [Table("BorrowRecords")]
    public class BorrowRecordModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BorrowRecordId { get; set; } = Guid.NewGuid(); 

        [Required]
        public Guid BookId { get; set; }  

        [Required]
        public Guid BorrowerId { get; set; }  

        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }= DateTime.UtcNow.AddDays(7);
        public DateTime? ReturnDate { get; set; }= null;
        public bool IsReturned { get; set; } = false;

        [ForeignKey(nameof(BookId))]
        [JsonIgnore]
        public BookModel Book { get; set; }=null!;

        [ForeignKey(nameof(BorrowerId))]
        [JsonIgnore]
        public BorrowerModel Borrower { get; set; }=null!;
    }
}
