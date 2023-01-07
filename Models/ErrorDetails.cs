using System.Text.Json;

namespace Attendr.API.Models
{
    public class ErrorDetails
    {

        public int StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorDetail { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public ErrorDetails(int statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            Error = errorMessage;
        }

        public ErrorDetails(int statusCode, string errorMessage, string errorDetail) : this(statusCode, errorMessage)
        {
            ErrorDetail = errorDetail;
        }


    }
}
