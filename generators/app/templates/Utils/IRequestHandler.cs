using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace <%= ProjectName %>.Utils
{
    public interface IRequestsHandler
    {
        JsonResult JsonHandler(HttpRequest request, HttpResponse response, Func<JsonReturn> action);
    }
}