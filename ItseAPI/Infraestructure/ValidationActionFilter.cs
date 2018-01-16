using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;

namespace ConventionalAndWhiteTariffCalculator.Infraestructure
{
    public class ValidationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //nothing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new ContentResult();
                string content = JsonConvert.SerializeObject(context.ModelState.Select(m => new
                {
                    Error = m.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                }),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                result.Content = content;
                result.ContentType = "application/json";

                context.HttpContext.Response.StatusCode = 400;
                context.Result = result;
            }
        }
    }
}