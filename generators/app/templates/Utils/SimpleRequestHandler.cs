using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace <%= ProjectName %>.Utils
{
    public class SimpleRequestHandler: IRequestsHandler
    {
        public readonly ILog _log;

        public SimpleRequestHandler(ILog log)
        {
            _log = log;
        }

        public JsonResult JsonHandler(HttpRequest request, HttpResponse response, Func<JsonReturn> action)
        {
            try
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                return new JsonResult(action());
            }
            catch (InvalidOperationException invalidOperationException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(new JsonReturn { Result = invalidOperationException.Message });
            }
            catch (Exception exception)
            {
                _log.Write(exception);
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new JsonReturn { Result = "Internal error" });
            }
        }
    }

    public class JsonReturn
    {
        public object Result { get; set; }
    }
}