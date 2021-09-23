using System;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests
{
   public class FunctionIntegrationTest
   {
      protected readonly Environments_OpenApiClient _sut = new Environments_OpenApiClient(new HttpClient());
      protected static Guid _environmentKey = new Guid("EBA2CCBB-89D3-45E3-BF90-2DB160BF1552");
      protected static string _businessServiceName = "CommentService";
      protected static string _businessServiceDependencyName = "SqlDatabase";

      static FunctionIntegrationTest()
      {
         // this runs only once

         // TODO: check for functions up and running here?

         // seed data here?
         // is this cheating?
         // TODO: pull from secrets or environment?
         DbContextOptions<EnvironmentContext> contextOptions = new DbContextOptionsBuilder<EnvironmentContext>()
             .UseSqlServer("SECRETS!")
             .Options;
         EnvironmentContext db = new EnvironmentContext(contextOptions);

         bool testEnvironmentExists = db.Environments.Any(e => e.EnvironmentKey == _environmentKey);

         if (!testEnvironmentExists)
         {
            // seed one of each entity, all related
            ResourceType resourceType = new ResourceType { Name = "Azure Sql Database" };
            Resource resource = new Resource
            {
               Uri = "https://myazuresql.database.windows.net/",
               ResourceType = resourceType
            };
            BusinessService businessService = new BusinessService { Name = _businessServiceName };
            BusinessServiceDependency businessServiceDependency = new BusinessServiceDependency
            {
               DependencyName = _businessServiceDependencyName,
               ResourceType = resourceType,
               BusinessService = businessService
            };
            Environment environment = new Environment
            {
               Name = "User 1 Default Environment",
               Description = "Default environment created on user signup",
               EnvironmentKey = Some.Random.Guid(),
               Owner = Some.Random.Guid()
            };
            EnvironmentResourceBinding environmentResourceBinding = new EnvironmentResourceBinding
            {
               BusinessServiceDependency = businessServiceDependency,
               Environment = environment,
               Resource = resource
            };

            db.Add(environmentResourceBinding);
            db.SaveChanges();
         }
      }

      public FunctionIntegrationTest()
      {
         // this runs once per test class
      }
   }
}