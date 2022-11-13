using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Library.Models.Othres
{
    [Table("favourites")]
    public class Favourites
    {
        
        [Column("user_id")]
        [Required]
        public string UserId { get; set; }

        [ForeignKey("Id")]
        public Users.Users User { get; set; }

       
        [Column("book_id")]
        [Required]
        public string BookId { get; set; }

        [ForeignKey("Id")]
        public Book.Books Book { get; set; }
    }
}
