using System.Globalization;

namespace CalendarPicker.Services
{
    public class LocalizationService
    {
        //private readonly BotConfiguration _configuration;

        public DateTimeFormatInfo DateCulture;

        public LocalizationService()
        {
            DateCulture = new CultureInfo("ru-RU", false).DateTimeFormat;
        }
    }
}
