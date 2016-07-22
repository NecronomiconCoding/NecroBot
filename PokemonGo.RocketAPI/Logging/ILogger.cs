namespace PokemonGo.RocketAPI.Logging
{
	/// <summary>
	/// All loggers must implement this interface.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Log a specific message by LogLevel.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="level">Optional. Default <see cref="LogLevel.Info"/>.</param>
		void Write(string message, LogLevel level = LogLevel.Info);
	}
}