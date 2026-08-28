using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NewProject.Common.Results
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }

        public T? Data { get; private set; }

        public string? Message { get; private set; }

        public List<string>? Errors { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        private ServiceResult() { }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static ServiceResult<T> Created(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = HttpStatusCode.Created
            };
        }

        public static ServiceResult<T> BadRequest(string message)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static ServiceResult<T> NotFound(string message)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public static ServiceResult<T> Conflict(string message)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = HttpStatusCode.Conflict
            };
        }

        public static ServiceResult<T> InternalServerError(string message)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }
}
