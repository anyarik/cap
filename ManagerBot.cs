using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace CAP.Bot.Telegram
{
    public class ManagerBot : BotBase
    {
        protected ManagerBot(string username, ITelegramBotClient client) : base(username, client)
        {
        }

        public ManagerBot(IOptions<BotOptions<ManagerBot>> options) : base(options.Value)
        {
        }
    }
}
