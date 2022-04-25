using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeCon.Models;

namespace TeCon.Services
{
    class VarientsService
    {
        const string Url = "https://tedevelopment.herokuapp.com/api/Varients/";
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
        public async Task<IEnumerable<Varient>> GetVarientByQuestId(int id)
        {
            HttpClient client = GetClient();
            var x = await client.GetAsync(Url + id);
            if (x.IsSuccessStatusCode)
            {
                string result = await client.GetStringAsync(Url + id);
                return JsonSerializer.Deserialize<IEnumerable<Varient>>(result, options);
            }
            else return null;
        }
        public async Task<Varient> Add(Varient varient)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonSerializer.Serialize(varient),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<Varient>(
                await response.Content.ReadAsStringAsync(), options);
        }
        public async Task<Varient> Update(Varient varient)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url,
                new StringContent(
                    JsonSerializer.Serialize(varient),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<Varient>(
                await response.Content.ReadAsStringAsync(), options);
        }
        public async Task<Varient> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<Varient>(
               await response.Content.ReadAsStringAsync(), options);
        }
    }
}
