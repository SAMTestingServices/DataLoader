using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Tasks
{
    public interface IApplicationTask
    {
        Application GetApplicationByUrl(string url);
    }
}
