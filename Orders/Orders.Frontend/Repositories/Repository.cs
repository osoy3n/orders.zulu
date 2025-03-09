﻿using System.Text;
using System.Text.Json;

namespace Orders.Frontend.Repositories
{
    public class Repository(HttpClient httpClient) : IRepository
    {
        private static JsonSerializerOptions JsonDefaultOptions => new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp = await httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<T>(responseHttp);
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, messageContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TActionResponse>(responseHttp);
                return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TActionResponse>(default, true, responseHttp);
        }

        private static async Task<T> UnserializeAnswer<T>(HttpResponseMessage responseHttp)
        {
            var response = await responseHttp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, JsonDefaultOptions)!;
        }
    }
}
