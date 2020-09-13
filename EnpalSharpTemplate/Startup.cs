using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EnpalSharpTemplate.Helper;
using EnpalSharpTemplate.Model;
using EnpalSharpTemplate.ServiceInterface;
using EnpalSharpTemplate.Services;
using EnpalSharpTemplate.Utils.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.SqlServer.Management.Smo;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EnpalSharpTemplate
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Hold the configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            
            services.AddCors();
            services.AddControllers();

            services.AddApiVersioning(config =>
            {
                // Specify the default API Version
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;

            });

            //var identityServerConfig = new IdentityServerConfig();
            //Configuration.GetSection("IdentityServer").Bind(identityServerConfig);

            //Uncomment for Basic Auth
            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            //Uncomment for Basic Auth
            // configure DI for application services, only needed for basic auth
            services.AddScoped<IUserService, UserService>();

            // authentication via Identity Server for enpal
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", options =>
            //    {
            //        options.Authority = identityServerConfig.BaseUrl;
            //        options.RequireHttpsMetadata = true;

            //        options.Audience = identityServerConfig.Audience;
            //    });


            //Uncomment this to add more scopes to your API which protect for example the creation
            //Then this can be used for each REST API like [Authorize("Creator")]
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Creator", policyAdmin =>
            //    {
            //        policyAdmin.RequireClaim("scope", identityServerConfig.CreationScope);
            //    });
            //});

            #region DI of Database and Services
            services.AddDbContext<HistoryContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IHistoryRegistration, RegistrationHistoryService>();
            services.AddScoped<IHistorySearch, SearchHistoryService>();
            services.AddScoped<ISampleService, SampleService>();
            #endregion

            services.AddHeaderPropagation(options =>
            {
                // forward the 'x-test-features' if present.
                options.Headers.Add("X-TrackingId");
                options.Headers.Add("X-TrackingId", context => new StringValues(Guid.NewGuid().ToString("D")));
            });


            var version = this.GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "EnpalSharpTemplate", Version = "v1.0", Description = $"Code Version : {version}" });
                c.SwaggerDoc("v1.1", new OpenApiInfo { Title = "EnpalSharpTemplate", Version = "v1.1", Description = $"Code Version : {version}" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.OperationFilter<RemoveVersionParameterFilter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                c.EnableAnnotations();
                c.OperationFilter<AddRequiredHeaderParameter>();


                //Uncomment for Basic Auth
                c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Description = "Basic Authentication",
                    Name = "Authorization",
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });

                //Uncomment for Basic Auth
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Basic"
                            }
                        },
                        new string[] { }
                    }
                });

                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme.",
                //    Name = "Authorization",
                //    Scheme = "bearer",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.Http,
                //    BearerFormat = "JWT"
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        new string[] { }
                //    }
                //});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseHeaderPropagation();
            app.UseRouting();


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "EnpalSharpTemplate  v1.0");
                c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "EnpalSharpTemplate  v1.1");
            });
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<HistoryContext>();
                context.Database.Migrate();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();


            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-TrackingId",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    Default = new OpenApiString(Guid.NewGuid().ToString("D"))
                }
            });

        }

    }

    public class SwaggerAuthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerAuthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger")
                && !context.User.Identity.IsAuthenticated)
            {
                await context.ChallengeAsync();
                return;
            }

            await _next.Invoke(context);
        }
    }

    public static class ActionDescriptorExtensions
    {
        public static ApiVersionModel GetApiVersion(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor?.Properties
                .Where((kvp) => ((Type)kvp.Key) == typeof(ApiVersionModel))
                .Select(kvp => kvp.Value as ApiVersionModel).FirstOrDefault();
        }
    }
}
