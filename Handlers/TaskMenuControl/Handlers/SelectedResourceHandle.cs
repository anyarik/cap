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
using Models.ResourceModel;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Requests;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Handlers
{
    public class SelectedResourceHandle : BaseTaskMenu, IUpdateHandler
    {
        public IDemoService DemoService { get; }
        public IEventBus EventBus { get; }

        public SelectedResourceHandle(IDemoService demoService, IEventBus eventBus, IMemoryCache cache) : base(cache)
        {
            DemoService = demoService;
            EventBus = eventBus;
        }

        public static bool CanHandle(IUpdateContext context) =>
            Validator.IsSelectedResource(context.Update.CallbackQuery?.Data);

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var queryData = context.Update.CallbackQuery.Data;

            var username = queryData.Substring(Constants.SelectedResource.Length);
            var task = this.Cache.GetTask(context.Update.ToUserchat());

            var users = await this.DemoService.GetAllUsers();
            var selectedResource = users.FirstOrDefault(u => u.Username == username);

            var collectiveId = await this.DemoService.GetLastCollectivePlanId();
            var message = new PlanChangeExecutorEvent(collectiveId, task.Id, new Resource() {Id = selectedResource.ResourceId});
            await this.EventBus.Publish(message);

            await context.Bot.Client.MakeRequestAsync(
                new SendMessageRequest(
                    context.Update.CallbackQuery.Message.Chat,
                    $"В таске '{task.Title}' успешно изменен исполнитель на '{username}'.\n"
                )
            ).ConfigureAwait(false);

            await Exit(context).ConfigureAwait(false);
        }
    }
}
