using Library.Models.Book;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Othres
{
    [Table("book_cat_relations")]
    public class BookCatRelations
    {
        [Column("book_id")]
        [Required]
        public string BookId { get; set; }

        [ForeignKey("Id")]
        public Books Book { get; set; }


        [Column("category_id")]
        [Required]
        public string CategoryId { get; set; }
        [ForeignKey("Id")]
        public Categories Category { get; set; }

    }
}
