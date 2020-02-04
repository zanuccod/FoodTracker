using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Common.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<T> GetAsync<T>(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            await HandleResponse(response);
            var serialized = await response.Content.ReadAsStringAsync();

            var result = await Task.Run(() => 
                JsonConvert.DeserializeObject<T>(serialized, _serializerSettings));

            return result;
        }

        public async Task<T> PostAsync<T>(string uri, T data, string token = "", string header = "")
        {
            var httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            var serialized = await response.Content.ReadAsStringAsync();

            var result = await Task.Run(() =>
                JsonConvert.DeserializeObject<T>(serialized, _serializerSettings));

            return result;
        }

        public async Task<T> PostAsync<T>(string uri, string data, string clientId, string clientSecret)
        {
			var httpClient = CreateHttpClient(string.Empty);

            if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
			{
                AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
			}

            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
			var response = await httpClient.PostAsync(uri, content);

			await HandleResponse(response);
			var serialized = await response.Content.ReadAsStringAsync();

			var result = await Task.Run(() =>
				JsonConvert.DeserializeObject<T>(serialized, _serializerSettings));

			return result;
        }

        public async Task<T> PutAsync<T>(string uri, T data, string token = "", string header = "")
        {
            var httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PutAsync(uri, content);

            await HandleResponse(response);
            var serialized = await response.Content.ReadAsStringAsync();

            var result = await Task.Run(() =>
                JsonConvert.DeserializeObject<T>(serialized, _serializerSettings));

            return result;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            var httpClient = CreateHttpClient(token);
            await httpClient.DeleteAsync(uri);
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private void AddBasicAuthenticationHeader(HttpClient httpClient, string clientId, string clientSecret)
        {
			if (httpClient == null)
				return;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
				return;

            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || 
				    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new AuthenticationException(content);
                }
                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }
    }
}