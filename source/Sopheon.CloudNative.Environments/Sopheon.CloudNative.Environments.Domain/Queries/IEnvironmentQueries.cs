using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Domain.Queries
{
   public interface IEnvironmentQueries
   {
      /// <summary>
      /// Returns URIs for all resources that have been allocated to environments for the business service dependency
      /// </summary>
      /// <param name="businessServiceName">Business Service name/key, string</param>
      /// <param name="dependencyName">Business Service Dependency name/key, string</param>
      /// <returns></returns>
      Task<IEnumerable<string>> GetResourceUrisByBusinessServiceDependency(string businessServiceName, string dependencyName);
   }
}
