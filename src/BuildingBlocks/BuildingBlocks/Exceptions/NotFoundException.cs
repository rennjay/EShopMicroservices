namespace BuildingBlocks.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified entity name and key.
        /// </summary>
        /// <param name="entity">The name of the entity that was not found.</param>
        /// <param name="key">The key of the entity that was not found.</param>
        public NotFoundException(string entity, object key)
            : base($"Entity \"{entity}\" (key: {key}) was not found.")
        {
        }
    }
}
