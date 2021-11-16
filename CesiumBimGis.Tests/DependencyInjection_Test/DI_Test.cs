using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cesium.Core;
using Cesium.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CesiumBimGis.Tests.DependencyInjection_Test
{
    public class DI_Test
    {
        [Fact]
        public void SimpleTestDemo()
        {
            //Arrange: 在这里做一些先决的设定。例如创建对象实例，数据，输入等。
            var expected = 5;
            //Act: 在这里执行生产代码并返回结果。例如调用方法或者设置属性。
            var act = 5;
            //Assert:在这里检查结果，会产生测试通过或者失败两种结果
            Assert.True(expected== act);
        }

        /// <summary>
        /// 测试依赖注入容器是否可用
        /// </summary>
        [Fact]
        public void DI_Connect_ShouldAvailibly()
        {
            var basePath = AppContext.BaseDirectory;

            IServiceCollection services = new ServiceCollection();

            //使用autofac容器
            var builder = new ContainerBuilder();

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

            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            var container = builder.Build();

            Assert.True(container.ComponentRegistry.Registrations.Count() > 0);
        }

        /// <summary>
        /// 初始化依赖注入容器
        /// </summary>
        /// <returns></returns>
        public IContainer DI_Container()
        {
            var basePath = AppContext.BaseDirectory;

            var configurationbuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
            IConfigurationRoot configuration = configurationbuilder.Build();


            IServiceCollection services = new ServiceCollection();

            services.Configure<DbOption>("DbOption", configuration.GetSection("DbOption"));

            //使用autofac容器
            var containerbuilder = new ContainerBuilder();

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
            containerbuilder.RegisterAssemblyTypes(assemblyList.ToArray())
             .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
             .AsSelf().AsImplementedInterfaces()
             .InstancePerLifetimeScope();

            //将services填充到Autofac容器生成器中
            containerbuilder.Populate(services);

            var container = containerbuilder.Build();

            return container;
        }
    }
}
