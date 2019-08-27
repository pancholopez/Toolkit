namespace OauthHelper
{
    /// <summary>
    /// Represents an OAuth token
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The OAuth token string
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Expiration in seconds
        /// </summary>
        public int ExpireInSeconds { get; }

        /// <summary>
        /// </summary>
        public Token(string value, int expireInSeconds)
        {
            Value = value;
            ExpireInSeconds = expireInSeconds;
        }

        /// <summary>
        /// Creates an empty token
        /// </summary>
        public static Token Empty { get; } = new Token(string.Empty, 0);
    }
}