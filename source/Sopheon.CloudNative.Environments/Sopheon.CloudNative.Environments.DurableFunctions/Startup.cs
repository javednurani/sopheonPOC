[assembly: FunctionsStartup(typeof(Sopheon.CloudNative.Environments.DurableFunctions.Startup))]

namespace Sopheon.CloudNative.Environments.DurableFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();            

            ConfigureServices(context, builder.Services);
        }

        private void ConfigureServices(FunctionsHostBuilderContext hostContext, IServiceCollection services)
        {
            string connString = string.Empty;
            if ("production".Equals(hostContext.EnvironmentName, StringComparison.OrdinalIgnoreCase))
            {
                connString = Environment.GetEnvironmentVariable("SQLCONNSTR_EnvironmentsSqlConnectionString");
            }
            else if ("development".Equals(hostContext.EnvironmentName, StringComparison.OrdinalIgnoreCase))
            {
                connString =
                   hostContext.Configuration.GetConnectionString("EnvironmentsSqlConnectionString") ?? // local dev, if stored in local.settings.json under key SQLCONNSTR_EnvironmentsSqlConnectionString
                   hostContext.Configuration["SQLCONNSTR_EnvironmentsSqlConnectionString"] ??          // local dev
                   Environment.GetEnvironmentVariable("SQLCONNSTR_EnvironmentsSqlConnectionString");   // CI pipeline
            }

            // Add Logging
            services.AddLogging();

            services.AddDbContext<EnvironmentContext>(options => options.UseSqlServer(connString));

            services.AddSingleton<Lazy<IAzure>>(sp => new Lazy<IAzure>(GetAzureInstance(hostContext)));   // single instance shared across functions
        }

        private static IAzure GetAzureInstance(FunctionsHostBuilderContext hostContext)
        {
            AzureCredentials credentials;
            if ("Production".Equals(hostContext.EnvironmentName, StringComparison.OrdinalIgnoreCase))
            {
                string tenantId = Environment.GetEnvironmentVariable("AzSpTenantId"); 
                credentials = SdkContext.AzureCredentialsFactory
                   .FromSystemAssignedManagedServiceIdentity(MSIResourceType.AppService, AzureEnvironment.AzureGlobalCloud, tenantId);
            }
            else
            {
                // authenticate with Service Principal credentials
                string tenantId = hostContext.Configuration["AzSpTenantId"];
                string clientId = hostContext.Configuration["AzSpClientId"]; 
                string clientSecret = hostContext.Configuration["AzSpClientSecret"];
                credentials = SdkContext.AzureCredentialsFactory
                   .FromServicePrincipal(clientId, clientSecret, tenantId, environment: AzureEnvironment.AzureGlobalCloud);
            }

            return Microsoft.Azure.Management.Fluent.Azure
               .Authenticate(credentials)
               .WithDefaultSubscription();
        }
    }
}
