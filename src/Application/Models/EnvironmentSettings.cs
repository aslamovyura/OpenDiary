namespace Application.Models
{
    /// <summary>
    /// Application environment settings.
    /// </summary>
    public class EnvironmentSettings
    {
        /// <summary>
        /// Environmetn settings. "true" - use docker container, "false" - use windows support.
        /// </summary>
        public bool IsDockerSupport { get; set; }
    }
}