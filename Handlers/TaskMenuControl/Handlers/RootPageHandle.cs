using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Handlers.TaskMenuControl.Common;
using CAP.Bot.Telegram.Services;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Framework.Abstractions;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Handlers
{
    public class RootPageHandle : BaseTaskMenu, IUpdateHandler
    {
        public IDemoService DemoService { get; }

        public RootPageHandle( IDemoService demoService, IMemoryCache cache) : base(cache)
        {
            DemoService = demoService;
        }

        public static bool CanHandle(IUpdateContext context) =>
            Validator.IsRootPage(context.Update.CallbackQuery?.Data);


        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var queryData = context.Update.CallbackQuery.Data;
            var pageStr = queryData.Substring(Constants.RootPage.Length).Split(":")[0];
            var page = int.Parse(pageStr);

            var allTask = await this.DemoService.GetAllTasks().ConfigureAwait(false);
            var tasks = allTask.OrderBy(t => t.Title)
                .Skip(page * Constants.CountOnPage)
                .Take(Constants.CountOnPage);
            var taskTitles = tasks.Select(t => t.Title).ToArray();

            var inlineKeyboard = Markup.CreateTaskInlineKeyboard(taskTitles, page);
            await RefreshInlineMenu(context, inlineKeyboard, $"Выбирите таск ({page + 1}/{(int)Math.Ceiling(allTask.Count / Constants.CountOnPage * 1d) + 1})").ConfigureAwait(false);
        }
    }
}
