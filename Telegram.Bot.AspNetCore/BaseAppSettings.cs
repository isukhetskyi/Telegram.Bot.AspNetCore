namespace Telegram.Bot.AspNetCore
{
    public class BaseAppSettings
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public string RollbarToken { get; set; }
        public string RollbarEnvironment { get; set; }
    }
}