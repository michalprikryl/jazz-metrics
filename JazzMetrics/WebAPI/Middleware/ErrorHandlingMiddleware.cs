using Library;
using Library.Models;
using Library.Models.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Services.Helper;

namespace WebAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHelperService helperService, IConfiguration configuration)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(helperService, context, ex, configuration["Version"]);
            }
        }

        private static Task HandleExceptionAsync(IHelperService helperService, HttpContext context, Exception exception, string version)
        {
            Task error = helperService.SaveErrorToDB(new ErrorModel(exception, $"JazzMetricsAPI - {version} -> {context.User.GetId()}", "global error handler"));

            string result = JsonConvert.SerializeObject(new BaseResponseModel
            {
                Message = "Error occured on server within request processing.",
                Success = false
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            error.Wait();

            return context.Response.WriteAsync(result);
        }
    }
}
