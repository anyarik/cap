using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl.Common
{
    public class Markup
    {
        public static InlineKeyboardMarkup CreateTaskInlineKeyboard(string[] taskTitles, int page = 0) =>
            new InlineKeyboardMarkup(
                taskTitles
                    .Select(c => new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{c}", $"{Constants.SelectedTask}{c}")
                    })
                    .Prepend(new[]
                    {
                        InlineKeyboardButton.WithCallbackData("<-", $"{Constants.RootPage}{Validator.GetPageNumber(page - 1)}:"),
                        InlineKeyboardButton.WithCallbackData("->", $"{Constants.RootPage}{Validator.GetPageNumber(page + 1)}:")
                    })
                    .Prepend(new[] { InlineKeyboardButton.WithCallbackData("Отмена", $"{Constants.Cancel}") })
            );

        public static InlineKeyboardMarkup CreateTaskOptionInlineKeyboard()
        {
            var i = 0;
            return new InlineKeyboardMarkup(
                new[] { "Изменить дейдлайн", /*"Изменить исполнителя",*/ "Увеличить время работы" }
                    .Select(c => new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{c}", $"{Constants.SelectedOption}{i++}")
                    })
                    .Prepend(new[] { InlineKeyboardButton.WithCallbackData("На выбор таска", $"{Constants.RootPage}0:") })
            );
        }


        public static InlineKeyboardMarkup CreateTaskResourceInlineKeyboard(string[] usernames) =>
            new InlineKeyboardMarkup(
                usernames
                    .Select(c => new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{c}", $"{Constants.SelectedResource}{c}")
                    })
                    .Prepend(new[] { InlineKeyboardButton.WithCallbackData("На выбор таска", $"{Constants.RootPage}0:") })
            );
    }
}
