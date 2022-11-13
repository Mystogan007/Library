using Library.Models.Othres;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Book
{
    [Table("books")]
    public class Books
    {
        [Column("id")]
        [Key]
        public string Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("normalize_name")]
        [Required]
        public string NameNormalise { get; set; }

        [Column("image_link")]
        public string ImageLink { get; set; }

        [Column("author")]
        [Required]
        public string Author { get; set; }

        [Column("description")]
        [Required]
        public string Description { get; set; }

        [Column("publish_year")]
        [Required]
        public int PublishYear { get; set; }

        [Column("pages")]
        [Required]
        public int Pages { get; set; }

        public IList<BookCatRelations> BookCategoryRelations { get; set; }

        public IList<Favourites> Favourites { get; set; }
    }
}
