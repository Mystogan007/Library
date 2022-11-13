using System.Collections.Generic;

namespace Library.Models.Book
{
    public class Filters
    {
        public string SearchString { get; set; }

        public List<string> Categories { get; set; }
    }
}
