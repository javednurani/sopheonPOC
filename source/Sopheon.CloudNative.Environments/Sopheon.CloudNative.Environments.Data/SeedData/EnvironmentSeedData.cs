using Sopheon.CloudNative.Environments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopheon.CloudNative.Environments.Data.SeedData
{
   public static class EnvironmentSeedData
   {
      public static readonly DomainResourceType[] _domainResourceTypes;
      public static DomainResourceType[] DomainResourceTypes => _domainResourceTypes.ToArray();

      public static readonly Dictionary<string, BusinessService> _businessServicesByName;
      public static BusinessService[] BusinessServices => _businessServicesByName.Values.ToArray();

      public static readonly BusinessServiceDependency[] _businessServiceDependencies;
      public static BusinessServiceDependency[] BusinessServiceDependencies => _businessServiceDependencies.ToArray();

      static EnvironmentSeedData()
      {
         _domainResourceTypes = new[]
         {
            new DomainResourceType()
            {
               Id = (int)DomainResourceTypeIds.SqlDatabase,
               Name = nameof(DomainResourceTypeIds.SqlDatabase)
            },
            new DomainResourceType()
            {
               Id = (int)DomainResourceTypeIds.AzureBlobStorageContainer,
               Name = nameof(DomainResourceTypeIds.AzureBlobStorageContainer)
            }
         };

         _businessServicesByName = new[]
         {
            new BusinessService()
            {
               Id = (int)BusinessServiceIds.ProductManagement,
               Name = nameof(BusinessServiceIds.ProductManagement)
            }
         }.ToDictionary(businessService => businessService.Name, StringComparer.OrdinalIgnoreCase);

         _businessServiceDependencies = new[]
         {
            new BusinessServiceDependency()
            {
               Id = (int)BusinessServiceDependencyIds.ProductManagement_SqlDatabase,
               BusinessServiceId = (int)BusinessServiceIds.ProductManagement,
               DependencyName = "SqlDatabase",
               DomainResourceTypeId = (int)DomainResourceTypeIds.SqlDatabase
            }
         };
      }
   }
}
