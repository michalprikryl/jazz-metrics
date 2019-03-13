using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Error;
using WebAPI.Services.Error;

namespace WebAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IErrorService errorService, IConfiguration configuration)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(errorService, context, ex, configuration["Version"]);
            }
        }

        private static Task HandleExceptionAsync(IErrorService errorService, HttpContext context, Exception exception, string version)
        {
            Task error = errorService.SaveErrorToDB(new ErrorModel(exception, $"JazzMetricsAPI - {version} -> {context.User.GetId()}", "global error handler"));

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
