using Library.Storage;
using Library.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public class PgCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext _context;

        public PgCategoryRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task AddCategoryAsync(string name, CancellationToken cancellationToken)
        {
            var id = MD5.Create(name);
            var cat = await _context.Categories.FindAsync(new object[] { id }, cancellationToken);
            if (cat == null)
            {
                await _context.Categories.AddAsync(new Models.Othres.Categories() { Id = id, Name = name }, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);              
            }           
        }

        public async Task DeleteCategoryAsync(string name, IBooksRepository booksRepository , CancellationToken cancellationToken)
        {
            var id = MD5.Create(name);
            var cat = await _context.Categories.FindAsync(new object[] { id }, cancellationToken);
            if (cat == null)
            {
                _context.Categories.Remove(cat);
                var categoriesRel = await _context.BookCategoryRelations.Where(a => a.CategoryId == id).ToListAsync(cancellationToken);
                if (categoriesRel.Any())
                {
                    _context.BookCategoryRelations.RemoveRange(categoriesRel);
                }
                await _context.SaveChangesAsync(cancellationToken);              
            }
        }
    }
}
