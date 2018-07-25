using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hapi.Client.Abstracts;
using Hapi.Client.Book.Abstracts;
using Hapi.Common.Abstracts;
using RestSharp;
using ResultModel=Hapi.Data.Models;
namespace Hapi.Client.Book
{
    public class BookClient:ClientBase,IBookClient
    {
        public BookClient(IDependencyContext context,ISettings settings):base(context,settings)
        {
            
        }
        public async Task<IEnumerable<ResultModel.Book>> GetBooksAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await RequestAsync<List<ResultModel.Book>>("/api/book", Method.GET, true,cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception("Kitaplar getirilemedi!",e);
            }
        }
        public IEnumerable<ResultModel.Book> GetBooks()
        {
            try
            {
                return Request<List<ResultModel.Book>>("/api/book", Method.GET, true);
            }
            catch (Exception e)
            {
                throw new Exception("Kitaplar getirilemedi!",e);
            }
        }
    }
}
