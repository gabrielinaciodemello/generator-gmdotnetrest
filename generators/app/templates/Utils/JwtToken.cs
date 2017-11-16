using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace <%= ProjectName %>.Utils
{
    public class JwtToken: IBearerAuth
    {
        private readonly string _secret;
        private readonly int _expireMinutes;

        public JwtToken(string secret, int expireMinutes)
        {
            _secret = secret;
            _expireMinutes = expireMinutes;
        }
        
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(_secret));
        }

        public string GenerateToken(string id, string name)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, id),
                            new Claim(ClaimTypes.Name, name)
                        }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_expireMinutes)),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}