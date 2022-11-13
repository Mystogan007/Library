using Library.Models.Book;
using System.Collections.Generic;

namespace Library.Models.ApiResponses
{
    public class ApiResponseBooks : ApiResponse
    {
        public List<Books> Books { get; set; }
    }
}
