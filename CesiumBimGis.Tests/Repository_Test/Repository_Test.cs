using Autofac;
using Cesium.IRepository;
using Cesium.Respository;
using CesiumBimGis.Tests.DependencyInjection_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CesiumBimGis.Tests.Repository_Test
{
    public class Repository_Test
    {
        DI_Test dI_Test = new DI_Test();
        private readonly ISysUserRepository _sysUserRepository;

        public Repository_Test()
        {
            var container = dI_Test.DI_Container();
            _sysUserRepository = container.Resolve<SysUserRepository>();
        }

        [Fact]
        public void GetSysUsersTest_ShouldNotNull()
        {
            var data = _sysUserRepository.GetList();
            Assert.NotNull(data);
        }
    }
}
