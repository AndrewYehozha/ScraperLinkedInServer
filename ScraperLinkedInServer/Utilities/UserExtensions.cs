using ScraperLinkedInServer.Models.Entities;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace ScraperLinkedInServer.Utilities
{
    public static class UserExtensions
    {
        public static int ToAccountID(this IIdentity identity)
        {
            var result = default(int);
            int.TryParse(GetClaimValue(identity, Claims.AccountId), out result);
            return result;
        }
        public static string ToFirstName(this IIdentity identity)
        {
            return GetClaimValue(identity, Claims.FirstName);
        }
        public static string ToLastName(this IIdentity identity)
        {
            return GetClaimValue(identity, Claims.LastName);
        }
        public static string ToEmail(this IIdentity identity)
        {
            return GetClaimValue(identity, Claims.Email);
        }
        public static short ToRoleID(this IIdentity identity)
        {
            var result = default(short);
            short.TryParse(GetClaimValue(identity, Claims.RoleId), out result);
            return result;
        }
        public static string ToRole(this IIdentity identity)
        {
            return GetClaimValue(identity, ClaimTypes.Role);
        }

        #region Get Raw Claim
        private static string GetClaimValue(IIdentity identity, string claimName)
        {
            var result = default(string);
            var claimsIdentity = identity as ClaimsIdentity;
            var claims = claimsIdentity != null ? claimsIdentity.Claims : null;
            if (claims != null && claims.Where(c => c.Type == claimName).Any()) result = claims.Where(c => c.Type == claimName).First().Value;
            return result;
        }
        #endregion
    }

}