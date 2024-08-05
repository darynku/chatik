using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace DarynChat.API.JWT;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string Generate(UserEntity user)
    {
        var jwtHandler = new JsonWebTokenHandler();

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));


        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(claims),
            SigningCredentials = new(symmetricKey, SecurityAlgorithms.HmacSha256),
            Expires = DateTime.UtcNow.AddHours(_jwtOptions.Expires)
        };

        var token = jwtHandler.CreateToken(tokenDescriptor);

        if (token is null)
            throw new Exception("Не удалось создать пользователя");

        return token;
    }
}
