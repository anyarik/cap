using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace CAP.Bot.Telegram.Handlers.Commands
{
    public class HelpCommand : CommandBase
    {
        public override async Task HandleAsync(
            IUpdateContext context,
            UpdateDelegate next,
            string[] args
        )
        {
            string text = $"Hi {context.Update.Message.From.FirstName}!\n" + ""
                          /* "You can use /new_task for create a new task."*/;

            await context.Bot.Client.SendTextMessageAsync(
                context.Update.Message.Chat.Id,
                text,
                ParseMode.Default,
                replyToMessageId: context.Update.Message.MessageId
            ).ConfigureAwait(false);
        }
    }
}
