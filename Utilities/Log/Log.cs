namespace apief
{
    public class Log : ILog
    {
        private readonly ILogger<Log> _logger;

        public Log(ILogger<Log> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message, params object?[] args)
        {
            DateTime dateTime = DateTime.Now;
            string datalog = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            string formattedMessage = $"{datalog}: {message}";
            _logger.LogInformation(formattedMessage, args);
        }

        public void LogWarning(string message, params object?[] args)
        {
            DateTime dateTime = DateTime.Now;
            string datalog = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            string formattedMessage = $"{datalog}: {message}";
            _logger.LogWarning(formattedMessage, args);
        }
    }
}
