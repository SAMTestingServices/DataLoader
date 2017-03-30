using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure;
using Lloyds.LAF.Agent.Restricted.Domain;
using Lloyds.LAF.Agent.Restricted.Domain.Search;
using Lloyds.LAF.Agent.Restricted.Infrastructure.Dapper;
using Lloyds.LAF.Agent.Restricted.Infrastructure.Extensions;

namespace Lloyds.LAF.Agent.Restricted.Infrastructure
{
    internal class UserGroupRepository : BaseRepository, IUserGroupRepository
    {
        public List<UserGroup> GetByIds(int[] ids)
        {
            const string StoredProcedureName = "[LAF].[csp_GetUserGroupsById]";
            const string ParameterName = "userGroupIds";

            var foundUserGroups =
                this.RunQuery<UserGroup>(StoredProcedureName, new IntDynamicParameter(ParameterName, ids)).ToList();
            return foundUserGroups;
        }

        public UserGroup GetUserGroupById(int id)
        {
            const string StoredProcedureName = "[LAF].[csp_GetUserGroup]";
            var parameters = new { Id = id };
            var foundId = this.RunQuery<UserGroup>(StoredProcedureName, parameters).FirstOrDefault();
            return foundId;
        }

        public void Save(UserGroup userGroup)
        {
            if (userGroup.IsNewEntity())
            {
                InsertUserGroup(userGroup);    
            }
            else
            {
                UpdateUserGroup(userGroup);
            }
        }

        public void Save(UserGroupCredentialValue value)
        {
            const string RelationshipStoredProcedureName = "[LAF].[csp_InsertUserGroupCredentialValueWithExpiryDate]";
            var parameters = new DynamicParameters(new
            {
                userGroupId = value.UserGroupId,
                credentialValueId = value.CredentialValueId,
                value.ExpiryDate
            });

            var foundId = this.RunQuery<int>(RelationshipStoredProcedureName, parameters).FirstOrDefault();
            value.UserGroupCredentialValueId = Convert.ToInt32(foundId);
        }
       
        public UserGroup AddDevolvedAdminUserGroup(string name, string description)
        {
            var createdDaUserGroup = new UserGroup { Description = description, Name = name };
            InsertDevolvedAdminUserGroup(createdDaUserGroup);
            InsertDevolvedAdminRoleUserGroupCredentialValue(createdDaUserGroup.UserGroupId);
            return createdDaUserGroup;
        }

        public bool DoesUserGroupAlreadyExist(int userGroupId)
        {
            const string StoredProcedureName = "[LAF].[csp_DoesUserGroupAlreadyExist]";
            var parameters = new
            {
                UserGroupId = userGroupId
            };

            var doesUserGroupExist = this.RunQuery<bool>(StoredProcedureName, parameters).FirstOrDefault();
            return doesUserGroupExist;
        }

        public List<UserGroupCredentialValue> GetUserGroupCredentialValuesByUserGroupId(int userGroupId)
        {
            const string StoredProcedureName = "[LAF].[csp_GetUserGroupCredentialValuesByUserGroupId]";
            var parameters = new
            {
                UserGroupId = userGroupId
            };

            var values = this.RunQuery<UserGroupCredentialValue>(StoredProcedureName, parameters).ToList();
            return values;
        }

        public int GetIdForLafUserGroupCredential()
        {
            const string StoredProcedureName = "[LAF].[csp_GetIdForLafUserGroupCredential]";
            return this.RunQuery<int>(StoredProcedureName, null).FirstOrDefault();
        }

        public List<UserGroup> GetUserGroupsAssociatedToCredentialValue(int credentialValueId)
        {
            const string StoredProcedureName = "[LAF].[csp_GetUserGroupsAssociatedToCredentialValue]";
            var parameters = new { CredentialValueId = credentialValueId };
            var foundItems = this.RunQuery<UserGroup>(StoredProcedureName, parameters).ToList();
            return foundItems;
        }

        public void DeleteUserGroupsAndAssociatedRelationships(params int[] userGroupIds)
        {
            const string StoredProcedureName = "[LAF].[csp_DeleteUserGroupsAndAssociatedRelationships]";
            const string ParameterName = "userGroupIds";

            this.RunQuery<int>(StoredProcedureName, new IntDynamicParameter(ParameterName, userGroupIds));
        }

        public List<UserGroup> SearchByNames(params string[] names)
        {
            const string StoredProcedureName = "[LAF].[csp_SearchForUserGroups]";
            const string ParameterName = "names";
            
            return this.RunQuery<UserGroup>(StoredProcedureName, new VarDynamicParameter(ParameterName, names)).ToList();
        }

        private void InsertUserGroup(UserGroup userGroup)
        {
            const string UserGroupStoredProcedureName = "[LAF].[csp_InsertUserGroup]";

            var parameters = new
            {
                userGroup.ApplicationId,
                userGroup.Name,
                userGroup.Description,
            };

            var foundId = this.RunQuery<int>(UserGroupStoredProcedureName, parameters).FirstOrDefault();
            userGroup.UserGroupId = Convert.ToInt32(foundId);
        }

        private void InsertDevolvedAdminUserGroup(UserGroup daUserGroup)
        {
            const string StoredProcedureName = "[LAF].[csp_InsertDAUserGroup]";

            var parameters = new DynamicParameters(new
                {
                    daUserGroup.Name,
                    daUserGroup.Description
                });

            parameters.Add("@outputLAFApplicationId", 0, size: null, dbType: DbType.Int32, direction: ParameterDirection.Output);

            var foundId = this.RunQuery<int>(StoredProcedureName, parameters).FirstOrDefault();
            daUserGroup.ApplicationId = parameters.Get<int>("@outputLAFApplicationId");
            daUserGroup.UserGroupId = Convert.ToInt32(foundId);
        }

        private void InsertDevolvedAdminRoleUserGroupCredentialValue(int daUserGroupId)
        {
            const string StoredProcedureName = "[LAF].[csp_InsertDevolvedAdminRoleUserGroupCredentialValue]";
            var parameters = new { DAUserGroupId = daUserGroupId};
            this.RunQuery<int>(StoredProcedureName, parameters);
        }

        private void UpdateUserGroup(UserGroup userGroup)
        {
            const string StoreProcedureName = "[LAF].[csp_UpdateUserGroup]";
            var parameter = new
            {
                userGroup.UserGroupId,
                userGroup.Name,
                userGroup.Description
            };

            this.RunQuery<int>(StoreProcedureName, parameter);
        }
    }
}