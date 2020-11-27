using Bari.Api.AMQP;
using Bari.Api.Consumers;
using Bari.Api.EventBus;
using Bari.Api.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Bari.Api
{
    public class Startup
    {
        public static IServiceProvider Provider { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Bari" });
            });

            services.AddSingleton<IHelloWorldEventBusManager, HelloWorldEventBusManager>();
            services.AddSingleton<IRabbitMQManager, RabbitMQManager>();
            services.AddTransient<IHellorWorldConsumer, HellorWorldConsumer>();

            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .WithOrigins("http://localhost:55371")
                      .AllowCredentials();
                });
            });

            services.AddSwaggerGenNewtonsoftSupport();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHelloWorldEventBusManager eventBusManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("ClientPermission");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<QueuetHub>("/hubs/queue");
            });

            app.UseSwagger();


            eventBusManager.Subscribe();
            eventBusManager.PublishMessageEvery("Hello World", 5000);

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Bari");
                c.RoutePrefix = string.Empty;
            });

        }
    }
}