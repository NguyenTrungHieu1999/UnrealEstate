using System.IdentityModel.Tokens.Jwt;

namespace UnrealEstate.Utilities.Constants
{
    public static class DecodeToken
    {
        public static JwtSecurityToken Decode(string sessions)
        {
            var stream = sessions;

            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

            return tokenS;
        }
    }
}
