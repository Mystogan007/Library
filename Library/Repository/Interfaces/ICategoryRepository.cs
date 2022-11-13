using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public interface ICategoryRepository
    {
        public Task AddCategoryAsync(string name, CancellationToken cancellationToken);

        public Task DeleteCategoryAsync(string name, IBooksRepository booksRepository , CancellationToken cancellationToken);
    }
}
