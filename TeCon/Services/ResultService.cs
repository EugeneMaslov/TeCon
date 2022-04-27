using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeCon.Models;

namespace TeCon.Services
{
    public class ResultService
    {
        const string Url = "https://tedevelopment.herokuapp.com/api/Result/";
        readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
        public async Task<IEnumerable<Result>> Get(int testId)
        {
            HttpClient client = GetClient();
            var x = await client.GetAsync(Url + "result/by_id/" + testId);
            if (x.IsSuccessStatusCode)
            {
                string result = await client.GetStringAsync(Url + "result/by_id/" + testId);
                return JsonSerializer.Deserialize<IEnumerable<Result>>(result, options);
            }
            else return null;
        }
    }
}
