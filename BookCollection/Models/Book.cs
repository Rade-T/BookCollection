using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookCollection.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [RegularExpression(@"^[0-9]*$")]
        [StringLength(13)]
        public string ISBN { get; set; }

        public Genre Genre { get; set; }

        public int GenreID { get; set; }

        public Language Language { get; set; }

        public int LanguageID { get; set; }

        public List<Author> Authors { get; set; }

        [StringLength(140)]
        public string Description { get; set; }

        public Book()
        {
            this.Authors = new List<Author>();
        }
    }
}