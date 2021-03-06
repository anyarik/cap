﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Options;
using CAP.Bot.Telegram.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace CAP.Bot.Telegram.Extensions
{
    internal static class AppStartupExtensions
    {
        public static void UseTelegramBotLongPolling<TBot>(
            this IApplicationBuilder app,
            IBotBuilder botBuilder,
            TimeSpan startAfter = default,
            CancellationToken cancellationToken = default
        )
            where TBot : BotBase
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            if (startAfter == default)
            {
                startAfter = TimeSpan.FromSeconds(2);
            }

            var updateManager = new UpdatePollingManager<TBot>(botBuilder, new BotServiceProvider(app));

            Task.Run(async () =>
            {
                await Task.Delay(startAfter, cancellationToken)
                    .ConfigureAwait(false);
                await updateManager.RunAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }, cancellationToken
                )
                .ContinueWith(t =>
                {
                    logger.LogError(t.Exception, "Bot update manager failed.");
                    throw t.Exception;
                }, TaskContinuationOptions.OnlyOnFaulted
                );
        }

        public static void EnsureWebhookSet<TBot>(this IApplicationBuilder app, string webHoodDomain = null)
            where TBot : IBot
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var bot = scope.ServiceProvider.GetRequiredService<TBot>();
                var options = scope.ServiceProvider.GetRequiredService<IOptions<CustomBotOptions<TBot>>>();
                var url = new Uri(new Uri( webHoodDomain ?? options.Value.WebhookDomain), options.Value.WebhookPath);

                logger.LogDebug("Setting webhook for bot \"{0}\" to URL \"{1}\"", typeof(TBot).Name, url);

                bot.Client.SetWebhookAsync(url.AbsoluteUri)
                    .GetAwaiter().GetResult();
            }
        }
    }
}
