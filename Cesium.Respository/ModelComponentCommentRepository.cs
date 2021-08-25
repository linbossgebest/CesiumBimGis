using Cesium.Core;
using Cesium.Core.DbHelper;
using Cesium.Core.Extensions;
using Cesium.IRepository;
using Cesium.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Respository
{
    public class ModelComponentCommentRepository : BaseRepository<ModelComponentComment, int>, IDependency, IModelComponentCommentRepository
    {
        //public ModelComponentCommentRepository(IOptionsSnapshot<DbOption> options)
        //{
        //    _dbOption = options.Get("DbOption");
        //    if (_dbOption == null)
        //    {
        //        throw new ArgumentNullException(nameof(DbOption));
        //    }
        //    _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        //}
        public ModelComponentCommentRepository(IOptionsSnapshot<DbOption> options) : base(options.Get("DbOption"))
        {
        }
    }
}
