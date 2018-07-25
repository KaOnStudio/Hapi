using System.Collections.Generic;
using Hapi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hapi.Api.Controllers
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly List<Book> _books;
        public BookController()
        {
            _books = new List<Book>
            {
                new Book {Author = "Ray Bradbury", Title = "Fahrenheit 451"},
                new Book {Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude"},
                new Book {Author = "George Orwell", Title = "1984"},
                new Book {Author = "Anais Nin", Title = "Delta of Venus"}
            };
        }

        [HttpGet,Authorize]
        public ResultModel<List<Book>> Get()
        {
            return new ResultModel<List<Book>>(_books);
        }
    }
}
