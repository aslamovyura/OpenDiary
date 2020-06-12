namespace Infrastructure.Extentions
{
    /// <summary>
    /// Extension for database connection.
    /// </summary>
    public static class DatabaseConnection
    {
        /// <summary>
        /// Get connection string type (Default/Docker) based on appsetting.json.
        /// </summary>
        /// <param name="isDockerSupport">Docker support enable.</param>
        /// <returns>Connection type.</returns>
        public static string ToDbConnectionString(this bool isDockerSupport)
        {
            string result;

            switch (isDockerSupport)
            {
                case true: { result = "DockerConnection"; } break;

                default: { result = "DefaultConnection"; } break;
            }

            return result;
        }
    }
}