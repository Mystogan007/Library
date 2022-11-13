using Library.Models.Othres;
using Library.Storage;
using Library.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public class PgFavouriteRepository : IFavouriteRepository
    {
        private readonly ApplicationContext _context;

        public PgFavouriteRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddFavouriteAsync(FavouritesRelations relations, CancellationToken cancellationToken)
        {
            var id = MD5.Create($"{relations.UserId}{relations.BookId}"); 
            var fav = await _context.Favourites.FindAsync(new object[] {id }, cancellationToken);
            if (fav == null)
            {
                fav = new Favourites() { BookId = relations.BookId, UserId = relations.UserId };
                await _context.Favourites.AddAsync(fav, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteFavouriteAsync(FavouritesRelations relations, CancellationToken cancellationToken)
        {
            var id = MD5.Create($"{relations.UserId}{relations.BookId}");
            var fav = await _context.Favourites.FindAsync(new object[] { id }, cancellationToken);
            if (fav == null)
            {
                _context.Favourites.Remove(fav);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
     
    }
}
