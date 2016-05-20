using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeExercise.WebApi.Models
{
    public class ApiResponse<T>
    {
        public ApiStatus status { get; set; }
        public T Data { get; set; }
        public ApiResponse()
        {
            status = new ApiStatus() { IsSuccess = true };
        }
    }

    public class ApiStatus
    {
        public bool IsSuccess { get; set; }
        public string Code { get; set; }
        public string Reason { get; set; }
    }
}