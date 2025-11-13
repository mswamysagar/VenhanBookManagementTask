using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenhanBookManagementTask.Models
{
    [Table("Books")]
    public class BookModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BookId { get; set; } = Guid.NewGuid();  

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; }=string.Empty; 

        [Required(ErrorMessage = "Author is required")]
        [StringLength(150)]
        public string Author { get; set; }=string.Empty;    

        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(50)]
        public string ISBN { get; set; }=string.Empty;

        [StringLength(100)]
        public string Genre { get; set; }=string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }

        public ICollection<BorrowRecordModel> BorrowRecords { get; set; } = new List<BorrowRecordModel>();
    }
}
