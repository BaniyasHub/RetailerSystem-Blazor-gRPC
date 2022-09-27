using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Common.Utility
{
    public static class AutoMapperConfiguration
    {
        public static IEnumerable<Assembly> GetAutoMapperProfilesFromAllAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var retailerAssemblies = assemblies.Where(a => a.FullName.StartsWith("Retailer."));
            return retailerAssemblies;
        }
    }
}
