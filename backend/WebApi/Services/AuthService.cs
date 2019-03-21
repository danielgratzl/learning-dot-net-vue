using System.Net.NetworkInformation;
using WebApi.Models;
using WebApi.Repositiories;
using System;
using JWT.Builder;
using JWT.Algorithms;

namespace WebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthTokenRepository _repository;

        private readonly IEmailService _emailService;

        private readonly Random _random;

        public AuthService(IAuthTokenRepository repository, Random random, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
            _random = random;
        }

        /*
         * Creates a new PIN and sends it to the given email using 
         * the configured provider.
         */
        public string RequestPin(string email)
        {
            var authToken = GetOrCreateAuthToken(email);
            
            // use email service to send email with new pin
            _emailService.Send(email, authToken.Pin);

            return authToken.Id;
        }

        /*
         * Validates a given Pin against the stored in the database.
         * Returns the associated identity.
         */       
        public Identity ValidatePin(string token, string pin) {
            var authToken = _repository.FindAuthToken(token, pin);

            if (authToken == null) {
                throw new Exception("Pin invalid");
            }

            // udpate token with verified date
            authToken.DateVerified = DateTime.UtcNow;
            _repository.Update(authToken);


            return new Identity { Email = authToken.Email };
        }

        private AuthToken GetOrCreateAuthToken(string email) {
            var existingToken = _repository.FindUnused(email);

            if (existingToken != null) {
                return existingToken;
            }

            // generate new pin code
            var pin = _random.Next(0, 9999).ToString("D4");
            var authTokenId = Guid.NewGuid();
            var authToken = new AuthToken();
            authToken.Id = authTokenId.ToString();
            authToken.Email = email;
            authToken.Pin = pin;
            _repository.Create(authToken);
            return authToken;
        }
    }
}