﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using SSO.Demo.Toolkits.Model;

namespace SSO.Demo.Toolkits.Extension
{
    public static class DependenceExtension
    {
        public static List<Type> GetTypeOfISerice(this AssemblyName assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName).ExportedTypes.Where(b => b.GetInterfaces().Contains(typeof(IService))).ToList();
        }

        public static void AddService(this IServiceCollection services)
        {
            var defaultAssemblyNames = DependencyContext.Default.GetDefaultAssemblyNames().Where(a => a.FullName.Contains("SSO.Demo")).ToList();

            var assemblies = defaultAssemblyNames.SelectMany(a => a.GetTypeOfISerice()).ToList();

            assemblies.ForEach(assembliy =>
            {
                services.AddScoped(assembliy);
            });
        }
    }
}
