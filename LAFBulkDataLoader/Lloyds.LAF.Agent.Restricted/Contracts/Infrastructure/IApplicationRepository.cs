using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure
{
    public interface IApplicationRepository
    {
        Application GetApplicationByUrl(string url);
    }
}