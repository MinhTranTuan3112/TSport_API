using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Models.ResponseModels
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.InternalServerError;

        [EnumDataType(typeof(ErrorType))]
        public required string ErrorType { get; set; }

        public required string ErrorMessage { get; set; }

        public string? StackTrace { get; set; }
    }
}