﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Classes
{
    /// <summary>
    /// abstraktni trida, ktera nabizi metody, ktere je mozne pouzit pro praci s HTTP API
    /// </summary>
    public abstract class ClientAPI
    {
        protected readonly string URL;

        private AuthenticationHeaderValue _authHeader;

        /// <summary>
        /// na jaky controller se pozadavky posilaji
        /// </summary>
        public string Controller { get; private set; }
        /// <summary>
        /// znaci, zda pri pozadavku na vzdaleny zdroj pouzit autentifikacni hlavicku (defaultne se pouziva)
        /// </summary>
        public bool UseAuthetificationHeader { get; set; }
        /// <summary>
        /// vysledek posledniho volani vzdalene sluzby
        /// </summary>
        public string HTTPResultOfLastReques { get; set; }
        /// <summary>
        /// vrati kompletni URL i s controllerem
        /// </summary>
        public string URLWithController { get { return $"{URL}/{Controller}"; } }

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="controller">na jaky controller API se maji data zaslat</param>
        /// <param name="useAuthetificationHeader">zda se ma pouzit autentifikacni hlavicka</param>
        /// <param name="jwt">JSON Web token, ktery urcuje druh autentifikacni hlavicky (pokud se pouzije - dle parametru vyse)</param>
        public ClientAPI(string controller, bool useAuthetificationHeader, string jwt)
        {
            Controller = controller;

            _authHeader = SetHttpAuthHeader(jwt);

            UseAuthetificationHeader = useAuthetificationHeader;

            URL = ConfigurationManager.AppSettings["API"];
        }

        /// <summary>
        /// posle HTTP POST pozadavek na dany controller
        /// </summary>
        /// <param name="content">content pozadavku ve formatu danem dalsim parametrem</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="mediaType">jaky chci urcit media type pozadavku - defaultne "application/json"</param>
        protected async Task PostToAPI(string content, Action<Task<HttpResponseMessage>> method, string mediaType = "application/json")
        {
            StringContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);

            using (HttpClient client = new HttpClient())
            {
                AuthetificatonHeader(client);

                await client.PostAsync(URLWithController, httpContent).ContinueWith(task =>
                {
                    SetResult(task.Result);

                    method(task);
                });
            }
        }

        /// <summary>
        /// posle HTTP PUT pozadavek na dany controller
        /// </summary>
        /// <param name="content">content pozadavku ve formatu danem dalsim parametrem</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        /// <param name="mediaType">jaky chci urcit media type pozadavku - defaultne "application/json"</param>
        protected async Task PutToAPI(string content, Action<Task<HttpResponseMessage>> method, string mediaType = "application/json")
        {
            StringContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);

            using (HttpClient client = new HttpClient())
            {
                AuthetificatonHeader(client);

                await client.PutAsync(URLWithController, httpContent).ContinueWith(task =>
                {
                    SetResult(task.Result);

                    method(task);
                });
            }
        }

        /// <summary>
        /// posle HTTP DELETE pozadavek na dany controller
        /// </summary>
        /// <param name="id">id entity, kterou chci smazat</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        protected async Task DeleteToAPI(int? id, Action<Task<HttpResponseMessage>> method)
        {
            string requestUri = id.HasValue ? $"{URLWithController}/{id.Value}" : $"{URL}/{Controller}";

            using (HttpClient client = new HttpClient())
            {
                AuthetificatonHeader(client);

                await client.DeleteAsync(requestUri).ContinueWith(task =>
                {
                    SetResult(task.Result);

                    method(task);
                });
            }
        }

        /// <summary>
        /// posle HTTP DELETE pozadavek na dany controller (s daty v tele pozadavku!)
        /// </summary>
        /// <param name="id">id entity, kterou chci smazat</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        protected async Task DeleteToAPI(string content, Action<Task<HttpResponseMessage>> method, string mediaType = "application/json")
        {
            StringContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);

            using (HttpClient client = new HttpClient())
            {
                AuthetificatonHeader(client);

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = httpContent,
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(URLWithController)
                };

                await client.SendAsync(request).ContinueWith(task =>
                {
                    SetResult(task.Result);

                    method(task);
                });
            }
        }

        /// <summary>
        /// posle HTTP GET pozadavek na dany controller
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="method">metoda, kterou chci spustit po ziskani dat z API</param>
        protected async Task GetToAPI(List<Tuple<string, string>> parameters, Action<Task<HttpResponseMessage>> method)
        {
            StringBuilder builder = new StringBuilder($"{URLWithController}");

            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    builder.Append("?");
                }

                foreach (var item in parameters)
                {
                    builder.Append($"{item.Item1}={item.Item2}&");
                }

                if (parameters.Count > 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }

            using (HttpClient client = new HttpClient())
            {
                AuthetificatonHeader(client);

                await client.GetAsync(builder.ToString()).ContinueWith(task =>
                {
                    SetResult(task.Result);

                    method(task);
                });
            }
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

        private void AuthetificatonHeader(HttpClient client)
        {
            if (UseAuthetificationHeader)
            {
                client.DefaultRequestHeaders.Authorization = _authHeader;
            }
        }

        private void SetResult(HttpResponseMessage result)
        {
            HTTPResultOfLastReques = $"{(int)result.StatusCode} - {Enum.GetName(typeof(HttpStatusCode), result.StatusCode)}";
        }

        private AuthenticationHeaderValue SetHttpAuthHeader(string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
            {
                string username = ConfigurationManager.AppSettings["txt"].DecodeFromBase64();
                string password = ConfigurationManager.AppSettings["setting"].DecodeFromBase64();

                byte[] byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }
            else
            {
                return new AuthenticationHeaderValue("Bearer", jwt);
            }
        }
    }
}