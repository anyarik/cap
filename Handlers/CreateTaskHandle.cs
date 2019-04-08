using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Requests;

namespace CAP.Bot.Telegram.Handlers
{
    public class CreateTaskHandle : IUpdateHandler
    {
        public IMemoryCache Cache { get; }

        public CreateTaskHandle( IMemoryCache cache)
        {
            Cache = cache;
        }

        public static bool CanHandle(IUpdateContext context)
        {
            return false;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var userChat = context.Update.ToUserchat();
            var resourceTask = this.Cache.GetResourceTask(userChat);

            if (resourceTask != null)
            {
                if (String.IsNullOrEmpty(resourceTask.Title))
                {
                    resourceTask.Title = context.Update.Message.Text;

                    this.Cache.UpdateResourceTask(userChat, resourceTask);

                    await context.Bot.Client.MakeRequestAsync(
                        new SendMessageRequest(
                            context.Update.Message.Chat.Id,
                            $"Enter Description\n"
                        )
                    ).ConfigureAwait(false);
                }
                else if (String.IsNullOrEmpty(resourceTask.Description))
                {
                    resourceTask.Description = context.Update.Message.Text;

                    this.Cache.UpdateResourceTask(userChat, resourceTask);

                    await context.Bot.Client.MakeRequestAsync(
                        new SendMessageRequest(
                            context.Update.Message.Chat.Id,
                            $"Task created\n"
                        )
                    ).ConfigureAwait(false);

                    this.Cache.RemoveResourceTask(userChat);
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}
