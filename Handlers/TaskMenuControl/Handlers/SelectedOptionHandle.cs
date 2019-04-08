using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Extensions;
using CAP.Bot.Telegram.Handlers.TaskMenuControl.Common;
using CAP.Bot.Telegram.Services;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;
using VS.Extensions.BCL;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Handlers
{
    public class SelectedOptionHandle : BaseTaskMenu, IUpdateHandler
    {
        public IDemoService DemoService { get; }

        public SelectedOptionHandle(IMemoryCache cache, IDemoService demoService) : base(cache)
        {
            DemoService = demoService;
        }

        public static bool CanHandle(IUpdateContext context) =>
            Validator.IsSelectedOption(context.Update.CallbackQuery?.Data);

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var queryData = context.Update.CallbackQuery.Data;
            var type = queryData.Substring(Constants.SelectedOption.Length).ToInt();

            switch (type)
            {
                case 0:
                {
                    var calendarMarkup = CalendarPicker.CalendarControl.Markup.Calendar(DateTime.Today,
                        new CultureInfo("ru-RU", false).DateTimeFormat);

                    var calendarWithCancelMakup = new InlineKeyboardMarkup(calendarMarkup.InlineKeyboard
                        .Prepend(new[]
                            {InlineKeyboardButton.WithCallbackData("На выбор таска", $"{Constants.RootPage}0:")}));

                    var task = this.Cache.GetTask(context.Update.ToUserchat());

                    await RefreshInlineMenu(context, calendarWithCancelMakup,
                        $"Выбирите дату дедлайна (текущий {task.DeadlineDate:d})").ConfigureAwait(false);
                    break;
                }
                case 1:
                {
                    await RefreshInlineMenu(context, InlineKeyboardMarkup.Empty(), $"Увеличить время выполнение на: (вписать кол-во часов)");

                        //var resources = await this.DemoService.GetAllUsers();

                        //var resorceTitles = resources.Select(r => r.Username).ToArray();

                        //var inlineKeyboard = Markup.CreateTaskResourceInlineKeyboard(resorceTitles);
                        //await RefreshInlineMenu(context, inlineKeyboard, "Выбирите исполнителя").ConfigureAwait(false);

                        break;
                }
                case 2:
                {
                    await RefreshInlineMenu(context, InlineKeyboardMarkup.Empty(), $"Увеличить время выполнение на: (вписать кол-во часов)");
                    break;
                }

                default:
                    break;
            }
        }
    }
}
