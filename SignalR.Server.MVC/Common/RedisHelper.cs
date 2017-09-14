using SignalR.Server.MVC.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace SignalR.Server.MVC.Common
{
    public class RedisHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">connectionId</param>
        /// <param name="value"></param>
        /// <param name="dbIndex"></param>
        public async void SetValue(string key, UserModel value, int dbIndex = 0)
        {
            using (ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(dbIndex);
                // 如果缓存中已经有了，就不再存储
                HashEntry[] tmp = await db.HashGetAllAsync(key);
                if (tmp == null || tmp.Length < 1)
                {
                    PropertyInfo[] PropertyList = value.GetType().GetProperties();
                    HashEntry[] values = new HashEntry[PropertyList.Length];
                    for (int i = 0; i < PropertyList.Length; i++)
                    {
                        values[i] = new HashEntry(PropertyList[i].Name, PropertyList[i].GetValue(value).ToString());
                    }
                    await db.HashSetAsync(key, values);
                }
            }
        }

        public async Task<UserModel> GetValue(string key, int dbIndex = 0)
        {
            using (ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(dbIndex);
                var tmp = await db.HashGetAllAsync(key);
                if (tmp == null || tmp.Length < 1) return null;
                UserModel re = new UserModel();
                foreach (var prop in tmp)
                {
                    PropertyInfo[] PropertyList = typeof(UserModel).GetProperties();
                    foreach (var p in PropertyList)
                    {
                        var propertyCurrent = tmp.Where(x => x.Name.Equals(p.Name)).FirstOrDefault();
                        if (propertyCurrent != null)
                        {
                            p.SetValue(re, propertyCurrent.Value);
                        }
                    }
                }
                return re;
            }
        }
        /*
        按 connectionid name 存储用户对象
        按 group connectionid 存储组对象
        按 gender connectionid 存储性别对象
    */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionid"></param>
        /// <param name="name">名字</param>
        /// <param name="groupName">组名称</param>
        /// <param name="gender">性别</param>
        /// <param name="dbIndex">哪个库</param>
        public async Task insertUserAsync(string connectionid, string name, string groupName, string gender, int dbIndex = 0)
        {
            using (ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(dbIndex);
                // 存储用户
                await db.StringSetAsync(connectionid, name);
                // 存储组
                if (!string.IsNullOrEmpty(groupName))
                {
                    await db.SetAddAsync(groupName, connectionid);
                }
                // 存储性别
                await db.SetAddAsync(gender, connectionid);
            }
        }

        public void insertUser(string connectionid, string name, string groupName, string gender, int dbIndex = 0)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(dbIndex);
                // 存储用户
                db.StringSetAsync(connectionid, name);
                // 存储组
                if (!string.IsNullOrEmpty(groupName))
                {
                    db.SetAddAsync(groupName, connectionid);
                }
                // 存储性别
                db.SetAddAsync(gender, connectionid);
            }
        }


        public async Task<string> getUserName(string connectionid)
        {
            return await getValue(connectionid);
        }
        public async Task<string[]> getConnectionidByGroupName(string groupName)
        {
            return await getValues(groupName);
        }
        public async Task<string[]> getConnectionidByGender(string gender)
        {
            return await getValues(gender);
        }
        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        private async Task<string> getValue(string key, int dbIndex = 0)
        {
            using (ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(dbIndex);
                return await db.StringGetAsync(key);
            }
        }

        /// <summary>
        /// 根据分组获取组内成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        private async Task<string[]> getValues(string key, int dbIndex = 0)
        {
            RedisValue[] redisValues;
            using (ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(dbIndex);
                redisValues = await db.SetMembersAsync(key);
            }

            if (redisValues.Length < 1) return null;
            string[] re = new string[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                re[i] = redisValues[i];
            }
            return re;
        }


        public async Task RemoveUserAsync(string connectionid,string groupname,string gender, int dbIndex = 0)
        {
            using (ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379"))
            {
                IDatabase db = redis.GetDatabase(dbIndex);
                // 删除用户
                if (await db.KeyExistsAsync(connectionid))
                {
                    await db.KeyDeleteAsync(connectionid);

                    // 删除组里的用户
                    await db.SetRemoveAsync(groupname, connectionid);
                    
                    // 删除性别里的用户
                    await db.SetRemoveAsync(gender, connectionid);
                }
                
                
            }
        }
    }
}