using Autofac;
using Cesium.IServices;
using Cesium.Services;
using Cesium.ViewModels.System;
using CesiumBimGis.Tests.DependencyInjection_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CesiumBimGis.Tests.Service_Test
{
    public class Service_Test
    {
        DI_Test dI_Test = new DI_Test();
        private readonly ISysUserService _sysUserService;

        public Service_Test()
        {
            var container = dI_Test.DI_Container();
            _sysUserService = container.Resolve<SysUserService>();
        }

        [Theory]
        [InlineData("linyong","1234567")]
        [InlineData("linyong", "123")]
        public async  void SignInAsync_ShoudHasUser(string username,string password)
        {
            var model = new LoginModel() { username=username,password=password};
            var user=await _sysUserService.SignInAsync(model);
            Assert.NotNull(user);
        }
    }
}
