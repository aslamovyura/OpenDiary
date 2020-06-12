namespace WebUI.Contants
{
    /// <summary>
    /// Constants for application services initialization.
    /// </summary>
    public class InitializationConstants
    {
        /// <summary>
        /// Migration error message.
        /// </summary>
        public const string MigrationError = "An error occurred while DB migrating.";

        /// <summary>
        /// Migration success message.
        /// </summary>
        public const string MigrationSuccess = "The database is successfully migrated.";

        /// <summary>
        /// Database seed error.
        /// </summary>
        public const string SeedError = "An error occurred while DB seeding.";

        /// <summary>
        /// Database seed success.
        /// </summary>
        public const string SeedSuccess = "The database is successfully seeded.";

        /// <summary>
        /// Web host is staring.
        /// </summary>
        public const string WebHostStarting = "Web host is starting.";

        /// <summary>
        /// Web host was terminated unexpectedly.
        /// </summary>
        public const string WebHostTerminated = "Web host was terminated unexpectedly!";
    }
}