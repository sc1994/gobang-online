using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace SocketAPI
{
    public class Consts
    {
        public const string RedisSite = "118.24.27.231:6379,password=sun940622";
    }

    public class RedisMethods
    {
        private readonly IDatabase _db;

        public RedisMethods()
        {
            _db = ConnectionMultiplexer.ConnectAsync(Consts.RedisSite).Result.GetDatabase(10);
        }

        /// <summary>
        /// 累加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long StringIncrement(string key, long value)
        {
            return _db.StringIncrement(key, value);
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T value)
        {
            return _db.StringSet(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T StringGet<T>(string key)
        {
            var v = _db.StringGet(key);
            if (string.IsNullOrWhiteSpace(v))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(v);
        }

        /// <summary>
        /// 右添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string key, T value)
        {
            return _db.ListRightPush(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 右移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ListRemove<T>(string key, T value)
        {
            return _db.ListRemove(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 全部
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(string key, long start = 0, long stop = -1)
        {
            return _db.ListRange(key, start, stop).Select(x => JsonConvert.DeserializeObject<T>(x)).ToList();
        }
    }
}
