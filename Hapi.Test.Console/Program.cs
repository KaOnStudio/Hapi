using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hapi.Client.Abstracts;
using Hapi.Client.Book.Abstracts;
using Hapi.Client.Book.Extensions;
using Hapi.Client.Pokemon.Abstracts;
using Hapi.Client.Pokemon.Extensions;
using Hapi.Common.Extensions;
using Hapi.Data.Models;
using Microsoft.Extensions.DependencyInjection;
namespace Hapi.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services=new ServiceCollection();
            services
                .AddDependencyInfrastructure() //Temel Dependency kütüphanesi ekleniyor
                .AddBookClient() //Kitap Client Servisi Ekleniyor
                .AddPokemonClient(); //Pokemon Client Servisi Ekleniyor
            ServiceProvider provider = services.BuildServiceProvider();
            
            ISettings settings = provider.GetService<ISettings>();
            settings.ApiPath = "http://localhost:19714/";
            settings.AppLoginModel = new AppLoginModel
            {
                Username = "hasan",
                Password = "123654"
            };

            IBookClient bookClient = provider.GetService<IBookClient>();
            //Kitaplara getiriliyor
            var books = bookClient.GetBooks();
            System.Console.WriteLine($"{books.Count()} kitap verisi bulundu!");
            System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(books));

            IPokemonClient pokemonClient = provider.GetService<IPokemonClient>();
            //pokemonlar getiriliyor
            var task = Task.Run(async () => await pokemonClient.GetPokemonsAsync(CancellationToken.None));
            task.Wait();
            var pokemons = task.Result.ToList();
            System.Console.WriteLine($"{pokemons.Count()} pokemon verisi bulundu!");

            //1 anahtarına ait pokemon bilgisi getiriliyor
            task = Task.Run(async () => await pokemonClient.GetPokemonAsync(1,CancellationToken.None));
            task.Wait();
            var pokemon = task.Result;
            System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(pokemon));


            System.Console.ReadKey();
        }
    }
}
