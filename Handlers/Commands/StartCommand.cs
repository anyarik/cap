using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.ReplyMarkups;

namespace CAP.Bot.Telegram.Handlers.Commands
{
    public class StartCommand : CommandBase
    {
        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
        {
            await context.Bot.Client.SendTextMessageAsync(context.Update.Message.Chat.Id, $"Hello {context.Update.Message.From.FirstName}!\n",
                replyMarkup: new ReplyKeyboardMarkup(new[] { new KeyboardButton("/help"), /*new KeyboardButton("/new_task"), */new KeyboardButton("/all_tasks"), },true)).ConfigureAwait(false);


            //await context.Bot.Client.MakeRequestAsync(
            //    new SendMessageRequest(
            //        context.Update.Message.Chat.Id,
            //        $"Hello {context.Update.Message.From.FirstName}!\n"
            //    )
            //).ConfigureAwait(false);
        }
    }
}
