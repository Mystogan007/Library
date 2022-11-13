using Library.Models.Book;
using Library.Models.CRUDResult;
using Library.Models.Othres;
using Library.Storage;
using Library.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public class PgBooksRepository : IBooksRepository
    {
        private readonly ApplicationContext _context;

        public PgBooksRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<CrudResult> AddBookAsync(BooksForm bookForm, ICategoryRepository categoryRepository, string webRoot, CancellationToken cancellationToken)
        {
            var normalizeName = StringNormaliser.NormalizeString(bookForm.Name);
            var id = MD5.Create(normalizeName);
            var bookTemp = await _context.Books.FindAsync(new object[] { id }, cancellationToken);

            if (bookTemp == null)
            {
                using (var intermediate = new MemoryStream())
                {
                    await bookForm.Image.CopyToAsync(intermediate, cancellationToken);
                    intermediate.Seek(0, SeekOrigin.Begin);
                    await ImageHelper.SavePictureAsync(bookForm.Image.Name, intermediate.ToArray(), cancellationToken);
                }

                Books book = new Books()
                {
                    Id = id,
                    Name = bookForm.Name,
                    NameNormalise = normalizeName,
                    Author = bookForm.Author,
                    ImageLink = $"{webRoot}{bookForm.Image.FileName}",
                    Description = bookForm.Description,
                    PublishYear = bookForm.PublishYear,
                    Pages = bookForm.Pages,

                };
                if (bookForm.CategoryIds.Any())
                {
                    List<BookCatRelations> categoriesRel = bookForm.CategoryIds
                        .Select(c => new BookCatRelations() { BookId = id, CategoryId = c }).ToList();
                    book.BookCategoryRelations = categoriesRel;
                    await _context.Books.AddAsync(book, cancellationToken);
                    await _context.BookCategoryRelations.AddRangeAsync();
                }
                else
                {
                    await _context.Books.AddAsync(book, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return new CrudResult() { StatusOperation = Status.Ok, Error = String.Empty };
            }
            else
            {
                return new CrudResult() { StatusOperation = Status.Error, Error = "Book already exists" };
            }
        }

        public async Task<CrudResult> ChangeBookAsync(BooksForm bookForm, ICategoryRepository categoryRepository, string webRoot, CancellationToken cancellationToken)
        {
            var bookFormating = await _context.Books.FindAsync(new object[] { bookForm.Id }, cancellationToken);
            if (bookFormating != null)
            {
                var normalizeName = StringNormaliser.NormalizeString(bookForm.Name);
                if (bookForm.Image != null)
                {
                    ImageHelper.DeletePicture(Path.GetFileName(new Uri(bookFormating.ImageLink).LocalPath));
                    using (var intermediate = new MemoryStream())
                    {
                        await bookForm.Image.CopyToAsync(intermediate, cancellationToken);
                        intermediate.Seek(0, SeekOrigin.Begin);
                        await ImageHelper.SavePictureAsync(bookForm.Image.Name, intermediate.ToArray(), cancellationToken);
                    }
                }

                bookFormating.Name = bookForm.Name;
                bookFormating.NameNormalise = normalizeName;
                bookFormating.Author = bookForm.Author;
                bookFormating.ImageLink = $"{webRoot}{bookForm.Image.FileName}";
                bookFormating.Description = bookForm.Description;
                bookFormating.PublishYear = bookForm.PublishYear;
                bookFormating.Pages = bookForm.Pages;

                if (bookForm.CategoryIds.Any())
                {
                    var categoriesRel = await _context.BookCategoryRelations.Where(a => a.BookId == bookFormating.Id).ToListAsync(cancellationToken);
                    if (categoriesRel.Any())
                    {
                        _context.BookCategoryRelations.RemoveRange(categoriesRel);
                    }
                    categoriesRel = bookForm.CategoryIds
                       .Select(c => new BookCatRelations() { BookId = bookFormating.Id, CategoryId = c }).ToList();
                    bookFormating.BookCategoryRelations = categoriesRel;                   
                    await _context.BookCategoryRelations.AddRangeAsync();
                }
               
                await _context.SaveChangesAsync(cancellationToken);
                return new CrudResult() { StatusOperation = Status.Ok, Error = String.Empty };
            }
            else
            {
                return new CrudResult() { StatusOperation = Status.Error, Error = "Book doesn't exist" };
            }
        }

        public async Task<CrudResult> DeleteBookAsync(string id, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { id }, cancellationToken);
            if (book != null)
            {
                _context.Books.Remove(book);
                var categoriesRel = await _context.BookCategoryRelations.Where(a => a.BookId == id).ToListAsync(cancellationToken);
                var favourites = await _context.Favourites.Where(a => a.BookId == id).ToListAsync(cancellationToken);
                if (categoriesRel.Any())
                {
                    _context.BookCategoryRelations.RemoveRange(categoriesRel);
                }
                if (favourites.Any())
                {
                    _context.Favourites.RemoveRange(favourites);
                }
                await _context.SaveChangesAsync(cancellationToken);
                ImageHelper.DeletePicture(Path.GetFileName(new Uri(book.ImageLink).LocalPath));
                return new CrudResult() { StatusOperation = Status.Ok, Error = String.Empty };
            }
            else
            {
                return new CrudResult() { StatusOperation = Status.Error, Error = "Book doesn't exist" };
            }
        }

        public async Task<List<Books>> GetBooksWithSearchingAsync(Filters filter, CancellationToken cancellationToken)
        {
            List<Books> booksList = new();
            if (filter.Categories.Count > 0)
            {
                booksList = await (from books in _context.Books
                                   join catRel in _context.BookCategoryRelations on books.Id equals catRel.BookId
                                   join cat in _context.Categories on catRel.CategoryId equals cat.Id
                                   where filter.Categories.Contains(cat.Name)
                                   select new Books
                                   {
                                       Name = books.Name,
                                       Id = books.Id,
                                       Author = books.Author,
                                       NameNormalise = books.NameNormalise,
                                       Description = books.Description,
                                       Pages = books.Pages,
                                       PublishYear = books.PublishYear,
                                       ImageLink = books.ImageLink,
                                   }).ToListAsync(cancellationToken);
                booksList = booksList.Where(a => a.NameNormalise.Contains(filter.SearchString)).ToList();
            }
            else
            {
                booksList = await _context.Books.Where(a => a.NameNormalise.Contains(filter.SearchString)).ToListAsync(cancellationToken);
            }
            return booksList;
        }

    }
}
