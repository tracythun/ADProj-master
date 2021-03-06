using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ADProj.DB;
using ADProj.Services;
using Newtonsoft;

namespace ADProj
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
            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddDbContext<ADProjContext>(opt =>
                opt.UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("DbConn")));

            services.AddScoped<EmployeeService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<CollectionPointService>();
            services.AddScoped<InventoryService>();
            services.AddScoped<AdjustmentVoucherValidation>();
            services.AddScoped<RequestServices>();
            services.AddScoped<RequestDetailService>();
            services.AddScoped<Emailservice>();
            services.AddScoped<SupplierService>();
            services.AddScoped<PurchaseOrderServices>();
            services.AddScoped<DisbursementService>();
            services.AddScoped<RetrievalService>();
            services.AddScoped<TrendAnalysisService>();
            services.AddScoped<DisbursementAPIService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ADProjContext dbcontext)
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

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            dbcontext.Database.EnsureDeleted();
            dbcontext.Database.EnsureCreated();

            new DBSeeder(dbcontext);
        }
    }
}
