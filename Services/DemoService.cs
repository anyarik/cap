using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Models.PlanModel;
using Models.ResourceModel;
using Models.UserModel;
using Repositories.PlanRepo;
using Repositories.ResourceRepo;

namespace CAP.Bot.Telegram.Services
{
    public interface IDemoService
    {
        Task<List<TaskModel>> GetAllTasks();
        Task<List<UserWithResurceID>> GetAllUsers();
        Task<Guid> GetLastCollectivePlanId();
    }

    public class DemoService : IDemoService
    {
        private readonly IPlanRepository planRepository;
        private readonly IResourceRepository resourceRepository;
        private readonly IMemoryCache cache;

        public DemoService( IPlanRepository planRepository
                          , IResourceRepository resourceRepository
                          , IMemoryCache cache)
        {
            this.planRepository = planRepository;
            this.resourceRepository = resourceRepository;
            this.cache = cache;
        }

        public async Task<List<TaskModel>> GetAllTasks()
        {
            var tasksCash = this.cache.Get<List<TaskModel>>("all_tasks");

            if (tasksCash != null) return tasksCash;

            var tasks =  await planRepository.GetAllTasks().ConfigureAwait(false);

            this.cache.Set("all_tasks", tasks, TimeSpan.FromMinutes(1));

            return tasks;
        }

        public async Task<List<UserWithResurceID>> GetAllUsers()
        {
            var usersCash = this.cache.Get<List<UserWithResurceID>>("all_users");

            if (usersCash != null) return usersCash;

            var users = await resourceRepository.GetAllResources().ConfigureAwait(false);

            this.cache.Set("all_users", users, TimeSpan.FromMinutes(1));

            return users;
        }

        public async Task<Guid> GetLastCollectivePlanId()
        {
            return await this.planRepository.GetLastCollectivePlanId();
        }
    }
}
