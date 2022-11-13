using Library.Models.Othres;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Users
{
    [Table("users")]
    public class Users
    {
        [Column("id")]
        [Key]
        public string Id { get; set; }

        [Column("login")]
        [Required]
        public string Login { get; set; }

        [Column("password")]
        [Required]
        public string Password { get; set; }

        [Column("role")]
        [Required]
        public string Role { get; set; }


        public IList<Favourites> Favourites { get; set; }

    }
}
