using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalesMart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Infrastructure.Utilities
{
    public class TokenMgtService: ITokenMgtService
    {
        private readonly IConfiguration _configuration;
        public TokenMgtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> generateJWtToken(User user)
        {
            var claim = new[] {
                        new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                        new Claim(ClaimTypes.Email, user.Email)
                       };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("TokensecretKey").Value)
   ;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claim),
                Expires = DateTime.Now.AddMinutes(45),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),            SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> DecryptTokenToUserdetails(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetSection("TokensecretKey").Value)
   ;
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
   ,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var UserEmail = (jwtToken.Claims.First(x => x.Type == "email").Value);
                return UserEmail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> RefreshToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetSection("TokensecretKey").Value)
   ;
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
   ,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var userEmail = (jwtToken.Claims.First(x => x.Type == "email").Value);
                return (jwtToken.Claims.First(x => x.Type == "email").Value);
                ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
