using Application.Interfaces;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthenticationServiceApi : IAuthenticationServiceApi
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        public AuthenticationServiceApi(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _config = configuration;
        }

        public User? ValidateUser(CredentialRequest credentialRequest)
        {
            return _userRepository.Authenticate(credentialRequest.UsernameOrEmail, credentialRequest.Password);
        }
        public string AuthenticateCredentials(CredentialRequest credentialRequest)
        {
            User? userAuthenticated = ValidateUser(credentialRequest);
            if (userAuthenticated == null)
             {
                return string.Empty; ;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", userAuthenticated.Id.ToString()));
            claimsForToken.Add(new Claim("given_email", userAuthenticated.Email));
            claimsForToken.Add(new Claim("role", userAuthenticated.Status.ToString()));  

            var jwtSecurityToken = new JwtSecurityToken(
            _config["Authentication:Issuer"],
              _config["Authentication:Audience"],
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


        }
    }
}


