using System;
using System.Net;

namespace ConventionalAndWhiteTariffCalculatorAPI.Infraestructure
{
    public class HttpException : Exception
    {
        public int StatusCode { get; set; }
        public dynamic Body { get; set; }

        public HttpException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpException(int statusCode, dynamic body) : this(statusCode)
        {
            this.Body = body;
        }
    }

    public class NotFoundException : HttpException
    {
        public NotFoundException() : base((int)HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(dynamic body) : base((int)HttpStatusCode.NotFound)
        {
            this.Body = body;
        }
    }

    public class BadRequestException : HttpException
    {
        public BadRequestException() : base((int)HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(dynamic body) : base((int)HttpStatusCode.BadRequest)
        {
            this.Body = body;
        }
    }
}
