using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookCollection.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public int NationalityID { get; set; }

        public Nationality Nationality { get; set; }
    }
}