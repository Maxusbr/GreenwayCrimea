namespace AdvantShop.Core.Services.Api
{
    public class BaseApiResponse
    {
        public BaseApiResponse()
        {
            errors = "";
            warnings = "";
        }

        public string errors { get; set; }

        public string warnings { get; set; }
    }

    public class ApiResponse : BaseApiResponse
    {
        public ApiResponse(){}
        public ApiResponse(string status, string errors)
        {
            this.status = status;
            this.errors = errors;
        }
        public string status { get; set; }
    }
}