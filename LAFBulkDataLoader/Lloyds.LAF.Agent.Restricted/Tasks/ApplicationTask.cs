using Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure;
using Lloyds.LAF.Agent.Restricted.Contracts.Tasks;
using Lloyds.LAF.Agent.Restricted.Domain;
using Lloyds.LAF.Agent.Restricted.Extensions;

namespace Lloyds.LAF.Agent.Restricted.Tasks
{
    public class ApplicationTask : IApplicationTask
    {
        private readonly IApplicationRepository repository;

        public ApplicationTask(IApplicationRepository repository)
        {
            this.repository = repository;
        }

        public Application GetApplicationByUrl(string url)
        {
            url = url.GetSanitisedApplicationUrl();
            var application = this.repository.GetApplicationByUrl(url);
            return application;
        }
    }
}