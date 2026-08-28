using Microsoft.AspNetCore.Mvc;
using NewProject.Common.Results;
using System.Net;

namespace NewProject.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult FromServiceResult<T>(
       this ControllerBase controller,
       ServiceResult<T> result)
        {
            return result.StatusCode switch
            {
                HttpStatusCode.OK =>
                    controller.Ok(result.Data),

                HttpStatusCode.Created =>
                    controller.Created(string.Empty, result.Data),

                HttpStatusCode.BadRequest =>
                    controller.BadRequest(new
                    {
                        result.Message,
                        result.Errors
                    }),

                HttpStatusCode.NotFound =>
                    controller.NotFound(new
                    {
                        result.Message
                    }),

                HttpStatusCode.Conflict =>
                    controller.Conflict(new
                    {
                        result.Message
                    }),

                HttpStatusCode.InternalServerError =>
                    controller.StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        result.Message
                    }),

                _ =>
                    controller.StatusCode((int)result.StatusCode,
                    new
                    {
                        result.Message
                    })
            };
        }
    }
}
