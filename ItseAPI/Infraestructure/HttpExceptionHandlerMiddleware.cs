using ConventionalAndWhiteTariffCalculator.Infraestructure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculatorAPI.Infraestructure
{
    public class HttpExceptionHandlerMiddleware
    {
        readonly RequestDelegate next;
        public HttpExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (HttpException e)
            {
                context.Response.StatusCode = e.StatusCode;
                context.Response.ContentType = "application/json";

                byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ExceptionResult()
                {
                    Error = e.Body
                }));
                await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
