using System.Collections.Generic;
using System.Linq;
using Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Infrastructure
{
    internal class UserRepository : BaseRepository, IUserRepository
    {
        public List<User> GetUsers(string applicationUrl, string credentialUniformResourceName,
                                   string credentialElementValue)
        {
            const string StoredProcedureName = "[LAF].[csp_GetUsers_V2]";
            var parameters = new
                {
                    ApplicationUrl = applicationUrl,
                    CredentialUniformResourceName = credentialUniformResourceName,
                    CredentialValue = credentialElementValue
                };

            var foundUsers = this.RunQuery<User>(StoredProcedureName, parameters);
            return foundUsers.ToList();
        }
    }
}