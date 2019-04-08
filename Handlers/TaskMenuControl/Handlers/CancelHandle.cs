using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Handlers.TaskMenuControl.Common;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Framework.Abstractions;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Handlers
{
    public class CancelHandle : BaseTaskMenu, IUpdateHandler
    {
        public CancelHandle(IMemoryCache cache) : base(cache)
        {
        }

        public static bool CanHandle(IUpdateContext context) =>
            Validator.IsCancel(context.Update.CallbackQuery?.Data);

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            await Exit(context).ConfigureAwait(false);
        }
    }
}
