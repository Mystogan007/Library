using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Library.Models.Book
{
    public class BooksForm
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public int PublishYear { get; set; }

        public int Pages { get; set; }

        public List<string> CategoryIds { get; set; }
    }
}
