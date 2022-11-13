using Library.Models.Othres;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public interface IFavouriteRepository
    {
        public Task AddFavouriteAsync(FavouritesRelations relations, CancellationToken cancellationToken);

        public Task DeleteFavouriteAsync(FavouritesRelations relations, CancellationToken cancellationToken);
    }
}
