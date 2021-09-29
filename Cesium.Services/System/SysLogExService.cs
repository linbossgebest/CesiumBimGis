using Cesium.Core.Extensions;
using Cesium.IRepository.System;
using Cesium.IServices.System;
using Cesium.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Services.System
{
    public class SysLogExService : ISysLogExService, IDependency
    {
        private readonly ISysLogExRepository _sysLogExRepository;

        public SysLogExService(ISysLogExRepository sysLogExRepository)
        {
            _sysLogExRepository = sysLogExRepository;
        }

        public async Task<bool> AddLog(SysLogEx model)
        {
            if (await _sysLogExRepository.InsertAsync(model) > 0)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<SysLogEx>> GetLogs(string className, string methodName)
        {
            string conditions = " where 1=1 ";
            if (!className.IsNullOrWhiteSpace())
            {
                conditions += "And ClassName = @className";
            }
            if (!methodName.IsNullOrWhiteSpace())
            {
                conditions += "And MethodName = @methodName";
            }

            var logs = await _sysLogExRepository.GetListAsync(conditions, new { ClassName = className, MethodName = methodName });

            return logs;
        }
    }
}
