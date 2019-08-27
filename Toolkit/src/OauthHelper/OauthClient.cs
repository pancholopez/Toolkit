using System.Net.Http;
using System.Threading.Tasks;

namespace OauthHelper
{
    /// <summary>
    /// </summary>
    public class OAuthClient : OauthServiceBase, IOAuthClient
    {
        /// <inheritdoc />
        public ITokenManager TokenManager { get; private set; } = null;

        /// <summary>
        /// </summary>
        public OAuthClient(HttpClient httpClient, AuthConfig config) : base(httpClient, config)
        {
        }

        /// <inheritdoc />
        public async Task<Result<ITokenManager>> GetAccessToken(string username, string password)
        {
            var parameters = CreateLoginParameters(Config.ClientId, username, password);
            return await PostAsync(Config.AuthorizationUrl, parameters, OAuthRequestHeaders);
        }

        /// <inheritdoc />
        public async Task<Result<ITokenManager>> RefreshAccessToken(string refreshToken)
        {
            var parameters = CreateRefreshParameters(Config.ClientId, refreshToken);
            return await PostAsync(Config.AuthorizationUrl, parameters, OAuthRequestHeaders);
        }

        /// <inheritdoc />
        public Task Revoke()
        {
            //todo: call the API endpoint to revoke the access token in the server
            TokenManager = null;

            return Task.CompletedTask;
        }
    }
}