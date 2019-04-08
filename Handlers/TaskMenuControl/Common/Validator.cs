using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Common
{
    public static class Validator
    {
        public static bool IsRootPage(string data)         => data?.StartsWith(Constants.RootPage) ?? false;
        public static bool IsSelectedTask(string data)     => data?.StartsWith(Constants.SelectedTask) ?? false;
        public static bool IsSelectedOption(string data)   => data?.StartsWith(Constants.SelectedOption) ?? false;
        public static bool IsSelectedResource(string data) => data?.StartsWith(Constants.SelectedResource) ?? false;
        public static bool IsCancel(string data)           => data?.StartsWith(Constants.Cancel) ?? false;
        public static bool IsSelectedDeadline(string data) => data?.StartsWith(CalendarPicker.CalendarControl.Constants.PickDate) ?? false;
        public static bool IsSelectedHourTask(string data) => int.TryParse(data, out _);

        public static int GetPageNumber(int page) => page < 0 ? 0 : (page > Constants.MaxPageCount ? Constants.MaxPageCount : page);

    }
}
