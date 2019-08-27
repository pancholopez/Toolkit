using System;
using System.Threading.Tasks;

namespace OauthHelper
{
    public enum RefreshLoopStatus
    {
        Started,
        Running,
        Stopped
    }

    public interface IAuthenticationEvents
    {
        /// <summary>
        /// </summary>
        event Action<string> GetAccessTokenSucceed;

        /// <summary>
        /// </summary>
        event Action<string> GetAccessTokenFailed;

        /// <summary>
        /// </summary>
        event Action<RefreshLoopStatus, int> TokenRefreshLoopStatusChanged;
    }

    //TODO: implement IDisposable
    /// <summary>
    /// </summary>
    public interface IAuthenticationService : IAuthenticationEvents
    {
        /// <summary>
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Once logged in, it will automatically refresh the access token, see also RefreshAsync().
        /// </summary>
        Task<bool> LoginAsync(string userName, string password);

        /// <summary>
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Should be called by application before AccessToken is handed over to COS for finalizing the EPU run
        /// When EPU (process) is stopped, COS can still be offloading files on behalf of EPU;
        /// it needs a valid access token to be able to register the last remaining files.
        /// When refresh succeeds AccessTokenRefreshed event is raised.
        /// When refresh fails AccessTokenRefreshFailed event is raised.
        /// </summary>
        Task<bool> RefreshAsync();
    }
}