﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Formatting.Compact;
using MIT.ECSR.Core;
using MIT.ECSR.Data;
using System.Reflection;
using WonderKid.DAL;
using WonderKid.DAL.Interface;
using MIT.ECSR.Shared;
using MIT.ECSR.API.Handler;
using Microsoft.Extensions.FileProviders;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MIT.ECSR.API
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        private IConfiguration _configuration { get; }
        private IWebHostEnvironment _environment { get; set; }
        public Startup(IWebHostEnvironment environment)
        {
            _environment = environment;
            string _environtmentName = "Development";
            if (environment.IsDevelopment())
                _environtmentName = "Development";
            else if (environment.IsStaging())
                _environtmentName = "Staging";
            else if (environment.IsProduction())
                _environtmentName = "Production";

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                      .AddJsonFile("appsettings.json")
                                                      .AddJsonFile($"appsettings.{_environtmentName}.json", optional: true)
                                                      .AddEnvironmentVariables()
                                                      .Build();


            _configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .Filter.ByExcluding((le) => le.Level == Serilog.Events.LogEventLevel.Information)
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    new CompactJsonFormatter(),
                    path: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "Application_.json"),
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                    fileSizeLimitBytes: 524288000,
                    rollingInterval: RollingInterval.Day
                )
                .WriteTo.Seq(_configuration["ApplicationConfig:Seq"])
                .CreateLogger();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.RegisterShared(_configuration);
            services.RegisterData(_configuration);
            services.RegisterCore(_configuration);

            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
            });
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("ApplicationConfig")["SecretKey"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidIssuer = _configuration.GetSection("ApplicationConfig")["Issuer"],
                   ValidAudience = _configuration.GetSection("ApplicationConfig")["Audience"],
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.Zero
               };
               x.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                       {
                           context.Response.Headers.Add("Token-Expired", "true");
                       }
                       return Task.CompletedTask;
                   }
               };
           });
            services.AddAuthorization(options =>
            {

            });

            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(AuthorizeFilter));
            });
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddHealthChecks();

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(_configuration.GetSection("HangFire")["Connection"], new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            if (_environment.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MIT.ECSR.API", Version = "v1" });
                    c.AddSecurityDefinition("Bearer",
                        new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            BearerFormat = "JWT",
                            Scheme = "Bearer",
                            In = ParameterLocation.Header,
                            Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
                            Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')"
                        }
                    );
                    c.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                },
                                Array.Empty<string>()
                            }
                        });
                    c.EnableAnnotations();
                });
                services.AddSwaggerGen();
            }

            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            _configuration.GetValue<string>("AllowedHosts").Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MIT E-CSR API v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Media")),
                RequestPath = new PathString("/Media")
            });

            app.UseHangfireDashboard("/" + _configuration.GetSection("HangFire")["URL"], new DashboardOptions
            {
                DashboardTitle = "MIT E-CSR JOB",
                Authorization = new[] {
                    new HangfireCustomBasicAuthenticationFilter{
                    User = _configuration.GetSection("HangFire")["UserName"],
                    Pass = _configuration.GetSection("HangFire")["Password"]
                }
                },
                IgnoreAntiforgeryToken = true
            });
            app.UseCookiePolicy();
            app.UseHealthChecks("/health");
            app.UseCors(_defaultCorsPolicyName);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
