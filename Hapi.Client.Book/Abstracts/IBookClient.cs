using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hapi.Client.Book.Abstracts
{
    public interface IBookClient
    {
        Task<IEnumerable<Data.Models.Book>> GetBooksAsync(CancellationToken cancellationToken);
        IEnumerable<Data.Models.Book> GetBooks();
    }
}
