using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BUYBOATSUS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoatsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public static string KEY { get; } = "qydfsmmky7ytansbb3s38dgcvzu8u2";

        public BoatsController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient();
        }

        [HttpGet]
        public async Task<ActionResult> Get(bool active = true)
        {

            // handle filtering (not needed for now)
            // model validation etc...
                
            // make call to api.boats.com
            var response = await _httpClient.GetAsync($"https://api.boats.com/inventory/search?key={KEY}&salesstatus={(active ? "sale pending" : string.Empty)}");
            var response2 = await _httpClient.GetAsync($"https://api.boats.com/inventory/search?key={KEY}&salesstatus={(active ? "active" : string.Empty)}");

            var stream = await response.Content.ReadAsStreamAsync();
            var stream2 = await response2.Content.ReadAsStreamAsync();

            // return response
            return Ok(new { pending = await JsonSerializer.DeserializeAsync<dynamic>(stream), active = await JsonSerializer.DeserializeAsync<dynamic>(stream2) });
        }
    }
}
    