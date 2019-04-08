using System;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Handlers.TaskMenuControl.Common;
using CAP.Bot.Telegram.Services;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Requests;

namespace CAP.Bot.Telegram.Handlers.Commands
{
    public class AllTasksCommand : CommandBase
    {
        private readonly IDemoService demoService;

        public AllTasksCommand(IDemoService demoService)
        {
            this.demoService = demoService;
        }

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
        {
            var tasks = await this.demoService.GetAllTasks();
            var taskTitles = tasks.OrderBy(t => t.Title).Take(5).Select(t => t.Title).ToArray(); 
            await context.Bot.Client.MakeRequestAsync(
               new SendMessageRequest(
                   context.Update.Message.Chat.Id,
                   $"Выбирите таск (1/{(int)Math.Ceiling(tasks.Count / 5 * 1d) + 1})"
               )
               {
                   ReplyMarkup = Markup.CreateTaskInlineKeyboard(taskTitles)
               }
           ).ConfigureAwait(false);
            }
    }
}
