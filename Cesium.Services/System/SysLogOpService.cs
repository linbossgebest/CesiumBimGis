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
    public class SysLogOpService : ISysLogOpService, IDependency
    {
        private readonly ISysLogOpRepository _sysLogOpRepository;

        public SysLogOpService(ISysLogOpRepository sysLogOpRepository)
        {
            _sysLogOpRepository = sysLogOpRepository;
        }

        public async Task<bool> AddLog(SysLogOp model)
        {
            if (await _sysLogOpRepository.InsertAsync(model) > 0)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<SysLogOp>> GetLogs(string logName, string requestMethod)
        {
            string conditions = " where 1=1 ";
            if (!logName.IsNullOrWhiteSpace())
            {
                conditions += "And Name LIKE CONCAT('%',@logName,'%') ";
            }
            if (!requestMethod.IsNullOrWhiteSpace())
            {
                conditions += "And ReqMethod LIKE CONCAT('%',@requestMethod,'%') ";
            }

            var logs = await _sysLogOpRepository.GetListAsync(conditions, new { Name = logName, ReqMethod = requestMethod });

            return logs;
        }
    }
}
