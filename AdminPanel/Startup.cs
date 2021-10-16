using Business.Abstract;
using Common;
using DataAccess.Abstract;
using DataAccess.AutoMapper;
using DataAccess.Concret;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace AdminPanel
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
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

            #endregion

            #region Scoped

            services.RegisterAllTypes(ServiceLifetime.Scoped, typeof(ISliderDal));
            services.RegisterAllTypes(ServiceLifetime.Scoped, typeof(ISliderService));

            #endregion

            #region AutoMapper

            services.AddAutoMapper(typeof(AutoMapperProfile));

            #endregion

            #region General

            services.AddControllersWithViews();

            #endregion

            #region Constants

            Constants.ImageFolderPath = Path.Combine(_environment.WebRootPath, "images");

            Constants.VideoFolderPath = Path.Combine(_environment.WebRootPath, "videos");

            Constants.FrontImageFolderPath = Configuration["FrontFolderPath:ImageFolderPath"];

            Constants.FrontVideoFolderPath = Configuration["FrontFolderPath:VideoFolderPath"]; ;

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
