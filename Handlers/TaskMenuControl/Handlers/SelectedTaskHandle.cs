using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Extensions;
using CAP.Bot.Telegram.Handlers.TaskMenuControl.Common;
using CAP.Bot.Telegram.Services;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Framework.Abstractions;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Handlers
{
    public class SelectedTaskHandle : BaseTaskMenu, IUpdateHandler
    {
        public IDemoService DemoService { get; }

        public SelectedTaskHandle(IMemoryCache cache, IDemoService demoService) : base(cache)
        {
            DemoService = demoService;
        }

        public static bool CanHandle(IUpdateContext context) =>
            Validator.IsSelectedTask(context.Update.CallbackQuery?.Data);

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var queryData = context.Update.CallbackQuery.Data;

            var title = queryData.Substring(Constants.SelectedTask.Length);
            var allTasks = await this.DemoService.GetAllTasks();
            var task = allTasks.FirstOrDefault(t => t.Title == title);

            this.Cache.UpdateTask(context.Update.ToUserchat(), task);

            var inlineKeyboard = Markup.CreateTaskOptionInlineKeyboard();
            await RefreshInlineMenu(context, inlineKeyboard, "Выбирите опцию").ConfigureAwait(false);
        }
    }
}
