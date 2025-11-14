using Microsoft.EntityFrameworkCore;
using VenhanBookManagementTask.Models;

namespace VenhanBookManagementTask.DAL
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }

        public DbSet<BookModel> Books { get; set; }
        public DbSet<BorrowerModel> Borrowers { get; set; }
        public DbSet<BorrowRecordModel> BorrowRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookModel>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            modelBuilder.Entity<BorrowerModel>()
                .HasIndex(b => b.MembershipId)
                .IsUnique();

            
            modelBuilder.Entity<BorrowRecordModel>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowRecordModel>()
                .HasOne(br => br.Borrower)
                .WithMany(bw => bw.BorrowRecords)
                .HasForeignKey(br => br.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
