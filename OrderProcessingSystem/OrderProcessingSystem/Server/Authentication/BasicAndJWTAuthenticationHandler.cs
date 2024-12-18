using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using OrderProcessingSystem.Shared.Http;
using OrderProcessingSystem.Shared.AppSettingKeys;
using static OrderProcessingSystem.Server.Authentication.AuthEnums;
using OrderProcessingSystemApplication.UserService;
using OrderProcessingSystem.Shared.Models.DTOs;

namespace OrderProcessingSystem.Server.Authentication
{
    public class BasicAndJWTAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public BasicAndJWTAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserService userService, IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
            _configuration = configuration;
        }



        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Path.ToString().ToLower().Contains(EndPoints.Authenticate.ToLower()))
            {

                if (Request.Headers.ContainsKey(HttpHeadersKeys.TokenKey.ToLower()))
                {
                    // contains token header ,validate token if invalid token check for basic auth if both failed return unAuthorised
                    string jwtToken = Request.Headers[HttpHeadersKeys.TokenKey.ToLower()].ToString().Replace(AuthEnums.AuthenticationSchemes.Bearer.ToString(), string.Empty).Replace(AuthEnums.AuthenticationSchemes.Basic.ToString(), string.Empty).Trim() ?? string.Empty;
                    if (AuthenticateJWTTokenAndCreateUser(jwtToken, out AuthenticationTicket authenticationTicket,isAddTokenToResponse:false))
                    {
                        return AuthenticateResult.Success(authenticationTicket);
                    }
                }
            }
            else
            {
                if (!Request.Headers.ContainsKey(HttpHeadersKeys.AuthorizationKey))
                {
                    return AuthenticateResult.Fail(_configuration[AppsettingsKeys.BadRequestTextMessageKey]);
                }

                var authHeader = Request.Headers[HttpHeadersKeys.AuthorizationKey].ToString();

                if (authHeader?.StartsWith(AuthEnums.AuthenticationSchemes.Bearer.ToString()) ?? false)
                {
                    var jwtToken = authHeader.Substring(AuthEnums.AuthenticationSchemes.Bearer.ToString().Length).Trim();
                    if (AuthenticateJWTTokenAndCreateUser(jwtToken, out AuthenticationTicket authenticationTicket, isAddTokenToResponse: false))
                    {
                        return AuthenticateResult.Success(authenticationTicket);
                    }
                }

                if ((authHeader?.StartsWith(AuthEnums.AuthenticationSchemes.Basic.ToString()) ?? false))
                {
                    var token = authHeader.Substring(AuthEnums.AuthenticationSchemes.Basic.ToString().Length).Trim();
                    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':');
                    var email = credentials[0];
                    var password = credentials[1];

                    // Authenticate user with the database
                    UserDTO user = await _userService.AuthenticateUserAsync(email, password);

                    if (user == null || !user.IsAuthorised)
                    {
                        return AuthenticateResult.Fail(_configuration[AppsettingsKeys.BadRequestTextMessageKey]);
                    }

                    // Return the JWT token
                    List<Claim> claims = AuthenticationHelper.GenerateClaims(
                        userId: user.Id.ToString(),
                        username: user.UserName,
                        roles: new List<string>() { AuthEnums.UserRole.User.ToString() },
                        permissions: new Dictionary<string, bool>() { { UserPermissions.IsAllowedToAccesServie.ToString(), true } },
                        otherClaims: new Dictionary<string, string>() { { AllClaimTypes.Email, email } }
                        );

                    // Create JWT token and add to claims to add to identity so that can be aceesed in cntoller User
                    var jwtToken = AuthenticationHelper.GenerateJwtToken(claims, _configuration);
                    if (AuthenticateJWTTokenAndCreateUser(jwtToken, out AuthenticationTicket authenticationTicket, isAddTokenToResponse: true))
                    {
                        return AuthenticateResult.Success(authenticationTicket);
                    }
                }
            }
            return AuthenticateResult.Fail(_configuration[AppsettingsKeys.BadRequestTextMessageKey]);
        }
        private bool AuthenticateJWTTokenAndCreateUser(string jwtToken, out AuthenticationTicket authenticationTicket, bool isAddTokenToResponse)
        {
            authenticationTicket = null;
            if (!string.IsNullOrEmpty(jwtToken) && AuthenticationHelper.GetClaimsPrincipalFromValidJwt(jwtToken, _configuration, out ClaimsPrincipal claimsPrincipal))
            {
                ClaimsIdentity identity = (ClaimsIdentity)claimsPrincipal.Identity;
                identity.AddClaim(new Claim(AllClaimTypes.TokenKey, jwtToken));

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                authenticationTicket = new AuthenticationTicket(principal, AuthEnums.AuthenticationSchemes.Basic.ToString());
                if (isAddTokenToResponse)
                {
                    Response.Headers.Add(HttpHeadersKeys.TokenKey, jwtToken);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
