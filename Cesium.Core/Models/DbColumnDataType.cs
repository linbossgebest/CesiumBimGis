using Cesium.Models;

namespace Cesium.Core.Models
{
    /// <summary>
    /// 数据库列的属性
    /// </summary>
    public class DbColumnDataType
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 数据库中对应的类型
        /// </summary>
        public string ColumnTypes { get; set; }
        /// <summary>
        /// C#中对应的类型
        /// </summary>
        public string CSharpType { get; set; }
    }
}
