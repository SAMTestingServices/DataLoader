using System.Collections.Generic;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Tasks
{
    public interface IUserTasks
    {
        List<User> GetUsers(string applicationUrl, string credentialElementNid, string credentialElementValue);
    }
}