using System.ComponentModel.DataAnnotations;

namespace Asp.NetWebAPI.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Genre { get; set; }

        // Foreign Key
        public int AuthorId { get; set; }

        // Navigation property
        public Author Author { get; set; }
    }

    public class BookDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string AuthorName { get; set; }
    }

    public class BookDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Genre { get; set; }
        public string AuthorName { get; set; }
    }
}