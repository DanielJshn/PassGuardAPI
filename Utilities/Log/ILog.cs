namespace apief
{
    public interface ILog
    {
        public void LogInfo(string message, params object?[] args);
        public void LogWarning(string message, params object?[] args);
    }
}