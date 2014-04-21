using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace DPE.ExceptionFilter
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ExceptionHelper _exceptionHelper;

        public ExceptionFilter()
        {
            _exceptionHelper = new ExceptionHelper();
        }

        public override void OnException(HttpActionExecutedContext actionContext)
        {
            _exceptionHelper.LogError(actionContext.Request, actionContext.Exception);
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
