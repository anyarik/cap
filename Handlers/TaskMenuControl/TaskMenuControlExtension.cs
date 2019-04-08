using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalendarPicker.CalendarControl.Handlers;
using CAP.Bot.Telegram.Handlers.TaskMenuControl.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace CAP.Bot.Telegram.Handlers.TaskMenuControl
{
    public static class TaskMenuControlExtension
    {
        public static IBotBuilder TaskMenuHandlers(this IBotBuilder builder) =>
            builder.UseWhen<RootPageHandle>(RootPageHandle.CanHandle)
                .UseWhen<SelectedTaskHandle>(SelectedTaskHandle.CanHandle)
                .UseWhen<SelectedOptionHandle>(SelectedOptionHandle.CanHandle)
                .UseWhen<SelectedResourceHandle>(SelectedResourceHandle.CanHandle)
                .UseWhen<SelectedDeadlineHandle>(SelectedDeadlineHandle.CanHandle)
                .UseWhen<SelectedHourTask>(SelectedHourTask.CanHandle)
                .UseWhen<CancelHandle>(CancelHandle.CanHandle);

        public static IServiceCollection TaskMenuCollection(this IServiceCollection collection) =>
            collection.AddScoped<RootPageHandle>()
                .AddScoped<SelectedTaskHandle>()
                .AddScoped<SelectedOptionHandle>()
                .AddScoped<CancelHandle>()
                .AddScoped<SelectedDeadlineHandle>()
                .AddScoped<SelectedHourTask>()
                .AddScoped<SelectedResourceHandle>();
    }
}
