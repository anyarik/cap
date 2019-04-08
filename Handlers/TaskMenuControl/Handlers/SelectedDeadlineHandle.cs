using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CalendarPicker.CalendarControl;
using CAP.Bot.Telegram.Extensions;
using CAP.Bot.Telegram.Handlers.TaskMenuControl.Common;
using CAP.Bot.Telegram.Services;
using Events.Planning;
using Microsoft.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Requests;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Handlers
{
    public class SelectedDeadlineHandle : BaseTaskMenu, IUpdateHandler
    {
        public IDemoService DemoService { get; }
        public IEventBus EventBus { get; }

        public SelectedDeadlineHandle(IDemoService demoService, IEventBus eventBus, IMemoryCache cache) : base(cache)
        {
            DemoService = demoService;
            EventBus = eventBus;
        }

        public static bool CanHandle(IUpdateContext context) =>
            Validator.IsSelectedDeadline(context.Update.CallbackQuery?.Data);

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            DateTime.TryParseExact(
                context.Update.TrimCallbackCommand(CalendarPicker.CalendarControl.Constants.PickDate),
                CalendarPicker.CalendarControl.Constants.DateFormat,
                null,
                DateTimeStyles.None,
                out var date);

            var task = this.Cache.GetTask(context.Update.ToUserchat());

            var collectiveId = await this.DemoService.GetLastCollectivePlanId();
            var message = new PlanChangeDeadlineEvent(collectiveId, task.Id, date);
            await this.EventBus.Publish(message);
            
            await context.Bot.Client.MakeRequestAsync(
                new SendMessageRequest(
                    context.Update.CallbackQuery.Message.Chat,
                    $"В таске '{task.Title}' успешно изменен дедлайн на '{date:d}'.\n"
                )
            ).ConfigureAwait(false);

            await Exit(context).ConfigureAwait(false);
        }
    }
}
