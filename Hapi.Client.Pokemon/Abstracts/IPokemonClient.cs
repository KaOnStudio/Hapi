using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hapi.Client.Pokemon.Abstracts
{
    public interface IPokemonClient
    {
        Task<List<Data.Models.Pokemon>> GetPokemonsAsync(CancellationToken cancellationToken);
        Task<List<Data.Models.Pokemon>> GetPokemonAsync(int id, CancellationToken cancellationToken);
    }
}