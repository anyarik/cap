using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Models.ResourceTaskModel;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.ReplyMarkups;

namespace CAP.Bot.Telegram.Handlers.Commands
{
    public class NewTaskCommand : CommandBase
    {
        public IMemoryCache Cache { get; }

        public NewTaskCommand( IMemoryCache cache)
        {
            Cache = cache;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
        {
            
            await context.Bot.Client.MakeRequestAsync(
                new SendMessageRequest(
                    context.Update.Message.Chat.Id,
                    $"Create new task enter title!\n"
                )
            ).ConfigureAwait(false);

            this.Cache.UpdateResourceTask(context.Update.ToUserchat(), new ResourceTask());
        }
    }
}
