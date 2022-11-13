using Library.Models.Book;
using Library.Models.CRUDResult;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public interface IBooksRepository
    {
        public Task<CrudResult> AddBookAsync(BooksForm book, ICategoryRepository categoryRepository, string webRoot,CancellationToken cancellationToken);

        public Task<CrudResult> DeleteBookAsync(string id, CancellationToken cancellationToken);

        public Task<CrudResult> ChangeBookAsync(BooksForm book, ICategoryRepository categoryRepository, string webRoot, CancellationToken cancellationToken);
              
        public Task<List<Books>> GetBooksWithSearchingAsync(Filters filter, CancellationToken cancellationToken);
    }
}
