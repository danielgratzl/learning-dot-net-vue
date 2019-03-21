using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;
using WebApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; 

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;

        public AuthController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }

        // POST api/auth
        [HttpPost]
        [AllowAnonymous]
        public LoginResponse Post(LoginRequest loginRequest)
        {
            var token = _authService.RequestPin(loginRequest.Email);

            return new LoginResponse { Email = loginRequest.Email, Token = token };
        }

         // POST api/auth/validate
        [HttpPost("validate")]
        [AllowAnonymous]
        public JwtResponse Validate(ValidationRequest validationRequest)
        {
            var identity = _authService.ValidatePin(validationRequest.Token, validationRequest.Pin);
            var jwt = BuildToken(identity);

            return new JwtResponse { Token = jwt };
        }

        private string BuildToken(Identity identity)
        {
            var secret = _config["Jwt:Secret"];
            var issuer = _config["Jwt:Issuer"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> {
                new Claim("sub", identity.Email),
           
            };
            var token = new JwtSecurityToken(issuer,
              issuer,
              claims,
              expires: DateTime.Now.AddHours(24),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public sealed class LoginRequest
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public sealed class LoginResponse
        {
            public string Email { get; set; }
            public string Token { get; set; }
        }

        public sealed class ValidationRequest
        {
            [Required]
            public string Token { get; set; }

            [Required]
            public string Pin { get; set; }
        }

        public sealed class JwtResponse
        {
            public string Token { get; set;  }
        }
    }
}
 