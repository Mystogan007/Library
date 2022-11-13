using Library.Models;
using Library.Models.ApiResponses;
using Library.Models.Book;
using Library.Models.CRUDResult;
using Library.Models.Othres;
using Library.Repository;
using Library.Storage;
using Library.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        IBooksRepository _booksRepository;
        ICategoryRepository _categoryRepository;
        IFavouriteRepository _favRepository;
        IConfiguration _iconfiguration;


        private CancellationTokenSource _cancellationTokenSource;
        public BooksController(ApplicationContext context, IConfiguration iconfiguration)
        {
            _favRepository = new PgFavouriteRepository(context);
            _categoryRepository = new PgCategoryRepository(context);
            _booksRepository = new PgBooksRepository(context);
            _iconfiguration = iconfiguration;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        #region admin
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ApiResponse> DeleteBook([FromRoute] string id)
        {
            try
            {
                var res = await _booksRepository.DeleteBookAsync(id, _cancellationTokenSource.Token);
                if (res.StatusOperation == Status.Ok)
                {
                    return new ApiResponse { Status = "Ok", Error = res.Error };
                }
                else
                {
                    return new ApiResponse { Status = "Error", Error = res.Error };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse { Status = "Error", Error = "Internal server error" };
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("new")]
        public async Task<ApiResponse> AddBook([FromBody] BooksForm bookForm)
        {
            try
            {
                var res = await _booksRepository.AddBookAsync(bookForm, _categoryRepository, _iconfiguration["Host"], _cancellationTokenSource.Token);
                if (res.StatusOperation == Status.Ok)
                {
                    return new ApiResponse { Status = "Ok", Error = res.Error };
                }
                else
                {
                    return new ApiResponse { Status = "Error", Error = res.Error };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse { Status = "Error", Error = "Internal server error" };
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("change")]
        public async Task<ApiResponse> ChangeBook([FromBody] BooksForm book)
        {
            try
            {
                var res = await _booksRepository.ChangeBookAsync(book, _categoryRepository, _iconfiguration["Host"], _cancellationTokenSource.Token);
                if (res.StatusOperation == Status.Ok)
                {
                    return new ApiResponse { Status = "Ok", Error = res.Error };
                }
                else
                {
                    return new ApiResponse { Status = "Error", Error = res.Error };
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse { Status = "Error", Error = "Internal server error" };
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("cat/delete")]
        public async Task<ApiResponse> DeleteCategory([FromBody] string name)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(name, _booksRepository, _cancellationTokenSource.Token);
                return new ApiResponse { Status = "Ok", Error = string.Empty };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse { Status = "Error", Error = "Internal server error" };
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("cat/new")]
        public async Task<ApiResponse> AddCategory([FromBody] string name)
        {
            try
            {
                await _categoryRepository.AddCategoryAsync(name, _cancellationTokenSource.Token);
                return new ApiResponse { Status = "Ok", Error = string.Empty };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse { Status = "Error", Error = "Internal server error" };
            }
        }
        #endregion

        #region user
        [Authorize]
        [HttpGet]
        [Route("favourite/delete")]
        public async Task<ApiResponse> DeleteFav(FavouritesRelations relations)
        {
            try
            {
                await _favRepository.DeleteFavouriteAsync(relations, _cancellationTokenSource.Token);
                return new ApiResponse { Status = "Ok", Error = string.Empty };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse { Status = "Error", Error = "Internal server error" };
            }
        }

        [Authorize]
        [HttpPost]
        [Route("favourite/add")]
        public async Task<ApiResponse> AddFav(FavouritesRelations relations)
        {
            try
            {
                await _favRepository.AddFavouriteAsync(relations, _cancellationTokenSource.Token);
                return new ApiResponse { Status = "Ok", Error = string.Empty };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse { Status = "Error", Error = "Internal server error" };
            }
        }
        #endregion

        [Authorize]
        [HttpGet]
        [Route("search")]
        public async Task<ApiResponseBooks> SearchBooks(Filters filters)
        {
            try
            {
                if (filters == null)
                {
                    return new ApiResponseBooks()
                    {
                        Status = "Error",
                        Error = "Bad request",
                        Books = new List<Books>()
                    };
                }

                if (string.IsNullOrEmpty(filters.SearchString) && filters.Categories == null)
                {
                    return new ApiResponseBooks()
                    {
                        Status = "Error",
                        Error = "Bad request",
                        Books = new List<Books>()
                    };
                }


                var books = await _booksRepository.GetBooksWithSearchingAsync(filters, _cancellationTokenSource.Token);
                if (books.Count > 0)
                {
                    return new ApiResponseBooks()
                    {
                        Books = books,
                        Status = "Ok",
                        Error = String.Empty
                    };
                }
                else
                {
                    return new ApiResponseBooks()
                    {
                        Status = "Error",
                        Error = "Nothing found",
                        Books = new List<Books>()
                    };
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponseBooks()
                {
                    Status = "Error",
                    Error = "Internal server error",
                    Books = new List<Books>()
                };
            }
        }

        [Authorize]
        [HttpGet]
        [Route("image/{name}")]
        public async Task<IActionResult> GetPicture([FromRoute] string name)
        {
            try
            {
                var image = await ImageHelper.GetPicture(name);
                if (image == null)
                {
                    return BadRequest("Such file doesn't exist");
                }
                return File(image, "image/jpeg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

    }
}
