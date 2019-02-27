using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace WebAPI.Authentification
{
    public class IdentityBasicAuthenticationAttribute : IAuthenticationFilter
    {
        public bool AllowMultiple { get { return false; } }

        /// <summary>
        /// metoda pro autetifikaci pozadavku zaslaneho z webu nebo scanneru (implementace z interfacu)
        /// </summary>
        /// <param name="context">zaslany kompletni pozadavek</param>
        /// <param name="cancellationToken">pri vyvolani zastavi beh metody</param>
        /// <returns></returns>
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null || (authorization.Scheme != "Basic" && authorization.Scheme != "Bearer") || string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            string method = request.Method.Method.ToUpper(), path = request.RequestUri.LocalPath.ToLower();
            if (authorization.Scheme == "Basic" && ((method == "POST" && (path == "/api/user" || path == "/api/error")) || (method == "GET" && path == "/api/test")))
            {
                string[] credentials = ExtractUserNameAndPassword(authorization.Parameter);
                if (credentials == null || credentials.Length != 2)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                    return;
                }

                PrincipalProvider provider = new PrincipalProvider();
                IPrincipal principal = await provider.CreatePrincipal(credentials[0], credentials[1]);
                if (principal == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
                }
                else
                {
                    context.Principal = principal;
                }
            }
            else if (authorization.Scheme == "Bearer")
            {
                JwtPrincipalProvider jwt = new JwtPrincipalProvider();
                IPrincipal principal = await jwt.CreatePrincipal(authorization.Parameter);
                if (principal == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
                }
                else
                {
                    context.Principal = principal;
                }
            }
            else
            {
                context.ErrorResult = new AuthenticationFailureResult("Bad credentials type", request);
            }
        }

        private string[] ExtractUserNameAndPassword(string parameter)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(parameter)).Split(':');
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);

            return Task.FromResult(0);
        }
    }
}