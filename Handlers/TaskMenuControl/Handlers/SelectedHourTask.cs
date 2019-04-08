using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SelectedHourTask : BaseTaskMenu, IUpdateHandler
    {
        public IEventBus EventBus { get; }
        public IDemoService DemoService { get; }

        public SelectedHourTask(IEventBus eventBus, IDemoService demoService, IMemoryCache cache) : base(cache)
        {
            EventBus = eventBus;
            DemoService = demoService;
        }

        public static bool CanHandle(IUpdateContext context) =>
            Validator.IsSelectedHourTask(context.Update?.Message?.Text);


        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            int.TryParse(context.Update.Message.Text, out var hour);

            var task = this.Cache.GetTask(context.Update.ToUserchat());

            if (task == null)
            {
                await context.Bot.Client.MakeRequestAsync(
                    new SendMessageRequest(
                        context.Update.Message.Chat,
                        $"Выбирите таск.\n"
                    )
                ).ConfigureAwait(false);

                return;
            }

            var collectiveId = await this.DemoService.GetLastCollectivePlanId();
            var message = new PlanChangeDurationTimeEvent(collectiveId, task.Id, (int) TimeSpan.FromHours(hour).TotalMinutes);
            await this.EventBus.Publish(message);

            await context.Bot.Client.MakeRequestAsync(
                new SendMessageRequest(
                    context.Update.Message.Chat,
                    $"В таске '{task.Title}' успешно увеличено время работы на '{hour}' часов.\n"
                )
            ).ConfigureAwait(false);

            //await Exit(context).ConfigureAwait(false);
        }
    }
}
