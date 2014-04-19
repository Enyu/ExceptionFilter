using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using DPE.LogLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DPE.ExceptionFilter
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ExceptionHelper _exceptionHelper;

        public ExceptionFilter(ExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public override void OnException(HttpActionExecutedContext actionContext)
        {
            _exceptionHelper.LogError(actionContext.Request, actionContext.Exception);
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

    }
    public class ExceptionHelper
    {
        private MagicLogger _magicLogger;
        public MagicLogger Log
        {
            get { return _magicLogger ?? new MagicLogger(); }
            set { _magicLogger = value; }
        }

        public virtual void LogError(HttpRequestMessage request, Exception exception)
        {
            request.Content.ReadAsStreamAsync().Result.Position = 0;
            var entity = request.Content.ReadAsStringAsync().Result;
            
            var dict = new Dictionary<string, object>
            {
                {"RequestId", Guid.NewGuid()},
                {"RequestUrl", request.RequestUri.OriginalString},
                {"Headers", Serialize(request.Headers.ToDictionary(i => i.Key, i => i.Value))},
                {"Params", Serialize(GetParams(request))},
                {"RequestBody", entity}
            };

            Log.WriteError(exception.Message, dict);
        }

        private static Dictionary<string, string> GetParams(HttpRequestMessage request)
        {
            if (!request.Properties.ContainsKey("MS_HttpContext")) 
                return null;

            var items = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.ServerVariables;
            return items.Cast<string>().Select(s => new { Key = s, Value = items[s] }).ToDictionary(p => p.Key, p => p.Value);
        }

        private static string Serialize(object item)
        {
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.RoundtripKind } },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var data = new StringBuilder();
            using (var jsonTextWriter = new JsonTextWriter(new StringWriter(data)))
            {
                jsonSerializer.Serialize(jsonTextWriter, item);
            }
            return data.ToString();
        }
    }
}
