using SelfHostedWebApi.BuisnessLayer;
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
        private const string WWWAuthenticateHeader = "WWW-Authenticate";

        public AuthenticationMessageHandler(HttpConfiguration httpconfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpconfiguration);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var actionName = GetRequestActionName(request);
            if (actionName == ServerStaticValues.MethodName_UserConnect.ToLower())
            {
                return await base.SendAsync(request, cancellationToken);
            }
            else
            {
                var credentials = ParseAuthorizationHeader(request);

                if (credentials != null)
                {
                    var identity = new BasicAuthenticationIdentity(credentials.Name, credentials.Password);
                    var principal = new GenericPrincipal(identity, null);
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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "argument is null in ParseAuthorizationHeader.");
            }

            string authHeader = null;
            var auth = request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            bool result = ValidateAuthenticationData(tokens);

            return !result ? null : new BasicAuthenticationIdentity(tokens[0], tokens[1]);
        }

        private bool ValidateAuthenticationData(string[] tokens)
        {
            if (tokens == null || tokens.Length < 2)
            {
                throw new ArgumentException(nameof(tokens), "One argument is wrong in ValidateAuthenticationData");
            }

            var userExist = AppHandler.Instance.users.Where(u => u.Name == tokens[0] && u.Password == tokens[1]).FirstOrDefault();

            return userExist != null ? true : false;
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