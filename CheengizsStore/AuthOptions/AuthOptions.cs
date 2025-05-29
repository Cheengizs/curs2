using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CheengizsStore.AuthOptions;

public record AuthOptions(string Login, string Password);

public class AuthJwtOptions
{
    private const string _issuer = "http://localhost:5212";
    private const string _audience = "http://localhost:5212";
    private const string _secretKey = "Bi17Ab!Inaogs8&&kbdk17dyukjdn79000";
    
    public string Audience => _audience;
    public string Issuer => _issuer;
    
    public SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
}