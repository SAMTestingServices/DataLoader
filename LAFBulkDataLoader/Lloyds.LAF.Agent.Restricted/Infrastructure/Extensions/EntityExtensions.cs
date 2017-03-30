using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsNewEntity(this CredentialValue value)
        {
            var newEntity = value.CredentialValueId == 0;
            return newEntity;
        }

        public static bool IsNewEntity(this UserGroup userGroup)
        {
            var newEntity = userGroup.UserGroupId == 0;
            return newEntity;
        }
    }
}
