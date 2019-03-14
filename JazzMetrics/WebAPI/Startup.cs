using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Middleware;
using WebAPI.Services.AffectedFields;
using WebAPI.Services.AspiceProcesses;
using WebAPI.Services.AspiceVersions;
using WebAPI.Services.Companies;
using WebAPI.Services.Email;
using WebAPI.Services.Helper;
using WebAPI.Services.Language;
using WebAPI.Services.Log;
using WebAPI.Services.Metrics;
using WebAPI.Services.MetricTypes;
using WebAPI.Services.ProjectMetrics;
using WebAPI.Services.Projects;
using WebAPI.Services.ProjectUsers;
using WebAPI.Services.Setting;
using WebAPI.Services.Test;
using WebAPI.Services.Users;

namespace WebAPI
{
    public class Startup
    {
        /// <summary>
        /// nazev connectionstringu databaze v appsettings.json
        /// </summary>
        public static string ConnectionStringName = "JazzMetricsDatabase";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddHttpContextAccessor(); //pristup k identite v kontruktoru pomoci IHttpContextAccessor 

            services.AddDbContext<JazzMetricsContext>(options => options.UseSqlServer(Configuration.GetConnectionString(ConnectionStringName)));

            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IHelperService, HelperService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMetricService, MetricService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IMetricTypeService, MetricTypeService>();
            services.AddScoped<IProjectUserService, ProjectUserService>();
            services.AddScoped<IAspiceVersionService, AspiceVersionService>(); 
            services.AddScoped<IAspiceProcessService, AspiceProcessService>();
            services.AddScoped<IProjectMetricService, ProjectMetricService>();
            services.AddScoped<IAffectedFieldService, AffectedFieldService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            app.UseHsts();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //}

            app.UseAuthentication();
            //app.UseHttpsRedirection(); //TODO https a origins
            app.UseCors(options => options
                        //.WithOrigins("http://localhost:60001", "http://localhost:20260", "http://localhost:58771")
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            app.UseMvc();
        }
    }
}
