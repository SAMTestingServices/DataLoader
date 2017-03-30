using System;
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
    internal class CredentialTasks : ICredentialTasks
    {
        private readonly IAuditTasks auditTasks;
        private readonly ICredentialRepository credentialRepository;
        private readonly IUserGroupTask userGroupTask;
        private readonly IApplicationTask applicationTask;

        public CredentialTasks(ICredentialRepository credentialRepository, 
            IUserGroupTask userGroupTask, 
            IApplicationTask applicationTask,
            IAuditTasks auditTasks)
        {
            this.credentialRepository = credentialRepository;
            this.userGroupTask = userGroupTask;
            this.applicationTask = applicationTask;
            this.auditTasks = auditTasks;
        }

        public void AddCredentialValueAndAssociatedUserGroup(string applicationUrl, string credentialElementNid,
                                                             string value, string description = null)
        {
            var credentialValue = this.GetCredentialValue(applicationUrl, credentialElementNid, value, description);
            if (credentialValue == null)
            {
                return;
            }

            using (var operation = BeginOperation())
            {
                UserGroup createdUserGroup;
                this.credentialRepository.AddCredentialValueAndAssociatedUserGroup(credentialValue, out createdUserGroup);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(CredentialValueAction.Created,
                                       AuditInformation.Create()
                                                       .WithCredentialValues(credentialValue));

            this.auditTasks.WriteEntry(UserGroupAction.Created,
                                       AuditInformation.Create()
                                                       .WithUserGroup(credentialValue.AssociatedUserGroup));

            this.auditTasks.WriteEntry(UserGroupAction.CredentialAdded,
                                       AuditInformation.Create()
                                                       .WithUserGroupCredentialValues(
                                                           credentialValue.AssociatedUserGroupCredentialValue));
        }

        public void AddCredentialValueAndAssociatedUserGroupAndDaUserGroup(string applicationUrl,
                                                                           string credentialUrnName,
                                                                           string credentialValueValue,
                                                                           string credentialValueDescription,
                                                                           out CredentialValue createdCredentialValue,
                                                                           out UserGroup createdUserGroup,
                                                                           out UserGroup createdDaUserGroup)
        {
            createdCredentialValue = this.GetCredentialValue(applicationUrl, credentialUrnName, credentialValueValue,
                                                             credentialValueDescription);
            if (createdCredentialValue == null)
            {
                throw new ApplicationException("An attempt was made to create a CredentialValue that already exists.");
            }

            CredentialValue createdLafUserGroupCredentialValueForCreatedDaUserGroup;


            using (var operation = BeginOperation())
            {
                this.credentialRepository.AddCredentialValueAndAssociatedUserGroupAndDaUserGroup(createdCredentialValue,
                                                                                                 out createdUserGroup,
                                                                                                 out createdDaUserGroup,
                                                                                                 out
                                                                                                     createdLafUserGroupCredentialValueForCreatedDaUserGroup);
                operation.Complete();
            }

            var daUserGroupCredentialValues =
                this.credentialRepository.GetUserGroupCredentialValuesByUserGroupId(createdDaUserGroup.UserGroupId)
                    .ToArray();

            this.auditTasks.WriteEntry(CredentialValueAction.Created,
                                       AuditInformation.Create().WithCredentialValues(createdCredentialValue));

            this.auditTasks.WriteEntry(UserGroupAction.Created,
                                       AuditInformation.Create()
                                                       .WithUserGroup(createdCredentialValue.AssociatedUserGroup));

            this.auditTasks.WriteEntry(UserGroupAction.CredentialAdded,
                                       AuditInformation.Create()
                                                       .WithUserGroupCredentialValues(
                                                           createdCredentialValue.AssociatedUserGroupCredentialValue));

            this.auditTasks.WriteEntry(UserGroupAction.Created,
                                       AuditInformation.Create().WithUserGroup(createdDaUserGroup));

            this.auditTasks.WriteEntry(CredentialValueAction.Created,
                                       AuditInformation.Create()
                                                       .WithCredentialValues(
                                                           createdLafUserGroupCredentialValueForCreatedDaUserGroup));

            this.auditTasks.WriteEntry(UserGroupAction.CredentialAdded,
                                       AuditInformation.Create()
                                                       .WithUserGroupCredentialValues(daUserGroupCredentialValues));
        }

        public void AddCredentialValueAndAssociatedUserGroupToExistingDaUserGroup(string applicationUrl,
                                                                                  string credentialUrnName,
                                                                                  string credentialValueValue,
                                                                                  string credentialValueDescription,
                                                                                  int daUserGroupId,
                                                                                  out CredentialValue
                                                                                      createdCredentialValue,
                                                                                  out UserGroup createdUserGroup)
        {
            createdCredentialValue = this.GetCredentialValue(applicationUrl, credentialUrnName, credentialValueValue,
                                                             credentialValueDescription);
            if (createdCredentialValue == null)
            {
                throw new ApplicationException("An attempt was made to create a CredentialValue that already exists.");
            }

            CredentialValue createdLafUserGroupCredentialValueForExistingDaUserGroup;

            using (var operation = BeginOperation())
            {
                this.credentialRepository.AddCredentialValueAndAssociatedUserGroupToExistingDaUserGroup(
                    createdCredentialValue,
                    applicationUrl,
                    daUserGroupId,
                    out createdUserGroup,
                    out createdLafUserGroupCredentialValueForExistingDaUserGroup);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(CredentialValueAction.Created,
                                       AuditInformation.Create().WithCredentialValues(createdCredentialValue));

            this.auditTasks.WriteEntry(UserGroupAction.Created,
                                       AuditInformation.Create()
                                                       .WithUserGroup(createdCredentialValue.AssociatedUserGroup));

            this.auditTasks.WriteEntry(UserGroupAction.CredentialAdded,
                                       AuditInformation.Create()
                                                       .WithUserGroupCredentialValues(
                                                           createdCredentialValue.AssociatedUserGroupCredentialValue));

            this.auditTasks.WriteEntry(CredentialValueAction.Created,
                                       AuditInformation.Create()
                                                       .WithCredentialValues(
                                                           createdLafUserGroupCredentialValueForExistingDaUserGroup));

            var daUserGroupCredentialValues =
                this.credentialRepository.GetUserGroupCredentialValuesByUserGroupId(daUserGroupId);

            var createdUgcv =
                daUserGroupCredentialValues.FindAll(
                    x =>
                    x.CredentialValueId == createdLafUserGroupCredentialValueForExistingDaUserGroup.CredentialValueId)
                                           .ToArray();

            this.auditTasks.WriteEntry(UserGroupAction.CredentialAdded,
                                       AuditInformation.Create()
                                                       .WithUserGroupCredentialValues(createdUgcv));
        }

        public void AddDevolvedAdminUserGroupForGivenUserGroup(string name, string description,
                                                               int idOfUserGroupForWhichADevolvedAdminUserGroupIsToBeCreated,
                                                               out UserGroup createdDaUserGroup)
        {
            var userGroup = this.userGroupTask.GetById(idOfUserGroupForWhichADevolvedAdminUserGroupIsToBeCreated);
            CredentialValue createdLafUserGroupCredentialValueForCreatedDaUserGroup;

            if (!this.userGroupTask.DoesUserGroupAlreadyExist(idOfUserGroupForWhichADevolvedAdminUserGroupIsToBeCreated))
            {
                throw new ApplicationException("An attempt was made to create a LAF Devolved Administrators User Group for a User Group that does not exist.");
            }

            using (var operation = BeginOperation())
            {
                this.credentialRepository.AddDevolvedAdminUserGroupForGivenUserGroup(name, description, userGroup,
                                                                                     out createdDaUserGroup,
                                                                                     out createdLafUserGroupCredentialValueForCreatedDaUserGroup);
                operation.Complete();
            }

            var daUserGroupCredentialValues =
                this.credentialRepository.GetUserGroupCredentialValuesByUserGroupId(createdDaUserGroup.UserGroupId)
                    .ToArray();

            this.auditTasks.WriteEntry(UserGroupAction.Created,
                                       AuditInformation.Create().WithUserGroup(createdDaUserGroup));

            this.auditTasks.WriteEntry(CredentialValueAction.Created,
                                       AuditInformation.Create()
                                                       .WithCredentialValues(
                                                           createdLafUserGroupCredentialValueForCreatedDaUserGroup));

            this.auditTasks.WriteEntry(UserGroupAction.CredentialAdded,
                                       AuditInformation.Create()
                                                       .WithUserGroupCredentialValues(daUserGroupCredentialValues));
        }

        public void AddCredentialValue(CredentialValue credentialValue)
        {
            using (var operation = BeginOperation())
            {
                this.credentialRepository.Save(credentialValue);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(CredentialValueAction.Created,
                                       AuditInformation.Create().WithCredentialValues(credentialValue));
        }

        public CredentialValue AddCredentialValue(string applicationUrl, string credentialUrnName,
                                                  string credentialValueValue, string credentialValueDescription)
        {
            var newCredentialValue = this.GetCredentialValue(applicationUrl, credentialUrnName, credentialValueValue,
                                                             credentialValueDescription);
            if (newCredentialValue == null)
            {
                throw new ApplicationException("An attempt was made to create a CredentialValue that already exists.");
            }

            using (var operation = BeginOperation())
            {
                this.credentialRepository.Save(newCredentialValue);
                operation.Complete();
            }

            //this.auditTasks.WriteEntry(CredentialValueAction.Created,
             //                          AuditInformation.Create().WithCredentialValues(newCredentialValue));

            return newCredentialValue;
        }

        public CredentialValue AddLafCredentialValue(string lafApplicationUrl, string sourceApplicationUrl,
                                                     string credentialUrnName, string credentialValueValue,
                                                     string credentialValueDescription)
        {
            var newCredentialValue = this.GetCredentialValue(lafApplicationUrl, credentialUrnName, credentialValueValue,
                                                             credentialValueDescription);
            if (newCredentialValue == null)
            {
                throw new ApplicationException("An attempt was made to create a CredentialValue that already exists.");
            }

            var application = this.applicationTask.GetApplicationByUrl(sourceApplicationUrl);
            newCredentialValue.SourceApplicationId = application.ApplicationId;

            using (var operation = BeginOperation())
            {
                this.credentialRepository.Save(newCredentialValue);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(CredentialValueAction.Created,
                                       AuditInformation.Create().WithCredentialValues(newCredentialValue));

            return newCredentialValue;
        }

        public List<string> GetCredentialValuesForApplication(string applicationUrl, string credentialElementNid)
        {
            applicationUrl = applicationUrl.GetSanitisedApplicationUrl();
            var foundValues = this.credentialRepository.GetCredentialValuesForApplication(applicationUrl,
                                                                                          credentialElementNid);
            return foundValues;
        }

        public List<string> GetCredentialValuesForUser(string applicationUrl, string credentialElementNid, string username)
        {
            applicationUrl = applicationUrl.GetSanitisedApplicationUrl();
            var foundValues = this.credentialRepository.GetCredentialValuesForUser(applicationUrl, credentialElementNid, username);
            return foundValues;
        }

        public List<CredentialValue> SearchForCredentialValues(CredentialValueSearchOptions searchOptions)
        {
            return this.credentialRepository.SearchForCredentialValues(searchOptions);
        }

        public void Save(CredentialValue value)
        {
            var isNew = value.IsNewEntity();
            var action = CredentialValueAction.Created;
            if (!isNew)
            {
                action = CredentialValueAction.Edited;
            }

            using (var operation = BeginOperation())
            {
                this.credentialRepository.Save(value);
                operation.Complete();
            }

            this.auditTasks.WriteEntry(action, AuditInformation.Create().WithCredentialValues(value));
        }

        public CredentialValue GetCredentialValueById(int id)
        {
            var foundItem = this.credentialRepository.GetCredentialValueById(id);
            return foundItem;
        }

        #region (Obsolete methods that can be removed once all of the people using this API have updated their code base)

        public void AddUserGroup(UserGroup userGroup)
        {
            this.userGroupTask.AddUserGroup(userGroup);
        }

        public UserGroup AddUserGroup(string applicationUrl, string name, string description)
        {
            return this.userGroupTask.AddUserGroup(applicationUrl, name, description);
        }

        public void AddDevolvedAdminUserGroup(string name, string description, out UserGroup createdDaUserGroup)
        {
            createdDaUserGroup = this.userGroupTask.AddDevolvedAdminUserGroup(name, description);
        }

        #endregion
        
        private static bool HasNoMatchingCredentialElement(int id)
        {
            return id == 0;
        }

        private CredentialValue GetCredentialValue(string applicationUrl, string credentialUrnName, string value, string description)
        {
            applicationUrl = applicationUrl.GetSanitisedApplicationUrl();
            var id = this.credentialRepository.GetCredentialId(applicationUrl, credentialUrnName);
            if (HasNoMatchingCredentialElement(id))
            {
                throw new ApplicationException("No valid Credential could be found");
            }

            using (var operation = BeginDirtyOperation())
            {
                if (this.credentialRepository.DoesCredentialValueAlreadyExist(id, value))
                {
                    return null;
                }

                operation.Complete();
            }

            var credentialElementValue = new CredentialValue
                {
                    CredentialId = id,
                    Value = value,
                    Description = description
                };

            return credentialElementValue;
        }

        public Dictionary<int, List<int>> GetUserGroupsAssociatedToCredentialValues(int[] ids)
        {
            var result = new Dictionary<int, List<int>>();
            var credentialValueAssociations = this.credentialRepository.GetUserGroupsAssociatedToCredentialValues(ids);

            foreach (var association in credentialValueAssociations)
            {
                if (!result.ContainsKey(association.CredentialValueId))
                {
                    result.Add(association.CredentialValueId, new List<int>());
                }

                result[association.CredentialValueId].Add(association.UserGroupId);
            }

            return result;
        }

        public List<CredentialValue> GetManyCredentialValues(string urn, params string[] values)
        {
            var items = this.credentialRepository.GetManyCredentialValues(urn, values);
            return items;
        }

        public void DeleteCredentialValuesAndAssociatedRelationships(params int[] credentialValueIds)
        {
            this.credentialRepository.DeleteCredentialValuesAndAssociatedRelationships(credentialValueIds);
        }

        private static TransactionScope BeginOperation()
        {
            var operation = new TransactionScope(TransactionScopeOption.Required,
                                                 new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted});
            return operation;
        }

        private static TransactionScope BeginDirtyOperation()
        {
            var operation = new TransactionScope(TransactionScopeOption.Required,
                                                 new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted });
            return operation;
        }
    }
}