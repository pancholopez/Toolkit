using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OauthHelper
{
    public abstract class OauthServiceBase
    {
        #region Oauth Constants
        //OAuth deserialization keys
        private const string AccessTokenKey = "access_token";
        private const string AccessTokenExpiresInKey = "expires_in";
        private const string RefreshTokenKey = "refresh_token";
        private const string RefreshTokenExpiresInKey = "refresh_expires_in";
        private const string ScopeKey = "scope";
        private const string SessionStateKey = "session_state";

        //Oauth header key values
        private const string CacheControlKey = "cache-control";
        private const string CacheControlValue = "no-store";
        private const string PragmaKey = "pragma";
        private const string PragmaValue = "no-cache";

        //Oauth Request Form Fields key values
        private const string GrantTypeKey = "grant_type";
        private const string GrantTypePassword = "password";
        private const string GrantTypeRefreshToken = "refresh_token";
        private const string ClientIdKey = "client_id";
        private const string UsernameKey = "username";
        private const string PasswordKey = "password";
        private const string RefreshTokenFormFieldKey = "refresh_token";
        #endregion

        private HttpClient HttpClient { get; }
        protected AuthConfig Config { get; }

        protected OauthServiceBase(HttpClient httpClient, AuthConfig config)
        {
            HttpClient = httpClient;
            Config = config;
        }

        //headers added as per the documentation: https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/
        protected static Dictionary<string, string> OAuthRequestHeaders => new Dictionary<string, string>
        {
            {CacheControlKey, CacheControlValue},
            {PragmaKey, PragmaValue}
        };

        protected static Dictionary<string, string> CreateLoginParameters(
            string clientId, string username, string password) =>
            new Dictionary<string, string>
            {
                {GrantTypeKey, GrantTypePassword},
                {ClientIdKey, clientId},
                {UsernameKey, username},
                {PasswordKey, password}
            };

        protected static Dictionary<string, string> CreateRefreshParameters(string clientId, string refreshToken)
            => new Dictionary<string, string>
            {
                {GrantTypeKey, GrantTypeRefreshToken},
                {ClientIdKey, clientId},
                {RefreshTokenFormFieldKey, refreshToken}
            };

        private string SerializePayload(object value)
            => JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

        protected async Task<Result<ITokenManager>> PostAsync(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            request.Content = new FormUrlEncodedContent(parameters);
            Token accessToken;
            Token refreshToken;
            string scope;
            string sessionState;
            try
            {
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                accessToken = new Token(dictionary[AccessTokenKey], Convert.ToInt32(dictionary[AccessTokenExpiresInKey]));
                refreshToken = new Token(dictionary[RefreshTokenKey], Convert.ToInt32(dictionary[RefreshTokenExpiresInKey]));
                scope = dictionary[ScopeKey];
                sessionState = dictionary[SessionStateKey];
            }
            catch (Exception exception)
            {
                //we don't want to expose passwords in our logs
                parameters.Remove(PasswordKey);

                return Result.Fail<ITokenManager>(
                    $"{exception.Message} StackTrace: {exception.StackTrace} Parameters: {SerializePayload(parameters)}");
            }

            return Result.Ok<ITokenManager>(new TokenManager(accessToken, refreshToken, scope, sessionState));
        }
    }
}