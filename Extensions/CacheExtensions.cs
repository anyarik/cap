using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAP.Bot.Telegram.Models;
using Microsoft.Extensions.Caching.Memory;
using Models.PlanModel;
using Models.ResourceTaskModel;

namespace CAP.Bot.Telegram.Extensions
{
    public static class CacheExtensions
    {
        public static void UpdateResourceTask(this IMemoryCache cache, UserChat userChat, ResourceTask task)
        {
            cache.Set(GetKey(userChat, "task"), task);  
        }

        public static ResourceTask GetResourceTask(this IMemoryCache cache, UserChat userChat)
        {
            return cache.Get<ResourceTask>(GetKey(userChat, "task"));
        }

        public static void RemoveResourceTask(this IMemoryCache cache, UserChat userChat)
        {
            cache.Remove(GetKey(userChat, "task"));
        }

        public static void UpdateTask(this IMemoryCache cache, UserChat userChat, TaskModel task)
        {
            cache.Set(GetKey(userChat, "task"), task);
        }

        public static TaskModel GetTask(this IMemoryCache cache, UserChat userChat)
        {
            return cache.Get<TaskModel>(GetKey(userChat, "task"));
        }

        public static void RemoveTask(this IMemoryCache cache, UserChat userChat)
        {
            cache.Remove(GetKey(userChat, "task"));
        }

        private static string GetKey(UserChat userchat, string kind) =>
            $@"{{""u"":{userchat.UserId},""c"":{userchat.ChatId},""k"":""{kind}""}}";
    }
}
