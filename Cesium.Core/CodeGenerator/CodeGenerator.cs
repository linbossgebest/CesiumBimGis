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
                _options.OutputPath = path;
        }

        private void GenerateEntity(DbTable table, bool isCoveredExsited = true)
        {
            var sb = new StringBuilder();
            foreach (var column in table.Columns)
            {
                var temp = GenerateEntityProperty(column);
                sb.AppendLine(temp);
            }
            var modelPath = "";
            //var iRepositoryPath = _options.OutputPath + Delimiter + "IRepository";
            //if (!Directory.Exists(iRepositoryPath))
            //{
            //    Directory.CreateDirectory(iRepositoryPath);
            //}
            //var fullPath = iRepositoryPath + Delimiter + "I" + table.TableName + "Repository.cs";
            //if (File.Exists(fullPath) && !ifExsitedCovered)
            //    return;
            var content = ReadTemplate("ModelTemplate.txt");
            content = content.Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ModelsNamespace}", _options.ModelsNamespace)
                .Replace("{Author}", _options.Author)
                .Replace("{Comment}", table.TableComment)
                .Replace("{ModelName}", table.TableName)
                .Replace("{ModelProperties}", sb.ToString());
            WriteAndSave(modelPath, content);

        }

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

                sb.AppendLine($"\t\tpublic {colType} {column.ColName} " + "{get;set;}");
            }
            return sb.ToString();
        }

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
