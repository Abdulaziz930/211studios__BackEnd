using AspNetCoreRateLimit;
using Business.Abstract;
using Business.Concret;
using Common;
using DataAccess.Abstract;
using DataAccess.AutoMapper;
using DataAccess.Concret;
using Entities.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace _211_Studios
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            LogManager.LoadConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _environment;

        public void ConfigureServices(IServiceCollection services)
        {
            #region Db

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            #endregion

            #region AutoMapper

            services.AddAutoMapper(typeof(AutoMapperProfile));

            #endregion

            #region Scoped

            services.RegisterAllTypes(ServiceLifetime.Scoped, typeof(ISliderDal));
            services.RegisterAllTypes(ServiceLifetime.Scoped, typeof(ISliderService));

            #endregion

            #region Cors

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins(Configuration["ClientPort:Port"])
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            #endregion

            #region Rate Limiting

            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimit"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddHttpContextAccessor();

            #endregion

            #region Logger

            services.ConfigureLoggerService();

            #endregion

            #region General

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling
                                        = ReferenceLoopHandling.Ignore);

            #endregion

            #region Constants

            Constants.EmailAdress = Configuration["Gmail:Address"];
            Constants.EmailPassword = Configuration["Gmail:Password"];
            Constants.ContactEmailFolderPath = Path.Combine(_environment.WebRootPath, "templates"
                , Configuration["Gmail:FileName"]);

            Constants.ClientPort = Configuration["ClientPort:Port"];

            Constants.ApiKey = Configuration["FireBase:ApiKey"];
            Constants.Bucket = Configuration["FireBase:Bucket"];
            Constants.AuthEmail = Configuration["FireBase:AuthEmail"];
            Constants.AuthPassword = Configuration["FireBase:AuthPassword"];

            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "_211_Studios", Version = "v1" });
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIpRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "_211_Studios v1"));
            }
            else
            {
                #region Security Headers

                app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains());
                app.UseXContentTypeOptions();
                app.UseReferrerPolicy(opts => opts.NoReferrer());
                app.UseXXssProtection(options => options.EnabledWithBlockMode());
                app.UseXfo(options => options.Deny());
                app.UseCsp(opts => opts
                    .BlockAllMixedContent()
                    .StyleSources(s => s.Self())
                    .StyleSources(s => s.UnsafeInline())
                    .FontSources(s => s.Self())
                    .FormActions(s => s.Self())
                    .FrameAncestors(s => s.Self())
                    .ImageSources(imageSrc => imageSrc.Self())
                    .ImageSources(imageSrc => imageSrc.CustomSources("data:"))
                    .ScriptSources(s => s.Self())
                );
                app.UseRedirectValidation();
                app.Use(async (context, next) =>
                {
                    if (!context.Response.Headers.ContainsKey("Feature-Policy"))
                    {
                        context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; microphone 'none';");
                    }
                    await next();
                });

                #endregion
            }

            #region Middleware

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseCors("AllowOrigin");

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
