using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NtuPH2023.Data;
using System.Security.Claims;

namespace NtuPH2023.Common
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        private readonly IMemoryCache _cache;
        private readonly NtuPH2023Context _context;

        public ClaimsTransformation(IMemoryCache cache, NtuPH2023Context context)
        {
            _cache = cache;
            _context = context;
        }

        // Each time HttpContext.AuthenticateAsync() or HttpContext.SignInAsync(...) are called, the claims transformer is invoked. 
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(c => c.IsAuthenticated);
            if (string.IsNullOrEmpty(identity?.Name))
            {
                return principal;
            }

            // Read claims from memory if exists
            if (_cache.TryGetValue(identity.Name, out List<Claim> claims) && principal.Identity != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(claims);
            }
            else
            {
                claims = new List<Claim>();

                var tblSetting = await _context.TblSettings.AsNoTracking().OrderByDescending(m => m.CreatedTimestamp).FirstOrDefaultAsync();

                if (tblSetting != null && tblSetting.AdminArr.Contains(identity.Name, StringComparer.OrdinalIgnoreCase))
                {
                    claims.Add(new Claim(identity.RoleClaimType, "Administrator"));
                }

                // timeount : 30  minutes
                _cache.Set(identity.Name, claims, TimeSpan.FromMinutes(30));
                identity.AddClaims(claims);
            }

            return new ClaimsPrincipal(identity);
        }
    }
}