using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using DotNetAPI.Infrastructure.Database;
using System.Reflection;
using DotNetAPI.Core;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
     
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructureDatabase(_configuration, _environment);

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(assemblies);
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_configuration["Swagger:DocumentVersion"], new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = _environment.ApplicationName,
                    Version = _configuration["Swagger:Version"]
                });

                options.OrderActionsBy(apiDescr => $"{apiDescr.RelativePath}_{apiDescr.HttpMethod}");

                options.CustomSchemaIds(type => type.FullName);
            });
            services.AddMemoryCache();
            services.AddApplication();
            
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
            services.AddAuthentication("Basic");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", builder => {
                    builder
                    .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            if (_environment.IsDevelopment())
            {
                services.AddRazorPages().AddRazorRuntimeCompilation();
            }
            else
            {
                services.AddRazorPages();
            }

            services.AddControllers()

                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddMvcOptions(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        ValidationProblemDetails problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Title = "Validation problem occured",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "See the error property for details",
                            Instance = context.HttpContext.Request.Path
                        };

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICorsService corsService, ICorsPolicyProvider corsPolicyProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint($"../swagger/{_configuration["Swagger:DocumentVersion"]}/swagger.json",
                        $"{_environment.ApplicationName} v{_configuration["Swagger:Version"]}");
                });
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var newContext = serviceScope.ServiceProvider.GetRequiredService<DotNetAPIContext>();
                newContext.Database.Migrate();
            }
            
            app.UseHttpsRedirection();

            app.UseSession();

            app.Use(async (context, next) =>
            {
                if(env.IsDevelopment())
                {
                    
                }

                await next();
            });

            app.UseCors("DefaultPolicy");
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
