using Cesium.Core.CustomEnum;
using Cesium.Core.DbHelper;
using Cesium.Core.Extensions;
using Cesium.Core.Models;
using Cesium.Core.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.CodeGenerator
{
    public class CodeGenerator
    {
        private static CodeGenerateOption _options;
        private readonly string Delimiter = "\\";//分隔符，默认为windows下的\\分隔符

        public CodeGenerator(IOptions<CodeGenerateOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            _options = options.Value;
            if (_options.ConnectionString.IsNullOrWhiteSpace())
                throw new ArgumentNullException("缺少数据库连接串");
            if (_options.DbType.IsNullOrWhiteSpace())
                throw new ArgumentNullException("缺少数据库类型");
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (_options.OutputPath.IsNullOrWhiteSpace())
                throw new ArgumentNullException("文件输出路径不能为空");
            var flag = path.IndexOf("/bin");//判断环境为linux系统
            if (flag > 0)
                Delimiter = "/";//如果可以取到值，修改分割符
        }

        /// <summary>
        /// 根据数据库连接字符串和表名生成对应的模板代码
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="isCoveredExsited">是否覆盖已存在的同名文件</param>
        public void GenerateTemplateCodesFromDb(string tableName, bool isCoveredExsited = true)
        {
            var table = new DbTable();
            DatabaseType dbType = ConnectionFactory.GetDataBaseType(_options.DbType);
            using (var dbConnection = ConnectionFactory.CreateConnection(dbType, _options.ConnectionString))
            {
                table = dbConnection.GetCurrentDbTable(tableName, dbType);
            }
            if (table != null)
            {
                GenerateEntity(table, isCoveredExsited);
                if (table.Columns.Any(c => c.IsPrimaryKey))
                {
                    var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                    GenerateIRepository(table, pkTypeName, isCoveredExsited);
                    GenerateRepository(table, pkTypeName, isCoveredExsited);
                }
                GenerateIServices(table, isCoveredExsited);
                GenerateServices(table, isCoveredExsited);
            }
        }

        /// <summary>
        /// 生成实体代码 (model)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="isCoveredExsited"></param>
        private void GenerateEntity(DbTable table, bool ifExsitedCovered = true)
        {
            var sb = new StringBuilder();
            foreach (var column in table.Columns)
            {
                var temp = GenerateEntityProperty(column);
                sb.AppendLine(temp);
            }
            var modelPath = _options.OutputPath + Delimiter + "Cesium.Models";
            //var iRepositoryPath = _options.OutputPath + Delimiter + "IRepository";
            if (!Directory.Exists(modelPath))
            {
                throw new ArgumentNullException("文件夹路径不存在");
            }
            var fullPath = modelPath + Delimiter + table.TableName + "Model.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("ModelTemplate.txt");
            content = content.Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ModelsNamespace}", _options.ModelsNamespace)
                .Replace("{Author}", _options.Author)
                .Replace("{Comment}", table.TableComment)
                .Replace("{ModelName}", table.TableName)
                .Replace("{ModelProperties}", sb.ToString());
            WriteAndSave(fullPath, content);

        }

        /// <summary>
        /// 生成IRepository层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateIRepository(DbTable table, string keyTypeName, bool ifExsitedCovered = true)
        {
            var iRepositoryPath = _options.OutputPath + Delimiter + "Cesium.IRepository";
            if (!Directory.Exists(iRepositoryPath))
            {
                throw new ArgumentNullException("文件夹路径不存在");
            }
            var fullPath = iRepositoryPath + Delimiter + "I" + table.TableName + "Repository.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("IRepositoryTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{IRepositoryNamespace}", _options.IRepositoryNamespace)
                .Replace("{ModelName}", table.TableName)
                .Replace("{KeyTypeName}", keyTypeName);
            WriteAndSave(fullPath, content);
        }

        /// <summary>
        /// 生成Repository层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateRepository(DbTable table, string keyTypeName, bool ifExsitedCovered = true)
        {
            var repositoryPath = _options.OutputPath + Delimiter + "Cesium.Repository";
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + table.TableName + "Repository.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("RepositoryTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{RepositoryNamespace}", _options.RepositoryNamespace)
                .Replace("{ModelName}", table.TableName)
                .Replace("{KeyTypeName}", keyTypeName);
            WriteAndSave(fullPath, content);
        }

        /// <summary>
        /// 生成IServices层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateIServices(DbTable table, bool ifExsitedCovered = true)
        {
            var iServicesPath = _options.OutputPath + Delimiter + "Cesium.IServices";
            if (!Directory.Exists(iServicesPath))
            {
                Directory.CreateDirectory(iServicesPath);
            }
            var fullPath = iServicesPath + Delimiter + "I" + table.TableName + "Service.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("IServicesTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{IServicesNamespace}", _options.IServicesNamespace)
                .Replace("{ModelName}", table.TableName);
            WriteAndSave(fullPath, content);
        }

        /// <summary>
        /// 生成Services层代码文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateServices(DbTable table, bool ifExsitedCovered = true)
        {
            var repositoryPath = _options.OutputPath + Delimiter + "Cesium.Services";
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + table.TableName + "Service.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("ServiceTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ServicesNamespace}", _options.ServicesNamespace)
                .Replace("{ModelName}", table.TableName);
            WriteAndSave(fullPath, content);
        }

        /// <summary>
        /// 根据数据列生成实体对应property
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string GenerateEntityProperty(DbTableColumn column)
        {
            var sb = new StringBuilder();
            if (!column.Comment.IsNullOrWhiteSpace())
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + column.Comment);
                sb.AppendLine("\t\t/// </summary>");
            }
            if (column.IsPrimaryKey)
            {
                sb.AppendLine("\t\t[Key]");
                sb.AppendLine($"\t\tpublic {column.CSharpType} {column.ColName} " + "{get;set;}");
            }
            else
            {
                var colType = column.CSharpType;
                if (colType.ToLower() != "string" && colType.ToLower() != "byte[]" && colType.ToLower() != "object" &&
                    column.IsNullable)
                {
                    colType = colType + "?";
                }

                sb.AppendLine($"\t\tpublic {colType} {column.ColName} " + "{ get; set; }");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 读取模板文件（内嵌资源文件）
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        private string ReadTemplate(string templateName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var content = string.Empty;
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.CodeTemplate.{templateName}"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        private static void WriteAndSave(string fileName, string content)
        {
            using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            using var sw = new StreamWriter(fs);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
