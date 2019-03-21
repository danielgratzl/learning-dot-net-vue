namespace WebApi.Models {
    public class JwtResponse {
        private string _token;

        public JwtResponse(string token)
        {
            _token = token;
        }
        
        public string Token => _token;

    }
}