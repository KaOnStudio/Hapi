using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hapi.Client.Abstracts;
using Hapi.Client.Pokemon.Abstracts;
using Hapi.Common.Abstracts;
using RestSharp;

namespace Hapi.Client.Pokemon
{
    public class PokemonClient:ClientBase,IPokemonClient
    {
        public PokemonClient(IDependencyContext context,ISettings settings):base(context,settings)
        {
            
        }

        public async Task<List<Data.Models.Pokemon>> GetPokemonsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await RequestAsync<List<Data.Models.Pokemon>>("/api/pokemon", Method.GET, true, cancellationToken);
            }
            catch (Exception e)
            {
                throw  new Exception("Pokemon bilgisi getirilemedi",e);
            }
        }
        public async Task<List<Data.Models.Pokemon>> GetPokemonAsync(int id,CancellationToken cancellationToken)
        {
            try
            {
                
                return await RequestAsync<List<Data.Models.Pokemon>>($"/api/pokemon/{id}", Method.GET, true, cancellationToken);
            }
            catch (Exception e)
            {
                throw  new Exception("Pokemon bilgisi getirilemedi",e);
            }
        }
    }
}
