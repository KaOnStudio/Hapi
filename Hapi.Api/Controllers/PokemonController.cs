using System.Collections.Generic;
using System.Linq;
using Hapi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hapi.Api.Controllers
{
    [Route("api/[controller]")]
    public class PokemonController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly List<Pokemon> pokemons;
        public PokemonController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            var pokemonJson=System.IO.File.ReadAllText(_hostingEnvironment.WebRootPath + "/pokemon.json");
            pokemons = JsonConvert.DeserializeObject<List<Pokemon>>(pokemonJson);
        }


        [HttpGet,Authorize]
        public ResultModel<List<Pokemon>> Get()
        {
            return new ResultModel<List<Pokemon>>(pokemons);
        }

        [HttpGet("{id:int}"),Authorize]
        public ResultModel<Pokemon> Get(int id)
        {
            var pokemon = pokemons.FirstOrDefault(x => x.Id == id);
            return pokemon==null ? new ResultModel<Pokemon>($"{id} anahtarına ait pokemon bilgisi bulunamadı") : new ResultModel<Pokemon>(pokemon);
        }
    }
}