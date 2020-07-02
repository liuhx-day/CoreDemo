using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.Services;
using CoreDemo.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreDemo
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /* 注册MVC一些相关服务到容器中
             */
            services.AddMvc();
            /* 生命周期固定，每当有其他的类来请求ICinemaService的时候，容器都会返回CinemaMemoryService的实例
            * Singleton 一旦被创建实例，就会一直使用这个实例，直到应用停止  适合内存数据 又叫单例模式
            * 在Controller的构造方法里都可以注入以下的接口类，注入ICinemaService//IMovieService
            * 也可以在Razor里注入
            * 依赖注入的好处：
            * 不用去管生命周期
            * 类型之间没有依赖 有利进行单元测试
             */
            services.AddSingleton<ICinemaService, CinemaMemoryService>();
            services.AddSingleton<IMovieService, MovieMemoryService>();
            //将配置文件映射成类，强类型 可直接注入到Controller
            services.Configure<ConnectionOptions>(_configuration.GetSection("ConnectionStrings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /* 配置Http请求的管道
         * IApplicationBuilder和IHostingEnvironment被注入进来
         * 这个管道告诉我们这个web应用如何来响应http的请求，请求钻进管道里
         * 管道里的东西就是中间件 MVC也是中间件
         
             */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILogger<Startup> logger)
        {
            // IHostingEnvironment 这里判断环境变量是否等于lau
            if (env.IsDevelopment())
            {            
                /* 传统.net项目 报错会有一个黄色的页面
                 * core里戳app.UseDeveloperExceptionPage(); 最好只在开发的时候使用，报错给用户会泄露安全
                 */
                app.UseDeveloperExceptionPage();
            }





            // 作用是把错误直接写到网页上
            app.UseStatusCodePages();
            /* 自定义错误跳转
             *  app.UseStatusCodePagesWithRedirects();
             *
                */
            // 添加wwwroot
            app.UseStaticFiles();

            // 最简单的使用MVC 缺少路由
            app.UseMvc(routes => 
            { 
                routes.MapRoute(
                    name: "default", 
                    template: "{controller=Home}/{action=Index}/{id?}"); 
            });


            ///* 遇到请求就返回hello world
            // * 
            // */
            //app.Use(async (context,next) =>
            //{
            //    logger.LogInformation("M1 Start");
            //    await context.Response.WriteAsync("Hello World!");
            //    await next();
            //    logger.LogInformation("M1 End");
            //});
            ///* 运行后还是执行Hello world 说明这个中间件没有执行，因为没有调用这个中间件 app.Run 不会调用下一个中间件
            // * 如果想调用下一个中间件就不能用app.Run
            // * 用app.Use
            // * 这样就继续调用了
            // */
            //app.Run(async (context) =>
            //{
            //    logger.LogInformation("M2 Start");
            //    await context.Response.WriteAsync("Another Hello!");
            //    logger.LogInformation("M2 End");
            //});
        }
    }
}
