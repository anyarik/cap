using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Common
{
    public static class Constants
    {
        public const string RootPage         = "t/0/"; // 't/0/<номер страницы>:' - перемещение по стартовому меню 
        public const string SelectedTask     = "t/1:"; // 't/1:'                  - меню тайтлов тасков с кнопками вперед/назад отображаются по 5 позиций
        public const string SelectedOption   = "t/2:"; // 't/2:'                  - меню опций {"Изменить дейдлайн", "Изменить исполнителя"} возвращаемые значения {0,1} соотвественно
        public const string SelectedResource = "t/3:"; // 't/3:'                  - меню выбора исполнителя, выбор по username
        public const string Cancel           = "t/c";  // 't/c'                   - отмена, завешается работа с менб тасков

        public const int CountOnPage = 5;
        public const int MaxPageCount = 20;

    }
}
