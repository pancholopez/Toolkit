using System;

namespace OauthHelper
{
    /// <summary>
    /// Hold reference to Access and Refresh Tokens and handle their expiration
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// </summary>
        Token AccessToken { get; }

        /// <summary>
        /// </summary>
        Token RefreshToken { get; }

        /// <summary>
        /// </summary>
        string Scope { get; }

        /// <summary>
        /// </summary>
        string SessionState { get; }

        /// <summary>
        /// </summary>
        DateTime TimeStamp { get; }
    }
}