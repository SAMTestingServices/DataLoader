using System.Collections.Generic;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Tasks
{
    public interface IUserGroupTask
    {
        void AddUserGroup(UserGroup userGroup);

        UserGroup AddUserGroup(string applicationUrl, string name, string description);

        List<UserGroup> GetByIds(params int[] ids);

        UserGroup GetById(int id);

        void AddUserGroupCredentialValue(UserGroupCredentialValue userGroupCredentialValue);

        UserGroup AddDevolvedAdminUserGroup(string name, string description);

        bool DoesUserGroupAlreadyExist(int userGroupId);

        void Save(UserGroup userGroup);

        List<UserGroup> GetUserGroupsAssociatedToCredentialValue(int credentialValueId);

        void DeleteUserGroupsAndAssociatedRelationships(params int[] userGroupIds);

        List<UserGroup> SearchByNames(params string[] names);
    }
}