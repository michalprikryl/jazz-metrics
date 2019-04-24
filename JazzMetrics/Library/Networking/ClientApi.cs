using Library.Networking.HttpException;
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
        /// <summary>
        /// controller na serveru
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// vysledek posledniho volani vzdalene sluzby
        /// </summary>
        public string HTTPResultOfLastRequest { get; private set; }

        /// <summary>
        /// pristup k appsettings.json
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// nacita URL serveru z konfigu (appsettings.json)
        /// </summary>
        /// <param name="config">pristup k appsettings.json</param>
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
        public ClientApi(IConfiguration config, string controller)
        {
            Configuration = config;
            Controller = controller;
            ServerUrl = config["ServerApiUrl"];
        }

        /// <summary>
        /// vytvori HTTP Basic hlavicku
        /// </summary>
        /// <param name="username">prihlasovaci jmeno</param>
        /// <param name="password">heslo</param>
        /// <returns></returns>
        public static string GetHttpBasicHeader(string username, string password) => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

        /// <summary>
        /// posle HTTP POST pozadavek
        /// </summary>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <param name="mediaType">typ dat</param>
        /// <returns></returns>
        protected async Task PostToAPI(Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null, string mediaType = "application/json")
        {
            await PostToAPI(string.Empty, method, controller, endpoint, jwt, mediaType);
        }

        /// <summary>
        /// posle HTTP POST pozadavek
        /// </summary>
        /// <param name="content">telo pozadavku</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <param name="mediaType">typ dat</param>
        /// <returns></returns>
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
        /// posle HTTP PUT pozadavek
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <param name="content">telo pozadavku</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <param name="mediaType">typ dat</param>
        /// <returns></returns>
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

        /// <summary>
        /// posle HTTP PUT pozadavek
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <param name="model">data pozadavku</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <param name="mediaType">typ dat</param>
        /// <returns></returns>
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
        /// posle HTTP DELETE pozadavek
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <returns></returns>
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
        /// posle HTTP DELETE pozadavek (s daty v tele pozadavku!)
        /// </summary>
        /// <param name="content">telo pozadavku</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <param name="mediaType">typ dat</param>
        /// <returns></returns>
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

        /// <summary>
        /// posle HTTP GET pozadavek
        /// </summary>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <returns></returns>
        protected async Task GetToAPI(Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI(string.Empty, method, controller, endpoint, jwt);
        }

        /// <summary>
        /// posle HTTP GET pozadavek
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <returns></returns>
        protected async Task GetToAPI(int id, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI($"/{id}", method, controller, endpoint, jwt);
        }

        /// <summary>
        /// posle HTTP GET pozadavek
        /// </summary>
        /// <param name="parameters">parametry pridane k URL adrese</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <returns></returns>
        protected async Task GetToAPI(List<Tuple<string, string>> parameters, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI(GetParametersString(parameters), method, controller, endpoint, jwt);
        }

        /// <summary>
        /// posle HTTP GET pozadavek
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <param name="parameters">parametry pridane k URL adrese</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <returns></returns>
        protected async Task GetToAPI(int id, List<Tuple<string, string>> parameters, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI($"/{id}{GetParametersString(parameters)}", method, controller, endpoint, jwt);
        }

        /// <summary>
        /// posle HTTP GET pozadavek
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <param name="entity">entita pod hlavni entitou</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <returns></returns>
        protected async Task GetToAPI(int id, string entity, Func<HttpResponseMessage, Task> method, string controller = null, string endpoint = "", string jwt = null)
        {
            await GetToAPI($"/{id}/{entity}", method, controller, endpoint, jwt);
        }

        /// <summary>
        /// posle HTTP GET pozadavek
        /// </summary>
        /// <param name="queryString">kompletni query string URL adresy</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="controller">controller, na ktery chci pozadavek poslat</param>
        /// <param name="endpoint">endpoint controlleru, na ktery chci pozadavek poslat</param>
        /// <param name="jwt">JWT pro pristup</param>
        /// <returns></returns>
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

        /// <summary>
        /// vytvori string z parametru URL -> query string
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

        /// <summary>
        /// vytvori parametr pro GET
        /// </summary>
        /// <param name="name">nazev</param>
        /// <param name="value">ciselna hodnota</param>
        /// <returns></returns>
        protected Tuple<string, string> GetParameter(string name, int value)
        {
            return GetParameter(name, value.ToString());
        }

        /// <summary>
        /// vytvori parametr pro GET
        /// </summary>
        /// <param name="name">nazev</param>
        /// <param name="value">hodnota</param>
        /// <returns></returns>
        protected Tuple<string, string> GetParameter(string name, string value)
        {
            return new Tuple<string, string>(name, value);
        }

        /// <summary>
        /// vytvori seznam prametru
        /// </summary>
        /// <param name="parameters">jednotlive parametry</param>
        /// <returns></returns>
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
        /// <param name="client">http client</param>
        /// <param name="jwt">JWT pro pristup</param>
        private void AdditionalHeaders(HttpClient client, string jwt)
        {
            if (!string.IsNullOrEmpty(jwt))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }
        }

        /// <summary>
        /// nastaveni vysledku -> metoda volana vzdy pro prijeti vysledku
        /// </summary>
        /// <param name="result">vysledek volani</param>
        /// <param name="method">metoda pro spusteni</param>
        /// <returns></returns>
        private async Task SetResult(HttpResponseMessage result, Func<HttpResponseMessage, Task> method)
        {
            if (result.StatusCode == HttpStatusCode.Forbidden) //403
            {
                throw new ForbiddenException();
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized) //401
            {
                throw new UnauthorizedException();
            }
            else if (result.StatusCode == HttpStatusCode.NotFound) //404
            {
                throw new NotFoundException();
            }

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
