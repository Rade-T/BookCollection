using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCollection.Models
{
    public class BookAuthors
    {
        public int AuthorID { get; set; }

        public int BookID { get; set; }

        public string Name { get; set; }

        public bool Assigned { get; set; }
    }
}