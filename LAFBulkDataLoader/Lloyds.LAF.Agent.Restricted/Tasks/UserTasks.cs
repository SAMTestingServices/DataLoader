using System.Collections.Generic;
using Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure;
using Lloyds.LAF.Agent.Restricted.Contracts.Tasks;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Tasks
{
    internal class UserTasks : IUserTasks
    {
        private readonly IUserRepository repository;

        public UserTasks(IUserRepository repository)
        {
            this.repository = repository;
        }

        public List<User> GetUsers(string applicationUrl, string credentialElementNid, string credentialElementValue)
        {
            applicationUrl = applicationUrl.TrimEnd('/');
            var foundUsers = this.repository.GetUsers(applicationUrl, credentialElementNid, credentialElementValue);
            return foundUsers;
        }
    }
}