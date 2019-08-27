using System.Threading.Tasks;

namespace OauthHelper
{
    /// <summary>
    /// Responsible to create an authentication token, also to refresh token
    /// </summary>
    public interface IOAuthClient
    {
        /// <summary>
        /// Access and Refresh token container
        /// </summary>
        ITokenManager TokenManager { get; }

        /// <summary>
        /// Gets a new set of tokens for a given user
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">user password</param>
        /// <returns></returns>
        Task<Result<ITokenManager>> GetAccessToken(string username, string password);

        /// <summary>
        /// Revoke the Access Token
        /// </summary>
        Task Revoke();

        /// <summary>
        /// Refresh Access Token with current Refresh Token
        /// </summary>
        /// <returns>New Token Manager Instance</returns>
        Task<Result<ITokenManager>> RefreshAccessToken(string refreshToken);
    }
}