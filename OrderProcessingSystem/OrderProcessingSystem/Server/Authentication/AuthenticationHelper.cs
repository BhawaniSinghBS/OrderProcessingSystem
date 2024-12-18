using Microsoft.IdentityModel.Tokens;
using OrderProcessingSystem.Shared.AppSettingKeys;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderProcessingSystem.Server.Authentication
{
    public static class AuthenticationHelper
    {
        public static bool AddUserToHttpContext(ref HttpContext httpcontext, ClaimsIdentity claimsIdentity)
        {
            try
            {
                var principal = new ClaimsPrincipal(claimsIdentity);
                httpcontext.User = principal;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Dictionary<claimType,stringValue> claimTypeAndValues
        /// </summary>
        /// <param name="claimTypeAndValues"></param>
        /// <returns>ClaimsIdentity</returns>
        public static ClaimsIdentity GetUserIdentyWithClaims(Dictionary<string, string> claimNameAndValues)
        {
            List<Claim> claims = new();
            foreach (var claimTypeAndValue in claimNameAndValues)
            {
                claims.Add(new Claim(claimTypeAndValue.Key, claimTypeAndValue.Value));
            }
            var claimsIdentity = new ClaimsIdentity(claims);
            return claimsIdentity;
        }
        public static ClaimsIdentity AddNewClaimToUserIdentiy(this ClaimsIdentity claimsIdentity, Dictionary<string, string> keyValuePairs)
        {
            foreach (var kvp in keyValuePairs)
            {
                // Add each key-value pair as a claim
                Claim claim = new Claim(kvp.Key, kvp.Value);
                claimsIdentity.AddClaim(claim);
            }
            return claimsIdentity;
        }
        public static ClaimsIdentity UpdateClaimToUserIdentiy(ClaimsIdentity claimsIdentity, Dictionary<string, string> updatedClaimDictionary)
        {
            foreach (var kvp in updatedClaimDictionary)
            {
                // Create a claim for the key-value pair
                Claim claim = new Claim(kvp.Key, kvp.Value);

                // Find if the claim already exists
                var existingClaim = claimsIdentity.FindFirst(claim.Type);

                // If the claim exists, remove it
                if (existingClaim != null)
                {
                    claimsIdentity.RemoveClaim(existingClaim);
                    // Add the updated claim
                    claimsIdentity.AddClaim(claim);
                }
            }
            return claimsIdentity;
        }
        public static List<Claim> GenerateClaims(string userId, string username, IEnumerable<string> roles = null, IDictionary<string, bool>? permissions = null, Dictionary<string, string> otherClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId),
                new Claim(System.Security.Claims.ClaimTypes.Name, username),
            };
            if (roles?.Count() > 0)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(System.Security.Claims.ClaimTypes.Role, role));
                }
            }
            if (permissions?.Count() > 0)
            {
                // Add permission claims as key-value pairs
                foreach (var permission in permissions)
                {
                    claims.Add(new Claim(AllClaimTypes.UserPermissions, $"{permission.Key}:{permission.Value}"));
                }
            }
            if (otherClaims?.Count() > 0)
            {
                // Add permission claims as key-value pairs
                foreach (var otherClaim in otherClaims)
                {
                    string claimKey = otherClaim.Key?.ToString() ?? "";
                    string claimValue = otherClaim.Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(claimKey) && !string.IsNullOrEmpty(claimValue))
                    {
                        claims.Add(new Claim(claimKey, claimValue));
                    }
                }
            }
            return claims;
        }
        public static string GenerateJwtToken(List<Claim> claims, IConfiguration configuration)
        {
            string jwtIssuerKey = configuration[AppsettingsKeys.JWTSectionName + ":" + AppsettingsKeys.JWTIssuerKey];
            try
            {
                string jwtSecurityKey = configuration[AppsettingsKeys.JWTSectionName + ":" + AppsettingsKeys.JWTSecretKeyKey];
                string jwtAudienceKey = configuration[AppsettingsKeys.JWTSectionName + ":" + AppsettingsKeys.JWTAudienceKey];
                string authTokenExpiryInSeconds = configuration[AppsettingsKeys.AuthTokenExpiryInSeconds];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    jwtIssuerKey,
                    jwtAudienceKey,
                    claims,
                    expires: DateTime.UtcNow.AddSeconds(int.Parse(authTokenExpiryInSeconds)).AddMinutes(-5),// token expires aftrer 5 minutes of given
                    signingCredentials: credentials
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool IsTokenValid(string token, IConfiguration configuration)
        {

            try
            {
                return GetClaimsPrincipalFromValidJwt(token, configuration, out ClaimsPrincipal claimsPrincipal);
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }
        public static bool GetClaimsPrincipalFromValidJwt(string token, IConfiguration configuration, out ClaimsPrincipal claimsPrincipal)
        {
            //Parse and validate the JWT token
            string jwtIssuerKey = configuration[AppsettingsKeys.JWTSectionName + ":" + AppsettingsKeys.JWTIssuerKey];
            string jwtSecurityKey = configuration[AppsettingsKeys.JWTSectionName + ":" + AppsettingsKeys.JWTSecretKeyKey];
            string jwtAudienceKey = configuration[AppsettingsKeys.JWTSectionName + ":" + AppsettingsKeys.JWTAudienceKey];
            bool isValidToken = false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecurityKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidIssuer = jwtIssuerKey,
                ValidAudience = jwtAudienceKey,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            claimsPrincipal = null;

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                isValidToken = true;
            }
            catch (Exception ex)
            {
                //Handle token validation errors
                // Example: Log the error or throw an exception
                isValidToken = false;
                Console.WriteLine(ex.Message);
            }
            return isValidToken;
        }
    }
}
