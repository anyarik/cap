using CalendarPicker.CalendarControl.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace CalendarPicker.CalendarControl
{
    public static class CalendarControlExtension
    {
        //public static IServiceCollection AddCalendarBot(this IServiceCollection services, IConfigurationSection botConfiguration) =>
        //    services.AddTransient<CalendarBot>()
        //        .Configure<BotOptions<CalendarBot>>(botConfiguration)
        //        .Configure<CustomBotOptions<CalendarBot>>(botConfiguration)
        //        .AddScoped<FaultedUpdateHandler>()
        //        .AddScoped<CalendarCommand>()
        //        .AddScoped<ChangeToHandler>()
        //        .AddScoped<PickDateHandler>()
        //        .AddScoped<YearMonthPickerHandler>()
        //        .AddScoped<MonthPickerHandler>()
        //        .AddScoped<YearPickerHandler>();

        //public static IServiceCollection AddOperationServices(this IServiceCollection services) =>
        //    services
        //        .AddTransient<LocalizationService>();

        public static IBotBuilder CalendarHandlers(this IBotBuilder builder) =>
            builder.UseWhen<ChangeToHandler>(ChangeToHandler.CanHandle)
                .UseWhen<YearMonthPickerHandler>(YearMonthPickerHandler.CanHandle)
                .UseWhen<MonthPickerHandler>(MonthPickerHandler.CanHandle)
                .UseWhen<YearPickerHandler>(YearPickerHandler.CanHandle);

        public static IServiceCollection CalendarCollection(this IServiceCollection collection) =>
            collection.AddScoped<ChangeToHandler>()
                    .AddScoped<YearMonthPickerHandler>()
                    .AddScoped<MonthPickerHandler>()
                    .AddScoped<YearPickerHandler>();
    }
}
