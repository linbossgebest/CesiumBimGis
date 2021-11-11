using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.Core.Redis
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisCacheManager
    {
        //获取 Reids 缓存值
        Task<string> GetValue(string key);

        //获取值，并序列化
        Task<TEntity> Get<TEntity>(string key);

        //保存
        Task Set(string key, object value, TimeSpan cacheTime);

        //判断是否存在
        Task<bool> Exist(string key);

        //移除某一个缓存值
        Task Remove(string key);

        //全部清除
        Task Clear();

        Task<RedisValue[]> ListRedisValueRangeAsync(string redisKey);

        Task<long> ListLeftPushAsync(string redisKey, string redisValue);

        Task<long> ListRightPushAsync(string redisKey, string redisValue);

        Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue);

        Task<T> ListLeftPopAsync<T>(string redisKey) where T : class;

        Task<T> ListRightPopAsync<T>(string redisKey) where T : class;

        Task<string> ListLeftPopAsync(string redisKey);

        Task<string> ListRightPopAsync(string redisKey);

        Task<long> ListLengthAsync(string redisKey);

        Task<IEnumerable<string>> ListRangeAsync(string redisKey);

        Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop);

        Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0);

        Task ListClearAsync(string redisKey);
    }
}
