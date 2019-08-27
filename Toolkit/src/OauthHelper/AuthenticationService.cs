using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace OauthHelper
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Helpers

        private class AuthParameters
        {
            public string AccessToken { get; }
            public int AccessTokenExpiration { get; }
            public string RefreshToken { get; }

            private AuthParameters(string accessToken, int accessTokenExpiration, string refreshToken)
            {
                AccessToken = accessToken;
                AccessTokenExpiration = accessTokenExpiration;
                RefreshToken = refreshToken;
            }

            public static AuthParameters Create(string accessToken, int accessTokenExpiration, string refreshToken) =>
                new AuthParameters(accessToken, accessTokenExpiration, refreshToken);

            public static AuthParameters Empty => new AuthParameters(string.Empty, 0, string.Empty);
        }

        #endregion

        private Task _loopReference; //keep reference of the fire and forget Task to not be disposed out of scope

        public event Action<string> GetAccessTokenSucceed;
        public event Action<string> GetAccessTokenFailed;
        public event Action<RefreshLoopStatus, int> TokenRefreshLoopStatusChanged;

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken);
        public string AccessToken => _authParameters.AccessToken;

        private AuthParameters _authParameters = AuthParameters.Empty;

        private CancellationTokenSource _refreshCancellationTokenSource;
        private int LoopTimeout => (int)(_authParameters.AccessTokenExpiration * 1000 * 0.75);

        private static HttpClient SslHttpClient => new HttpClient(new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12,
            ServerCertificateCustomValidationCallback = (sender, certificate, chain, errors) => true
        });

        private IOAuthClient OAuthClient { get; }

        public AuthenticationService(AuthConfig config) : this(new OAuthClient(SslHttpClient, config))
        {
        }

        internal AuthenticationService(IOAuthClient authClient)
        {
            OAuthClient = authClient;
        }

        public async Task<bool> LoginAsync(string userName, string password) =>
            (await OAuthClient.GetAccessToken(userName, password))
            .OnFailure(result =>
            {
                GetAccessTokenFailed?.Invoke(result.Error);
                _authParameters = AuthParameters.Empty;
            })
            .OnSuccess(result => GetAccessTokenSucceed?.Invoke(result.Value.AccessToken.Value))
            .OnSuccess(result => _authParameters = AuthParameters.Create(
                accessToken: result.Value.AccessToken.Value,
                accessTokenExpiration: result.Value.AccessToken.ExpireInSeconds,
                refreshToken: result.Value.RefreshToken.Value))
            .OnSuccess(() =>
            {
                _refreshCancellationTokenSource = new CancellationTokenSource();
                BeginTokenRefreshLoop(_refreshCancellationTokenSource.Token);
            }).IsSuccess;

        public Task LogoutAsync()
        {
            _refreshCancellationTokenSource.Cancel();
            _authParameters = AuthParameters.Empty;
            return Task.CompletedTask;
        }

        public async Task<bool> RefreshAsync()
        {
            _refreshCancellationTokenSource.Cancel(); //stop loop

            return (await RefreshAccessToken(_authParameters.RefreshToken))
                .OnSuccess(() =>
                {
                    _refreshCancellationTokenSource = new CancellationTokenSource();
                    BeginTokenRefreshLoop(_refreshCancellationTokenSource.Token);
                })
                .IsSuccess;
        }

        private void BeginTokenRefreshLoop(CancellationToken ct)
        {
            _loopReference = Task.Run(async () =>
            {
                TokenRefreshLoopStatusChanged?.Invoke(RefreshLoopStatus.Started, LoopTimeout);
                try
                {
                    while (true)
                    {
                        await Task.Delay(LoopTimeout, ct);
                        (await RefreshAccessToken(_authParameters.RefreshToken))
                            .OnSuccess(() => TokenRefreshLoopStatusChanged?.Invoke(RefreshLoopStatus.Running, LoopTimeout))
                            .OnSuccess(result =>
                            {
                                _authParameters = result.Value;
                                GetAccessTokenSucceed?.Invoke($"Access Token Refreshed: {result.Value.AccessToken}");
                            })
                            .OnFailure(result =>
                            {
                                GetAccessTokenFailed?.Invoke(result.Error);
                                _refreshCancellationTokenSource.Cancel();
                            });
                    }
                }
                catch (OperationCanceledException)
                {
                    //no-op: refresh loop manually stopped, all ok.
                }
                catch (Exception exception)
                {
                    GetAccessTokenFailed?.Invoke(exception.Message);
                    _refreshCancellationTokenSource.Cancel();
                }
                finally
                {
                    TokenRefreshLoopStatusChanged?.Invoke(RefreshLoopStatus.Stopped, LoopTimeout);
                }
            }, ct);
        }

        private async Task<Result<AuthParameters>> RefreshAccessToken(string refreshToken) =>
            (await OAuthClient.RefreshAccessToken(refreshToken))
            .Map(result => AuthParameters.Create(
                accessToken: result.AccessToken.Value,
                accessTokenExpiration: result.AccessToken.ExpireInSeconds,
                refreshToken: result.RefreshToken.Value));
    }
}