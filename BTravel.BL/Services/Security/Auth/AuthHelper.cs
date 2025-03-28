using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.BL.Services.Security.Auth
{
    public class AuthHelper
    {
        public static string GetClaimValue(ClaimsPrincipal user, string type)
        {
            if (user.Identity is ClaimsIdentity identity)
            {
                var claim = identity.FindFirst(c => c.Type == type);
                if (claim != null)
                    return claim.Value;
            }

            return null;
        }
    }
}