using Newtonsoft.Json.Serialization;
using WebAPI.Authentification;
using WebAPI.Classes.Error;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Cors;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors(new EnableCorsAttribute("http://localhost:3000", "*", "*"));

            config.Formatters.Clear(); //smaze vsechny navratove formaty dat z API - defaultne vraci podle typu zaslanych dat XML/JSON
            config.Formatters.Add(new JsonMediaTypeFormatter()); //ja vracim pouze JSON

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //prevede vsechny navracene properties na camelCase
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;


            config.Filters.Add(new IdentityBasicAuthenticationAttribute()); //konfigurace custom identity pro kontrolu prihlasovani pomoci basic HTTP autentifikace

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler()); //zachytavani chyb na globalni urovni

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
