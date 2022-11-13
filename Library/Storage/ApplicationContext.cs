using Library.Models;
using Library.Models.Authenticate;
using Library.Models.Book;
using Library.Models.Othres;
using Library.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Library.Storage
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<UserTokenData> Tokens { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<BookCatRelations> BookCategoryRelations { get; set; }
        public DbSet<Favourites> Favourites { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
