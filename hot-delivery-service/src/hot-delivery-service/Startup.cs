using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;
using hot_delivery_service.Queries;
using hot_delivery_service.Persistence;
using hot_delivery_service.CommandHandlers;
using hot_delivery_service.Helpers;
using hot_delivery_service.Persistence.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.DotNet.InternalAbstractions;
using hot_delivery_service.Scheduler;
using Quartz.Spi;

namespace hot_delivery_service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string appBasePath = ApplicationEnvironment.ApplicationBasePath;

            services.AddOptions();

            //чтение опций из конфига и регистрация в контейнере
            services.Configure<SchedulerOptions>(Configuration);
            services.Configure<StorageOptions>(Configuration);

            //регистрация EF контекста для SQLite
            services.AddDbContext<DeliveryContext>(
                options => { options.UseSqlite($"Data Source=deliveries.db"); });

            //Регистрация всех компонент
            services.AddTransient<IDeliveryWorkUnitProvider, DeliveryWorkUnitProvider>();
            services.AddTransient<IDeliveryQuery, DeliveryQuery>();
            services.AddTransient<IDeliveryCommandHandler, DeliveryCommandHandler>();
            services.AddSingleton<IDeliveryScheduler, DeliveryScheduler>();

            //регистрация кастомной jobfactory и задач для Quartz net для возможности dependency injection в задачах
            services.AddTransient<IJobFactory, CustomJobFactory>();
            services.AddTransient<CreateDeliveryJob, CreateDeliveryJob>();
            services.AddTransient<ExpireDeliveriesJob, ExpireDeliveriesJob>();
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //обработка необрабоанных исключений
            app.UseExceptionHandler(errorApp =>
            {
                //пишем в ответ код 500
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var ex = error.Error;

                        await context.Response.WriteAsync("InternalServerError");
                    }
                });
            });

            app.UseMvc();

            //Запуск планировщика задач
            var scheduler = serviceProvider.GetService<IDeliveryScheduler>();
            scheduler.StartTasks(); 
        }
    }
}
