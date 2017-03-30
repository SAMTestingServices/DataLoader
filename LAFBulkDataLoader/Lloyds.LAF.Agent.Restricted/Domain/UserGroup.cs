namespace Lloyds.LAF.Agent.Restricted.Domain
{
    public class UserGroup
    {
        public int UserGroupId { get; internal set; }

        public int ApplicationId  { get; set; }

        public string Name  { get; set; }

        public string Description  { get; set; }
    }
}

