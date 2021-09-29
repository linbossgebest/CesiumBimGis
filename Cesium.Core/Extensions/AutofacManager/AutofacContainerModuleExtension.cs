using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.Extensions
{
    public static class AutofacContainerModuleExtension
    {
        public static IServiceCollection AddModule(this IServiceCollection services, ContainerBuilder builder, IConfiguration configuration)
        {
            Type baseType = typeof(IDependency);
            //获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)
            var compilationLibrary = DependencyContext.Default
                       .CompileLibraries
                       .Where(x => !x.Serviceable
                       && x.Type == "project")
                       .ToList();

            var count = compilationLibrary.Count;
            List<Assembly> assemblyList = new List<Assembly>();

            foreach (var _compilation in compilationLibrary)
            {
                try
                {
                    assemblyList.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(_compilation.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_compilation.Name + ex.Message);
                }
            }
            builder.RegisterAssemblyTypes(assemblyList.ToArray())
             .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
             .AsSelf().AsImplementedInterfaces()
             .InstancePerLifetimeScope();

            return services;
        }
    }
}
