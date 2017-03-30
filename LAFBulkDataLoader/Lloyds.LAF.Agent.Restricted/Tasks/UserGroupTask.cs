using System.Collections.Generic;
using System.Transactions;
using Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure;
using Lloyds.LAF.Agent.Restricted.Contracts.Tasks;
using Lloyds.LAF.Agent.Restricted.Domain;
using Lloyds.LAF.Agent.Restricted.Domain.Search;
using Lloyds.LAF.Agent.Restricted.Extensions;
using Lloyds.LAF.Agent.Restricted.Infrastructure.Extensions;
using Lloyds.LAF.Audit;
using Lloyds.LAF.Audit.Contracts.Tasks;
using Lloyds.LAF.Audit.Enum;

namespace Lloyds.LAF.Agent.Restricted.Tasks
{
    internal class UserGroupTask : IUserGroupTask
    {
        private readonly IApplicationTask applicationTask;
        private readonly IAuditTasks auditTasks;
        private readonly IUserGroupRepository userGroupRepository;

        public UserGroupTask(IUserGroupRepository userGroupRepository,
                             IApplicationTask applicationTask,
                             IAuditTasks auditTasks)
        {
            this.userGroupRepository = userGroupRepository;
            this.applicationTask = applicationTask;
            this.auditTasks = auditTasks;
        }

        public void AddUserGroup(UserGroup userGroup)
        {
            using (var operation = BeginOperation())
            {
                this.userGroupRepository.Save(userGroup);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(UserGroupAction.Created, AuditInformation.Create().WithUserGroup(userGroup));
        }

        public UserGroup AddUserGroup(string applicationUrl, string name, string description)
        {
            var application = this.applicationTask.GetApplicationByUrl(applicationUrl);
            if (application == null)
            {
                return null;
            }

            var userGroup = new UserGroup
                {
                    ApplicationId = application.ApplicationId,
                    Description = description,
                    Name = name
                };
            using (var operation = BeginOperation())
            {
                this.userGroupRepository.Save(userGroup);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(UserGroupAction.Created, AuditInformation.Create().WithUserGroup(userGroup));
            return userGroup;
        }

        public List<UserGroup> GetByIds(params int[] ids)
        {
            var foundItems = this.userGroupRepository.GetByIds(ids);
            return foundItems;
        }

        public UserGroup GetById(int id)
        {
            var foundItem = this.userGroupRepository.GetUserGroupById(id);
            return foundItem;
        }

        public UserGroup AddDevolvedAdminUserGroup(string name, string description)
        {
            UserGroup userGroup;
            using (var operation = BeginOperation())
            {
                userGroup = this.userGroupRepository.AddDevolvedAdminUserGroup(name, description);

                var userGroupCredentialValues =
                        this.userGroupRepository.GetUserGroupCredentialValuesByUserGroupId(userGroup.UserGroupId).ToArray();

                this.auditTasks.WriteEntry(UserGroupAction.Created,
                                            AuditInformation.Create().WithUserGroup(userGroup));

                this.auditTasks.WriteEntry(UserGroupAction.CredentialAdded,
                                            AuditInformation.Create()
                                                            .WithUserGroupCredentialValues(userGroupCredentialValues));

                operation.Complete();
            }

            return userGroup;
        }

        public bool DoesUserGroupAlreadyExist(int userGroupId)
        {
            return this.userGroupRepository.DoesUserGroupAlreadyExist(userGroupId);
        }

        public void Save(UserGroup userGroup)
        {
            var isNew = userGroup.IsNewEntity();
            var action = UserGroupAction.Created;
            if (!isNew)
            {
                action = UserGroupAction.Edited;
            }

            using (var operation = BeginOperation())
            {
                this.userGroupRepository.Save(userGroup);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(action, AuditInformation.Create().WithUserGroup(userGroup));
        }

        public void AddUserGroupCredentialValue(UserGroupCredentialValue userGroupCredentialValue)
        {
            using (var operation = BeginOperation())
            {
                this.userGroupRepository.Save(userGroupCredentialValue);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(UserGroupAction.CredentialAdded,
                                       AuditInformation.Create()
                                                       .WithUserGroupCredentialValues(userGroupCredentialValue));
        }

        public List<UserGroup> GetUserGroupsAssociatedToCredentialValue(int credentialValueId)
        {
            var foundUserGroups = this.userGroupRepository.GetUserGroupsAssociatedToCredentialValue(credentialValueId);
            return foundUserGroups;
        }

        public void DeleteUserGroupsAndAssociatedRelationships(params int[] userGroupIds)
        {
            using (var operation = BeginOperation())
            {
                this.userGroupRepository.DeleteUserGroupsAndAssociatedRelationships(userGroupIds);
                operation.Complete();
            }
        }

        private static TransactionScope BeginOperation()
        {
            var operation = new TransactionScope(TransactionScopeOption.Required,
                                                 new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted});
            return operation;
        }

        public List<UserGroup> SearchByNames(params string[] names)
        {
            var foundUserGroups = this.userGroupRepository.SearchByNames(names);
            return foundUserGroups;
        }
    }
}