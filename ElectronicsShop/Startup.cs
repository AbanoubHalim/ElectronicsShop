using ElectronicsShop.Models;
using ElectronicsShop.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop
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
            services.AddDbContext<ShopContext>(options => options
              .UseLazyLoadingProxies()
              .UseSqlServer(Configuration.GetConnectionString("SystemConnection")
          ));
            services.AddControllers();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ISampleData, SampleData>();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ShopContext>();
            services.AddCors();
            services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
                .ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ElectronicsShop", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)//,ISampleData seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElectronicsShop v1"));
            }

            //await seeder.CreateRole();
           // await seeder.SeedAdminUser();
            
            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        Secure = CookieSecurePolicy.Always
    });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
