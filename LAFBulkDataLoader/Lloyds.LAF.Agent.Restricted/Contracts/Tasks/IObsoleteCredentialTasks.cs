using System;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Tasks
{
    public interface IObsoleteCredentialTasks
    {
        [Obsolete("Obsolete use -> IUserGroupTask.AddUserGroup")]
        void AddUserGroup(UserGroup userGroup);

        [Obsolete("Obsolete use -> IUserGroupTask.AddUserGroup")]
        UserGroup AddUserGroup(string applicationUrl, string name, string description);

        [Obsolete("Obsolete use -> IUserGroupTask.AddDevolvedAdminUserGroup")]
        void AddDevolvedAdminUserGroup(string name, string description, out UserGroup createdDaUserGroup);
    }
}
