using System;
using System.Collections.Generic;

namespace Library.Api.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int AuthorId { get; set; }
        public string? Editorial { get; set; }
        public int GenreId { get; set; }

        public virtual Author Author { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
    }
}
