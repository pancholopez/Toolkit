using System;

namespace OauthHelper
{
    /// <inheritdoc cref="ITokenManager" />
    public class TokenManager : ITokenManager
    {
        public Token AccessToken { get; private set; }
        public Token RefreshToken { get; private set; }

        public string Scope { get; }
        public string SessionState { get; }

        public DateTime TimeStamp { get; }

        private TokenManager()
        {
            TimeStamp = DateTime.Now;
        }

        public TokenManager(Token accessToken, Token refreshToken, string scope, string sessionState)
            : this()
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Scope = scope;
            SessionState = sessionState;
        }
    }
}