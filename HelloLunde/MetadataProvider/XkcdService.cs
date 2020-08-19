using MetadataProvider.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetadataProvider
{
    public class XkcdService
    {
        public HttpClient Client { get; }
        public XkcdService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://xkcd.com/");

            Client = client;
        }

        public async Task<ComicItem> GetComic(int number)
        {
            var response = await Client.GetAsync($"/{number}/info.0.json");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ComicItem>(responseStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
        }
    }
}
