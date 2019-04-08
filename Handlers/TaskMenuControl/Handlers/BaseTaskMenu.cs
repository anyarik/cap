using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;
using VS.Extensions.Collections;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Handlers
{
    public class BaseTaskMenu
    {
        public IMemoryCache Cache { get; }

        public BaseTaskMenu( IMemoryCache cache)
        {
            Cache = cache;
        }

        protected async Task RefreshInlineMenu(IUpdateContext context, InlineKeyboardMarkup inlineKeyboard, string messageText = "")
        {
            if (messageText.IsNullOrEmpty())
            {
                var message = context.Update.CallbackQuery.Message;
                await context.Bot.Client.EditMessageReplyMarkupAsync(
                    message.Chat,
                    message.MessageId,
                    inlineKeyboard
                ).ConfigureAwait(false);
            }
            else
            {
                var message = context.Update.CallbackQuery.Message;
                await context.Bot.Client.EditMessageTextAsync(
                    message.Chat.Id,
                    message.MessageId,
                    messageText,
                    replyMarkup: inlineKeyboard
                ).ConfigureAwait(false);
            }
        }

        protected async Task Exit(IUpdateContext context)
        {
            this.Cache.RemoveTask(context.Update.ToUserchat());

            // закрыть инлайн меню и вызвать меню стартовой страницы
            await RefreshInlineMenu(context, InlineKeyboardMarkup.Empty()).ConfigureAwait(false);
        }
    }
}
