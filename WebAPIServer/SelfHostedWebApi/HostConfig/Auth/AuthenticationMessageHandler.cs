using NLog;
using SelfHostedWebApi.BuisnessLayer;
using SelfHostedWebApi.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace SelfHostedWebApi.HostConfig
{
    internal class AuthenticationMessageHandler : DelegatingHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private const string WWWAuthenticateHeader = "WWW-Authenticate";

        public AuthenticationMessageHandler(HttpConfiguration httpconfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpconfiguration);
        }

        public AuthenticationMessageHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.Debug($"Starting authentication");

            var actionName = GetRequestActionName(request);
            if (false && actionName == ServerStaticValues.MethodName_UserConnect.ToLower())
            {
                return await base.SendAsync(request, cancellationToken);
            }
            else
            {
                var credentials = ParseAuthorizationHeader(request);

                if (credentials != null)
                {
                    var principal = new GenericPrincipal(credentials, null);
                    request.GetRequestContext().Principal = principal;
                }

                return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    if (credentials == null && response.StatusCode == HttpStatusCode.Unauthorized)
                        Challenge(request, response);
                    return response;
                });
            }
        }

        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpRequestMessage request)
        {
            logger.Debug($"Parsing authorization header");

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "argument is null in ParseAuthorizationHeader.");
            }

            logger.Debug($"Parsing authorization header step 1");

            string authHeader = null;
            var auth = request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            logger.Debug($"Parsing authorization header step 2");

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            logger.Debug($"Parsing authorization header step 3");

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            var result = ValidateAuthenticationData(tokens);
            logger.Debug($"Parsing result {result.ToString()}");

            return result == ServerStaticValues.AppRole.nothing ? null : new BasicAuthenticationIdentity(tokens[0], tokens[1], result);
        }

        private ServerStaticValues.AppRole ValidateAuthenticationData(string[] tokens)
        {
            logger.Debug($"Start validating auth data");

            if (tokens == null || tokens.Length < 2)
            {
                throw new ArgumentException(nameof(tokens), "One argument is wrong in ValidateAuthenticationData");
            }

            logger.Debug($"finding user in database");

            var userExist = AppHandler.Instance.UserExist(tokens[0], tokens[1]);

            logger.Debug($"user exist: {userExist != null}");

            return userExist != null ? AuthorizationHelper.GetRoleFromString(userExist.Role) : ServerStaticValues.AppRole.nothing;
        }

        private void Challenge(HttpRequestMessage request, HttpResponseMessage response)
        {
            var host = request.RequestUri.DnsSafeHost;
            response.Headers.Add(WWWAuthenticateHeader, string.Format("Basic realm=\"{0}\"", host));
        }

        private string GetRequestActionName(HttpRequestMessage request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RequestUri.LocalPath))
            {
                throw new ArgumentException(nameof(request), "GetRequestActionName, request malformed");
            }

            var localUri = request.RequestUri.LocalPath;
            var splittedUri = localUri.Split('/');

            if (splittedUri.Length < 3)
            {
                throw new ArgumentException(nameof(request.RequestUri.LocalPath), "GetRequestActionName, LocalPath malformed");
            }

            return splittedUri.ElementAt(3).ToLower();
        }
    }
}