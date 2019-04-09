using System;
using System.Net.Http;
using System.Xml;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CalendarPicker;
using CalendarPicker.CalendarControl;
using CalendarPicker.CalendarControl.Handlers;
using CalendarPicker.Services;
using CAP.Bot.Telegram.Extensions;
using CAP.Bot.Telegram.Handlers;
using CAP.Bot.Telegram.Handlers.Commands;
using CAP.Bot.Telegram.Handlers.TaskMenuControl;
using CAP.Bot.Telegram.Options;
using CAP.Bot.Telegram.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.BuildingBlocks.EventBus;
using Microsoft.BuildingBlocks.EventBus.Abstractions;
using Microsoft.BuildingBlocks.EventBusRabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models.Map;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using Repositories.PlanRepo;
using Repositories.ResourceRepo;
using Repositories.ResourceTaskRepo;
using Telegram.Bot.Framework;
using VallySoft.Dapper.CommonQuery.Providers;

namespace CAP.Bot.Telegram
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IBotService>(s => new BotService(""));
            var connectionString = "User ID=postgres;Password=;Host=84.201.154.141;Port=5432;Database=CAP.Prod;Pooling=true";
            VS.Dapper.SqlMapper.MappingContext.LoadMaps<Empty>();
            this.InitDI(services, connectionString);

            services.AddMemoryCache();

            services.AddTransient<ManagerBot>()
                    .Configure<BotOptions<ManagerBot>>(Configuration.GetSection("Bot"))
                    .Configure<CustomBotOptions<ManagerBot>>(Configuration.GetSection("Bot"))
                    .AddScoped<WebhookResponse>()
                    .AddScoped<StartCommand>()
                    .AddScoped<HelpCommand>()
                    .AddScoped<NewTaskCommand>()
                    .AddScoped<AllTasksCommand>()
                    .AddScoped<CreateTaskHandle>()
                    .AddScoped<CalendarCommand>()
                    .CalendarCollection()
                    .TaskMenuCollection()
                    ;

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = "84.201.154.141"
                };

                factory.UserName = "guest";
                factory.Password = "guest";

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            RegisterEventBus(services);

            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

            var botBuilder = new BotBuilder()
                    .Use<WebhookResponse>()
                    
                    .UseCommand<StartCommand>("start")
                    .UseCommand<HelpCommand>("help")
                    .UseCommand<AllTasksCommand>("all_tasks")

                    .TaskMenuHandlers()

                    .UseWhen<CreateTaskHandle>(CreateTaskHandle.CanHandle)

                    .UseWhen(When.NewMessage, msgBranch => msgBranch
                        .UseWhen(When.NewTextMessage, txtBranch => txtBranch
                            .UseCommand<NewTaskCommand>("new_task")
                        )
                    )
                    .UseCommand<CalendarCommand>("calendar")
                    .CalendarHandlers()
                    ;

            //var httpClient = new HttpClient();
            //var content = httpClient.GetAsync("http://127.0.0.1:4040/api/tunnels").GetAwaiter().GetResult()
            //                        .Content.ReadAsStringAsync().GetAwaiter().GetResult();
            //var tunnelAddress = ((string)JObject.Parse(content)["tunnels"][0]["public_url"]).Replace("http:", "https:");

            app.UseTelegramBotWebhook<ManagerBot>(botBuilder);
            app.EnsureWebhookSet<ManagerBot>();

            app.Run(async context => { await context.Response.WriteAsync("Hello World!"); });

            ConfigureEventBus(app);
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionClientName = "cap_bot_test";

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                ILifetimeScope iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });


            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            //eventBus.Subscribe<StartPlanningIntegrationEvent, StartPlanningIntegrationEventHandler>();

        }

        private void InitDI(IServiceCollection services, string connectionString)
        {
            //services.AddSingleton(
            //    Configuration.GetSection("CalendarBot").Get<BotConfiguration>()
            //);

            services.AddTransient<ISessionProvider>((a) => new SessionProvider(connectionString));

            services.AddTransient<IDemoService, DemoService>();
            services.AddTransient<IPlanRepository, PlanRepository>();
            services.AddTransient<IResourceTaskRepository, ResourceTaskRepository>();
            services.AddTransient<IResourceRepository, ResourceRepository>();

            services
               .AddTransient<LocalizationService>();

        }
    }
}
