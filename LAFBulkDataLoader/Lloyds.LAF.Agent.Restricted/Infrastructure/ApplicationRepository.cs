using System.Linq;
using Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Infrastructure
{
    internal class ApplicationRepository : BaseRepository, IApplicationRepository
    {
        public Application GetApplicationByUrl(string url)
        {
            const string StoredProcedureName = "[LAF].[csp_GetApplicationByUrl_V2]";

            var parameters = new {url};
            var found = this.RunQuery<Application>(StoredProcedureName, parameters)
                .FirstOrDefault();
            return found;
        }
    }
}
