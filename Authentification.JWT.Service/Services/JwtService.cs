using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentification.JWT.Service.Interfaces;

public class JwtService : IJwtService
{
    private readonly string _key;
    public JwtService(IConfiguration config)
    {
        _key = config["Jwt:Key"];
    }
    public string GenerateToken(int userId)
    {
        //preparation du cle de signature
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // config du token
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        // cela permet de generer token sous format string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}