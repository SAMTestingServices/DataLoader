using System.Collections.Generic;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure
{
    public interface IUserRepository
    {
        List<User> GetUsers(string applicationUrl, string credentialUniformResourceName, string credentialElementValue);
    }
}