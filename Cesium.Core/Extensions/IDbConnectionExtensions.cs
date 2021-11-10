using Cesium.Core.Models;
using Cesium.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;

namespace Cesium.Core.Extensions
{
    public static class IDbConnectionExtensions
    {
        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <param name="dbConnection">数据库连接</param>
        /// <param name="tableName">表名</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static DbTable GetCurrentDbTable(this IDbConnection dbConnection, string tableName, DatabaseType dbType)
        {
            DbTable tb = new DbTable { TableName = tableName, TableComment = tableName };
            tb.Columns= dbConnection.GetColumnsByTableName(dbType, tableName);
            tb.Columns.ForEach(x =>
            {
                var csharpType = DbColumnTypeCollection.DbColumnDataTypes.FirstOrDefault(t =>
                    t.DatabaseType == dbType && t.ColumnTypes.Split(',').Any(p =>
                        p.Trim().Equals(x.ColumnType, StringComparison.OrdinalIgnoreCase)))?.CSharpType;
                if (string.IsNullOrEmpty(csharpType))
                {
                    throw new SqlTypeException($"未从字典中找到\"{x.ColumnType}\"对应的C#数据类型，请更新DbColumnTypeCollection类型映射字典。");
                }

                x.CSharpType = csharpType;
            });
            return tb;
        }

        /// <summary>
        /// 根据表名，获取表所有的列
        /// </summary>
        /// <param name="dbConnection">数据库连接</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="tableName">数据库表名</param>
        /// <returns></returns>
        private static List<DbTableColumn> GetColumnsByTableName(this IDbConnection dbConnection, DatabaseType dbType, string tableName)
        {
            if (dbConnection == null) throw new ArgumentNullException(nameof(dbConnection));
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            return dbConnection.Query<DbTableColumn>(dbConnection.strGetAllColumnsSql(dbType, tableName)).ToList();
        }

        /// <summary>
        /// 获取所有列
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbType"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static string strGetAllColumnsSql(this IDbConnection dbConnection, DatabaseType dbType, string tableName)
        {
            var strGetTableColumns = string.Empty;
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    strGetTableColumns = $@"SELECT   a.name AS ColName, CONVERT(bit, (CASE WHEN COLUMNPROPERTY(a.id, a.name, 'IsIdentity') 
                = 1 THEN 1 ELSE 0 END)) AS IsIdentity, CONVERT(bit, (CASE WHEN
                    (SELECT   COUNT(*)
                     FROM      sysobjects
                     WHERE   (name IN
                                         (SELECT   name
                                          FROM      sysindexes
                                          WHERE   (id = a.id) AND (indid IN
                                                              (SELECT   indid
                                                               FROM      sysindexkeys
                                                               WHERE   (id = a.id) AND (colid IN
                                                                                   (SELECT   colid
                                                                                    FROM      syscolumns
                                                                                    WHERE   (id = a.id) AND (name = a.name))))))) AND (xtype = 'PK')) 
                > 0 THEN 1 ELSE 0 END)) AS IsPrimaryKey, b.name AS ColumnType, COLUMNPROPERTY(a.id, a.name, 'PRECISION') 
                AS ColumnLength, CONVERT(bit, (CASE WHEN a.isnullable = 1 THEN 1 ELSE 0 END)) AS IsNullable, ISNULL(e.text, '') 
                AS DefaultValue, ISNULL(g.value, ' ') AS Comment
FROM      sys.syscolumns AS a LEFT OUTER JOIN
                sys.systypes AS b ON a.xtype = b.xusertype INNER JOIN
                sys.sysobjects AS d ON a.id = d.id AND d.xtype = 'U' AND d.name <> 'dtproperties' LEFT OUTER JOIN
                sys.syscomments AS e ON a.cdefault = e.id LEFT OUTER JOIN
                sys.extended_properties AS g ON a.id = g.major_id AND a.colid = g.minor_id LEFT OUTER JOIN
                sys.extended_properties AS f ON d.id = f.class AND f.minor_id = 0
WHERE   (b.name IS NOT NULL) AND (d.name = '{tableName}')
ORDER BY a.id, a.colorder";
                    break;
                case DatabaseType.MySQL:
                    strGetTableColumns =
                   "select column_name as ColName, " +
                   " column_default as DefaultValue," +
                   " IF(extra = 'auto_increment','TRUE','FALSE') as IsIdentity," +
                   " IF(is_nullable = 'YES','TRUE','FALSE') as IsNullable," +
                   " DATA_TYPE as ColumnType," +
                   " CHARACTER_MAXIMUM_LENGTH as ColumnLength," +
                   " IF(COLUMN_KEY = 'PRI','TRUE','FALSE') as IsPrimaryKey," +
                   " COLUMN_COMMENT as Comment " +
                   $" from information_schema.columns where table_schema = '{dbConnection.Database}' and table_name = '{tableName}'";
                    break;
                case DatabaseType.PostgreSQL:
                    strGetTableColumns =
                   "select column_name as ColName," +
                   "data_type as ColumnType," +
                   "coalesce(character_maximum_length, numeric_precision, -1) as ColumnLength," +
                   "CAST((case is_nullable when 'NO' then 0 else 1 end) as bool) as IsNullable," +
                   "column_default as DefaultValue," +
                   "CAST((case when position('nextval' in column_default)> 0 then 1 else 0 end) as bool) as IsIdentity, " +
                   "CAST((case when b.pk_name is null then 0 else 1 end) as bool) as IsPrimaryKey," +
                   "c.DeText as Comment" +
                   " from information_schema.columns" +
                   " left join " +
                   " (select pg_attr.attname as colname,pg_constraint.conname as pk_name from pg_constraint " +
                   " inner join pg_class on pg_constraint.conrelid = pg_class.oid" +
                   " inner join pg_attribute pg_attr on pg_attr.attrelid = pg_class.oid and  pg_attr.attnum = pg_constraint.conkey[1]" +
                   $" inner join pg_type on pg_type.oid = pg_attr.atttypid where pg_class.relname = '{tableName}' and pg_constraint.contype = 'p') b on b.colname = information_schema.columns.column_name " +
                   " left join " +
                   " (select attname, description as DeText from pg_class " +
                   " left join pg_attribute pg_attr on pg_attr.attrelid = pg_class.oid" +
                   " left join pg_description pg_desc on pg_desc.objoid = pg_attr.attrelid and pg_desc.objsubid = pg_attr.attnum " +
                   $" where pg_attr.attnum > 0 and pg_attr.attrelid = pg_class.oid and pg_class.relname = '{tableName}') c on c.attname = information_schema.columns.column_name" +
                   $" where table_schema = 'public' and table_name = '{tableName}' order by ordinal_position asc";
                    break;
                default:
                    throw new ArgumentNullException($"还不支持的{dbType.ToString()}数据库类型");

            }
            return strGetTableColumns;
        }
    }
}
