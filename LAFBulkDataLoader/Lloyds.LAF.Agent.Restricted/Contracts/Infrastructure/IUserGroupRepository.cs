using System.Collections.Generic;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure
{
    public interface IUserGroupRepository
    {
        List<UserGroup> GetByIds(int[] ids);
        
        UserGroup GetUserGroupById(int id);
        
        void Save(UserGroup userGroup);

        void Save(UserGroupCredentialValue userGroupCredentialValue);

        UserGroup AddDevolvedAdminUserGroup(string name, string description);

        List<UserGroupCredentialValue> GetUserGroupCredentialValuesByUserGroupId(int userGroupId);

        bool DoesUserGroupAlreadyExist(int userGroupId);

        int GetIdForLafUserGroupCredential();
        
        List<UserGroup> GetUserGroupsAssociatedToCredentialValue(int credentialValueId);

        void DeleteUserGroupsAndAssociatedRelationships(params int[] userGroupIds);
        
        List<UserGroup> SearchByNames(params string[] searchOptions);
    }
}