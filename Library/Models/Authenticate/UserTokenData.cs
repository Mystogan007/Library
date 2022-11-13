using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Authenticate
{
    [Table("user_tokens")]
    public class UserTokenData
    {
        [Column("id")]
        [Key]
        public string Id { get; set; }

        [Column("login")]
        [Required]
        public string Login { get; set; }

        [Column("token_id")]
        [Required]
        public string TokenId { get; set; }

        [Column("refresh_token")]
        [Required]
        public string RefreshToken { get; set; }

        [Column("is_active")]
        [Required]
        public bool? IsActive { get; set; }
    }
}
