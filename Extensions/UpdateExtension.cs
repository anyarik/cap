using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Models;
using Microsoft.AspNetCore.Http;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace CAP.Bot.Telegram.Extensions
{
    public static class UpdateExtensions
    {
        /// <summary>
        /// Indicates if this update is received as a webhook
        /// </summary>
        public static bool IsWebhook(this IUpdateContext context) =>
            context.Items.ContainsKey(nameof(HttpContext));

        public static UserChat ToUserchat(this Update update)
        {
            long chatId = 0, userId = 0;
            // todo use UpdateType enum instead

            if (update.Message != null)
            {
                chatId = update.Message.Chat.Id; // todo check conversion
                userId = update.Message.From.Id;
            }
            else if (update.CallbackQuery != null)
            {
                chatId = update.CallbackQuery.From.Id;
                userId = update.CallbackQuery.Message.Chat.Id;
            }

            return new UserChat(userId, chatId);
        }
    }
}
