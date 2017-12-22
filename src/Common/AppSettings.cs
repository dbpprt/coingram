namespace CoinGram.Common
{
    public class AppSettings
    {
        public string Version { get; set; }

        public string TelegramApiKey { get; set; }

        public string CoinigyApiKey { get; set; }

        public string CoinigyApiSecret { get; set; }

        public string InfluxDbHost { get; set; }

        public string InfluxDbDatabase { get; set; }

        public string InfluxDbUser { get; set; }

        public string InfluxDbPassword { get; set; }
    }
}
