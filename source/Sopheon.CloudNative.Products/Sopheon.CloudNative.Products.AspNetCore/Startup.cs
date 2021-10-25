using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Sopheon.CloudNative.Products.AspNetCore.HealthCheck;
using Sopheon.CloudNative.Products.AspNetCore.Policies;
using Sopheon.CloudNative.Products.AspNetCore.Policies.Handlers;
using Sopheon.CloudNative.Products.AspNetCore.Policies.Requirements;
using Sopheon.CloudNative.Products.Domain;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore
{
   public class Startup
   {
      /// <summary>
      /// The OpenID Connect standard specifies several special scope values. The following scopes represent the permission to access the user's profile:
      /// openid - Requests an ID token.
      /// offline_access - Requests a refresh token using Auth Code flows.
      /// 00000000-0000-0000-0000-000000000000 - Using the client ID as the scope indicates that your app needs an access token that can be used against your own service or web API, represented by the same client ID.
      /// For more information, see https://docs.microsoft.com/en-us/azure/active-directory-b2c/access-tokens
      /// </summary>
      private Dictionary<string, string> _scopes = new Dictionary<string, string>
      {
         { "openid", "openid"},
         { "offline_access", "Refresh Token"},
         { "profile", "profile"} 
      };

      public Startup(IConfiguration configuration,
         IWebHostEnvironment env)
      {
         Configuration = configuration;
         _env = env;

         _scopes.Add(Configuration.GetValue<string>("AzureAdB2C:ClientId"), "Access the api as the signed-in user");
      }

      public IConfiguration Configuration { get; }
      private IWebHostEnvironment _env;

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         //services.Configure<CookiePolicyOptions>(options =>
         //{
         //   // This lambda determines whether user consent for non-essential cookies is needed for a given request.
         //   options.CheckConsentNeeded = context => true;
         //   options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
         //   // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
         //   options.HandleSameSiteCookieCompatibility();
         //});

         // Configuration to sign-in users with Azure AD B2C
         services
            .AddMicrosoftIdentityWebApiAuthentication(Configuration, Constants.AzureAdB2C);

         services
            .AddMemoryCache()
            .AddHttpClient();

         services
            .AddHealthChecks()//
            .AddCheck<EnvironmentCatalogHealthCheck>(nameof(EnvironmentCatalogHealthCheck), tags: new[] { "tenant-directory" })
            .AddCheck<EnvironmentDatabaseHealthCheck>(nameof(EnvironmentDatabaseHealthCheck), tags: new[] { "environment-specific" });
            //.AddCheck("Example", () =>
            //   HealthCheckResult.Healthy("Example is OK!"), tags: new[] { "example" })
            //.AddCheck("Examples", () =>
            //   HealthCheckResult.Healthy("Example is OKs!"), tags: new[] { "examples" });

         services.AddAuthorization(options =>
         {
            options.AddPolicy(nameof(HasEnvironmentAccessPolicy), policy => policy.Requirements.Add(new HasRelevantRelationshipToEnvironment()));

            // By default, all incoming requests will be authorized according to the default policy
            options.FallbackPolicy = options.DefaultPolicy;
         });

         services.AddAutoMapper(typeof(Startup));

         services.AddControllers();

         services.AddSwaggerGen(c =>
         {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sopheon.CloudNative.Products.AspNetCore", Version = "v1" });

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
               Name = "Authorization",
               //Type = SecuritySchemeType.Http, // Must manually enter bearer token
               Type = SecuritySchemeType.OAuth2,
               //Type = SecuritySchemeType.OpenIdConnect,
               //OpenIdConnectUrl = new Uri($"{Configuration.GetValue<string>("AzureAdB2C:Instance")}/{Configuration.GetValue<string>("AzureAdB2C:Domain")}/v2.0/.well-known/openid-configuration?p={Configuration.GetValue<string>("AzureAdB2C:SignUpSignInPolicyId")}"),
               Scheme = "bearer",
               BearerFormat = "JWT",
               In = ParameterLocation.Header,
               Description = "JWT Authorization header using the Bearer scheme.",
               Flows = new OpenApiOAuthFlows
               {
                  AuthorizationCode = new OpenApiOAuthFlow
                  {
                     AuthorizationUrl = new Uri($"{Configuration.GetValue<string>("AzureAdB2C:Instance")}/{Configuration.GetValue<string>("AzureAdB2C:Domain")}/{Configuration.GetValue<string>("AzureAdB2C:SignUpSignInPolicyId")}/oauth2/v2.0/authorize"), // ex: https://<b2c_tenant_name>.b2clogin.com/<b2c_tenant_name>.onmicrosoft.com/oauth2/v2.0/authorize?p=b2c_1_susi_v2
                     TokenUrl = new Uri($"{Configuration.GetValue<string>("AzureAdB2C:Instance")}/{Configuration.GetValue<string>("AzureAdB2C:Domain")}/{Configuration.GetValue<string>("AzureAdB2C:SignUpSignInPolicyId")}/oauth2/v2.0/token"), // ex: https://<b2c_tenant_name>.b2clogin.com/<b2c_tenant_name>.onmicrosoft.com/oauth2/v2.0/token?p=b2c_1_susi_v2
                     Scopes = _scopes
                  }
               }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
               {
                  new OpenApiSecurityScheme
                  {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } //
                  },
                  _scopes.Keys.ToArray()
               }
            });
         });

         //Configuring appsettings section AzureAdB2C, into IOptions
         services.AddOptions();
         services.Configure<OpenIdConnectOptions>(Configuration.GetSection("AzureAdB2C"));

         // Dependency Injection Registration
         services.AddScoped<IEnvironmentIdentificationService, EnvironmentIdentificationService>();

         // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-5.0#determining-the-environment-at-runtime
         if (!_env.IsDevelopment() || !Configuration.GetValue<bool>("LocalDevelopment:UseEnvironmentDatabasesFromAppSettings"))
         {
            services.AddScoped<IEnvironmentSqlConnectionStringProvider, EnvironmentSqlConnectionStringProvider>();
         }
         else
         {
            services.AddScoped<IEnvironmentSqlConnectionStringProvider, DevelopmentTimeEnvironmentSqlConnectionStringProvider>();
         }

         services.AddScoped<IAuthorizationHandler, EnvironmentOwnerHandler>();
         //services.AddScoped<IAuthorizationHandler, SopheonSupportEnvironmentAccessHandler>(); // TODO: Add handling for support access scenario

         // Entity Framework
         services.AddDbContext<ProductManagementContext>((serviceProvider, optionsBuilder) =>
         {
            // WARNING: As of EF 5, AddDbContext does not support an aysnc delegate
            var connectionStringProvider = serviceProvider.GetService<IEnvironmentSqlConnectionStringProvider>();
            string connectionString = connectionStringProvider.GetConnectionStringAsync().Result; // TODO: Need to find async registration method
            optionsBuilder.UseSqlServer(connectionString);
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "NextGen ProductManagement Api v1");

               c.OAuthClientId(Configuration.GetValue<string>("AzureAdB2C:ClientId"));
               //c.OAuthClientSecret(Configuration.GetValue<string>("AzureAdB2C:ClientSecret"));
               c.OAuthAppName("SwaggerUI");
               c.OAuthScopeSeparator(" ");
               c.OAuthUsePkce();
            });
         }

         app.UseHttpsRedirection();

         app.UseRouting();

         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
               Predicate = (check) => !check.Tags.Contains("environment-specific"),
               ResponseWriter = WriteHealthResponse
            }).AllowAnonymous();

            endpoints.MapHealthChecks("/environments/{environmentid}/health", new HealthCheckOptions
            {
               Predicate = (check) => check.Tags.Contains("environment-specific"),
               ResponseWriter = WriteHealthResponse
            }).AllowAnonymous(); 

            endpoints.MapControllers();
         });
      }

      private static async Task WriteHealthResponse(HttpContext context, HealthReport report)
      {
         context.Response.ContentType = "application/json";
         var response = new HealthCheckReponse
         {
            Status = report.Status.ToString(),
            HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
            {
               Component = x.Key,
               Status = x.Value.Status.ToString(),
               Description = x.Value.Description
            }),
            HealthCheckDuration = report.TotalDuration
         };
         await context.Response.WriteAsJsonAsync(response);
      }
   }
}
