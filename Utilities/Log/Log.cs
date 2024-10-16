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

            _logger.LogInformation("{Timestamp}: " + message + datalog + args);
        }

        public void LogWarning(string message, params object?[] args)
        {
            DateTime dateTime = DateTime.Now;
            string datalog = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            _logger.LogWarning("{Timestamp}: " + message + datalog + args);
        }

    }
}