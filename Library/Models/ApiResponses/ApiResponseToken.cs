using Library.Models.Authenticate;

namespace Library.Models.ApiResponses
{
    public class ApiResponseToken : ApiResponse
    {
        public TokenResponse Token { get; set; }
    }
}
