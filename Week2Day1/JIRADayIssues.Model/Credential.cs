using System;

namespace JIRADayIssues.Model
{
    /// <summary>
    /// Credentials for authentication.
    /// </summary>
    public class Credential
    {
        /// <summary>
        /// Gets or sets username for authentication.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets access token.
        /// </summary>
        public string AcessToken { get; set; }
    }
}
