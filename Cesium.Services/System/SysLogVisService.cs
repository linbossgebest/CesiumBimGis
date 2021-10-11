using Cesium.Core.CustomEnum;
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
    public class SysLogVisService : ISysLogVisService, IDependency
    {
        private readonly ISysLogVisRepository _sysLogVisRepository;

        public SysLogVisService(ISysLogVisRepository sysLogVisRepository)
        {
            _sysLogVisRepository = sysLogVisRepository;
        }

        public async Task<bool> AddLog(SysLogVis model)
        {
            if (await _sysLogVisRepository.InsertAsync(model) > 0)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<SysLogVis>> GetLogs(string logName, int? visType)
        {
            string conditions = " where 1=1 ";
            if (!logName.IsNullOrWhiteSpace())
            {
                conditions += "And Name LIKE CONCAT('%',@logName,'%') ";
            }
            if (visType.HasValue)
            {
                visType = visType.Value;
                conditions += "And VisType = @visType ";
            }
            else {
                visType = null;
            }

            var logs = await _sysLogVisRepository.GetListAsync(conditions, new { logName = logName, visType = visType });

            return logs;
        }
    }
}
