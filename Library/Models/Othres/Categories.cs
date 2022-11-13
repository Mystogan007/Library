using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Othres
{
    [Table("categories")]
    public class Categories
    {
        [Column("id")]
        [Key]
        public string Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        public IList<BookCatRelations> BookCategoryRelations { get; set; }
    }
}
