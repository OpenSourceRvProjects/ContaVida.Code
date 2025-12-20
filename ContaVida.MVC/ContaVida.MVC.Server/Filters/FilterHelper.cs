using System.IdentityModel.Tokens.Jwt;

namespace ContaVida.MVC.Server.Filters
{
    public class FilterHelper
    {

        public static JwtSecurityToken GetTokenDataByStringValue(string rawToken)
        {
            var token = rawToken;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken;
        }
    }
}
