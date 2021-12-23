using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Domain.Queries
{
   public interface IEnvironmentQueries
   {
      /// <summary>
      /// Returns URIs for all resources that have been allocated to environments for the business service dependency
      /// </summary>
      /// <param name="businessServiceName">Business Service name, string</param>
      /// <param name="dependencyName">Business Service Dependency name, string</param>
      /// <returns>URIs, IEnumerable of string</returns>
      Task<IEnumerable<string>> GetResourceUrisByBusinessServiceDependency(string businessServiceName, string dependencyName);

      /// <summary>
      /// Returns URI for a specific resources that have been allocated to a specified environment
      /// </summary>
      /// <param name="environmentKey">Environment key</param>
      /// <param name="businessServiceName">BusinessService name</param>
      /// <param name="dependencyName">BusinessServiceDependency name</param>
      /// <returns>URI of a resource that matches the keys/names provided</returns>
      Task<string> GetSpecificResourceUri(Guid environmentKey, string businessServiceName, string dependencyName);

      /// <summary>
      /// Returns Resource ID for a resource that matches the requested type and unassigned
      /// </summary>
      /// <param name="resourceTypes">Requested resource type</param>
      /// <returns>Id of the Resource</returns>
      Task<Resource> GetUnassignedResource(Enums.ResourceTypes resourceTypes);
   }
}
