﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace SocketAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ConfigureCors(services);
            ConfigureSignalR(services);
            Transfuse(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase("/service");   // 启用二级目录
            app.UseCors("SignalR");        // 启用跨域
            app.UseHttpsRedirection();     // 启用http上下文

            app.UseMvc();

            //注册ChatHub
            app.UseSignalR(builder =>
            {
                builder.MapHub<Hub.Hub>("/ws");
            });
        }

        /// <summary>
        /// 跨域配置
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("SignalR",
                builder =>
                {
                    builder
                        .AllowAnyMethod()    // 允许任何方法
                        .AllowAnyHeader()    // 允许任何头
                        .AllowAnyOrigin()    // 允许任何源
                        .AllowCredentials(); // 允许任何证书
                }));
        }

        /// <summary>
        /// SignalR配置
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureSignalR(IServiceCollection services)
        {
            services.AddSignalR(options =>
                {
                    options.KeepAliveInterval = TimeSpan.FromSeconds(60); // 心跳包
                    options.EnableDetailedErrors = true;                  // 输出错误详细
                    options.HandshakeTimeout = TimeSpan.FromSeconds(60);  // 超时时长
                })
                .AddRedis(option =>
                {
                    option.ConnectionFactory = async writer =>
                    {
                        var connection = await ConnectionMultiplexer.ConnectAsync("118.24.27.231:6379,password=sun940622");
                        connection.ConnectionFailed += (sender, e) =>           // redis连接失败记录日志
                        {
                            // todo 
                        };
                        connection.ErrorMessage += (sender, e) =>               // redis 发生错误异常的时候
                        {
                            // todo 
                        };
                        return connection;
                    };
                });
        }

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="services"></param>
        public void Transfuse(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // 注入http请求的上下文
        }
    }
}
