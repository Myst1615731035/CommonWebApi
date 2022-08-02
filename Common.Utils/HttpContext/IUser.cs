using System.Collections.Generic;
using System.Security.Claims;

namespace Common.Utils
{
    public interface IUser
    {
        string Name { get; }
        string ID { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);
        string GetToken();
        List<string> GetUserInfoFromToken(string ClaimType);
    }
}
