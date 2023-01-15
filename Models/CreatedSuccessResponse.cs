namespace Attendr.API.Models
{
    public class CreatedSuccessResponse
    {
        public int StatusCode { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }

        public CreatedSuccessResponse(string nameOfCreatedObject)
        {
            StatusCode = StatusCodes.Status201Created;
            Result = $"{nameOfCreatedObject} created succesfully";
            Message = "Check Loaction header to navigate";
        }
    }
}
