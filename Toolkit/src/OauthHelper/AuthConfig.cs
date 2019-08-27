namespace OauthHelper
{
    /// <summary>
    /// Authorization server settings
    /// </summary>
    public sealed class AuthConfig
    {
        /// <summary>
        /// Client Unique Identifier
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Authorization server Url
        /// </summary>
        public string AuthorizationUrl { get; set; }
    }
}