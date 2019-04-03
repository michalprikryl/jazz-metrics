using Newtonsoft.Json;
using WebAPI.Models.Error;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace WebAPI.Classes.Error
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            string module = "unknown", content = "none", headers = "none", method = "none";
            if (request != null)
            {
                method = request.Method.Method;
                module = request.RequestUri.AbsolutePath;
                content = await request.Content.ReadAsStringAsync();
                headers = JsonConvert.SerializeObject(request.Headers);
            }

            await ErrorManager.SaveErrorToDB(new ErrorModel(context.Exception, userID: "GLOBAL", message: $"module: {module} //// method: {method} //// content: {content} //// headers: {headers}"));
        }
    }
}