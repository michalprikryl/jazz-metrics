using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Library.Networking
{
    /// <summary>
    /// abstraktni trida, ktera nabizi metody, ktere je mozne pouzit pro praci s HTTP API
    /// </summary>
    public abstract class ClientApi
    {
        /// <summary>
        /// URL serveru
        /// </summary>
        public string ServerUrl { get; set; }
        public string Controller { get; set; }
        /// <summary>
        /// vysledek posledniho volani vzdalene sluzby
        /// </summary>
        public string HTTPResultOfLastRequest { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public ClientApi(string serverUrl, string controller)
        {
            ServerUrl = serverUrl;
            Controller = controller;
        }

        public ClientApi(IConfiguration config)
        {
            Configuration = config;
            ServerUrl = config["ServerApiUrl"];
        }

        /// <summary>
        /// nacte URL serveru z appsetting.json
        /// </summary>
        /// <param name="config">config</param>
        /// <param name="controller">controller, na ktery se posila</param>
        /// <param name="jwt">jwt</param>
        public ClientApi(IConfiguration config, string controller)
        {
            Configuration = config;
            Controller = controller;
            ServerUrl = config["ServerApiUrl"];
        }

        public static string GetHttpBasicHeader(string username, string password) => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

        protected async Task PostToAPI(Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null, string mediaType = "application/json")
        {
            await PostToAPI(string.Empty, method, controller, endpoint, jwt, mediaType);
        }

        /// <summary>
        /// posle HTTP POST pozadavek na dany controller
        /// </summary>
        /// <param name="content">content pozadavku ve formatu danem dalsim parametrem</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="mediaType">jaky chci urcit media type pozadavku - defaultne "application/json"</param>
        protected async Task PostToAPI(string content, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null, string mediaType = "application/json")
        {
            StringContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);

            using (HttpClient client = new HttpClient())
            {
                AdditionalHeaders(client, jwt);

                HttpResponseMessage response = await client.PostAsync($"{ServerUrl}/{controller ?? Controller}/{endpoint}", httpContent);

                await SetResult(response, method);
            }
        }

        /// <summary>
        /// posle HTTP PUT pozadavek na dany controller
        /// </summary>
        /// <param name="content">content pozadavku ve formatu danem dalsim parametrem</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="mediaType">jaky chci urcit media type pozadavku - defaultne "application/json"</param>
        protected async Task PutToAPI(int id, string content, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null, string mediaType = "application/json")
        {
            StringContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);

            if (!string.IsNullOrEmpty(endpoint))
            {
                endpoint = $"/{endpoint}";
            }

            using (HttpClient client = new HttpClient())
            {
                AdditionalHeaders(client, jwt);

                HttpResponseMessage response = await client.PutAsync($"{ServerUrl}/{controller ?? Controller}{endpoint}/{id}", httpContent);

                await SetResult(response, method);
            }
        }

        protected async Task PatchToAPI(int id, List<PatchModel> model, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null, string mediaType = "application/json")
        {
            StringContent httpContent = new StringContent(SerializeObjectToJSON(model), Encoding.UTF8, mediaType);

            if (!string.IsNullOrEmpty(endpoint))
            {
                endpoint = $"/{endpoint}";
            }

            using (HttpClient client = new HttpClient())
            {
                AdditionalHeaders(client, jwt);

                HttpResponseMessage response = await client.PatchAsync($"{ServerUrl}/{controller ?? Controller}{endpoint}/{id}", httpContent);

                await SetResult(response, method);
            }
        }

        /// <summary>
        /// posle HTTP DELETE pozadavek na dany controller
        /// </summary>
        /// <param name="id">id entity, kterou chci smazat</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        protected async Task DeleteToAPI(int? id, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            if (!string.IsNullOrEmpty(endpoint))
            {
                endpoint = $"/{endpoint}";
            }

            string requestUri = id.HasValue ? $"{ServerUrl}/{controller ?? Controller}{endpoint}/{id.Value}" : $"{ServerUrl}/{controller ?? Controller}{endpoint}";

            using (HttpClient client = new HttpClient())
            {
                AdditionalHeaders(client, jwt);

                HttpResponseMessage response = await client.DeleteAsync(requestUri);

                await SetResult(response, method);
            }
        }

        /// <summary>
        /// posle HTTP DELETE pozadavek na dany controller (s daty v tele pozadavku!)
        /// </summary>
        /// <param name="id">id entity, kterou chci smazat</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        protected async Task DeleteToAPI(string content, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null, string mediaType = "application/json")
        {
            StringContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);

            using (HttpClient client = new HttpClient())
            {
                AdditionalHeaders(client, jwt);

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = httpContent,
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"{ServerUrl}/{controller ?? Controller}/{endpoint}")
                };

                HttpResponseMessage response = await client.SendAsync(request);

                await SetResult(response, method);
            }
        }

        protected async Task GetToAPI(Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI(string.Empty, method, controller, endpoint, jwt);
        }

        protected async Task GetToAPI(int id, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI($"/{id}", method, controller, endpoint, jwt);
        }

        /// <summary>
        /// posle HTTP GET pozadavek na dany controller
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        protected async Task GetToAPI(List<Tuple<string, string>> parameters, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI(GetParametersString(parameters), method, controller, endpoint, jwt);
        }

        protected async Task GetToAPI(int id, List<Tuple<string, string>> parameters, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI($"/{id}{GetParametersString(parameters)}", method, controller, endpoint, jwt);
        }

        protected async Task GetToAPI(int id, string entity, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI($"/{id}/{entity}", method, controller, endpoint, jwt);
        }

        protected async Task GetToAPI(string queryString, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            if (!string.IsNullOrEmpty(endpoint))
            {
                endpoint = $"/{endpoint}";
            }

            using (HttpClient client = new HttpClient())
            {
                AdditionalHeaders(client, jwt);

                HttpResponseMessage response = await client.GetAsync($"{ServerUrl}/{controller ?? Controller}{endpoint}{queryString}");

                await SetResult(response, method);
            }
        }

        protected string GetParametersString(List<Tuple<string, string>> parameters)
        {
            StringBuilder builder = new StringBuilder();

            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    builder.Append("?");
                    foreach (var item in parameters)
                    {
                        builder.Append($"{item.Item1}={item.Item2}&");
                    }
                    builder.Remove(builder.Length - 1, 1);
                }
            }

            return builder.ToString();
        }

        protected Tuple<string, string> GetParameter(string name, int value)
        {
            return GetParameter(name, value.ToString());
        }

        protected Tuple<string, string> GetParameter(string name, string value)
        {
            return new Tuple<string, string>(name, value);
        }

        protected List<Tuple<string, string>> GetParametersList(params Tuple<string, string>[] parameters)
        {
            return parameters.ToList();
        }

        /// <summary>
        /// serializuje objekt do JSON retezce
        /// </summary>
        /// <param name="obj">objekt k serializaci</param>
        /// <returns></returns>
        protected string SerializeObjectToJSON(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// prida dalsi hlavicky
        /// </summary>
        /// <param name="client"></param>
        private void AdditionalHeaders(HttpClient client, string jwt)
        {
            if (!string.IsNullOrEmpty(jwt))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }
        }

        private async Task SetResult(HttpResponseMessage result, Func<HttpResponseMessage, Task> method)
        {
            await method(result);
            HTTPResultOfLastRequest = $"{(int)result.StatusCode} - {Enum.GetName(typeof(HttpStatusCode), result.StatusCode)}";
        }

        /// <summary>
        /// 'dekoduje' retezec z non-human readable retezce (base64)
        /// </summary>
        /// <param name="base64EncodedData">string 'zakodovany' v base64</param>
        /// <returns></returns>
        private string DecodeFromBase64(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
