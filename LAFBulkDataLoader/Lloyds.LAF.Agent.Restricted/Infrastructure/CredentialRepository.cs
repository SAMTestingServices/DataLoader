using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Dapper;
using Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure;
using Lloyds.LAF.Agent.Restricted.Domain;
using Lloyds.LAF.Agent.Restricted.Domain.Search;
using Lloyds.LAF.Agent.Restricted.Infrastructure.Dapper;
using Lloyds.LAF.Agent.Restricted.Infrastructure.Extensions;

namespace Lloyds.LAF.Agent.Restricted.Infrastructure
{
    internal class CredentialRepository : BaseRepository, ICredentialRepository
    {
        public void AddLafCredentialValue(CredentialValue credentialValue, int sourceApplicationId)
        {
            credentialValue.SourceApplicationId = sourceApplicationId;
            InsertCredentialValue(credentialValue);
        }

        public void AddCredentialValueAndAssociatedUserGroup(CredentialValue credentialValue,
                                                             out UserGroup createdUserGroup)
        {
            UserGroupCredentialValue createdUserGroupCredentialValue;
            InsertCredentialValueAndAssociatedUserGroup(credentialValue, out createdUserGroup, out createdUserGroupCredentialValue);
            credentialValue.AssociatedUserGroup = createdUserGroup;
            credentialValue.AssociatedUserGroupCredentialValue = createdUserGroupCredentialValue;
        }

        public void AddCredentialValueAndAssociatedUserGroupAndDaUserGroup(CredentialValue credentialValue,
                                                                           out UserGroup createdUserGroup,
                                                                           out UserGroup createdDaUserGroup,
                                                                           out CredentialValue
                                                                               createdLafUserGroupCredentialValueForCreatedDaUserGroup)
        {
            AddCredentialValueAndAssociatedUserGroup(credentialValue, out createdUserGroup);
            var daUserGroupText = string.Format("LAF DA_{0} {1}", credentialValue.AssociatedUserGroup.Name, credentialValue.AssociatedUserGroup.Description);

            this.AddDevolvedAdminUserGroupForGivenUserGroup(daUserGroupText,
                                                            daUserGroupText,
                                                            createdUserGroup,
                                                            out createdDaUserGroup,
                                                            out createdLafUserGroupCredentialValueForCreatedDaUserGroup);
        }

        public void AddCredentialValueAndAssociatedUserGroupToExistingDaUserGroup(CredentialValue credentialValue,
                                                                                  string sourceApplicationUrl,
                                                                                  int daUserGroupId,
                                                                                  out UserGroup createdUserGroup,
                                                                                  out CredentialValue
                                                                                      createdLafUserGroupCredentialValueForExistingDaUserGroup)
        {
            UserGroupCredentialValue createdUserGroupCredentialValue;
            var lafUserGroupCredentialId = this.GetIdForLafUserGroupCredential();

            InsertCredentialValueAndAssociatedUserGroup(credentialValue, out createdUserGroup, out createdUserGroupCredentialValue);
            credentialValue.AssociatedUserGroup = createdUserGroup;
            credentialValue.AssociatedUserGroupCredentialValue = createdUserGroupCredentialValue;

            createdLafUserGroupCredentialValueForExistingDaUserGroup = new CredentialValue
                {
                    CredentialId = lafUserGroupCredentialId,
                    SourceApplicationId = createdUserGroup.ApplicationId,
                    Value = createdUserGroup.UserGroupId.ToString(CultureInfo.InvariantCulture),
                    Description = string.Format("LAF {0} LafUserGroup", createdUserGroup.Name)
                };

            InsertCredentialValue(createdLafUserGroupCredentialValueForExistingDaUserGroup);
            var userGroupCredentialValue = new UserGroupCredentialValue
                {
                    CredentialValueId = createdLafUserGroupCredentialValueForExistingDaUserGroup.CredentialValueId,
                    UserGroupId = daUserGroupId
                };

            InsertUserGroupCredentialValue(userGroupCredentialValue);
        }

        public int GetCredentialId(string applicationUrl, string credentialUniformResourceName)
        {
            const string StoredProcedureName = "[LAF].[csp_GetCredentialId]";
            var parameters = new
                                 {
                                     ApplicationUrl = applicationUrl,
                                     CredentialUniformResourceName = credentialUniformResourceName
                                 };

            var foundId = this.RunQuery<int>(StoredProcedureName, parameters).FirstOrDefault();
            return foundId;
        }

        public bool DoesCredentialValueAlreadyExist(int credentialId, string credentialValue)
        {
            const string StoredProcedureName = "[LAF].[csp_DoesCredentialValueAlreadyExist]";
            var parameters = new
                                 {
                                     CredentialId = credentialId,
                                     Value = credentialValue
                                 };

            var doesCredentialValueExist = this.RunQuery<bool>(StoredProcedureName, parameters).FirstOrDefault();
            return doesCredentialValueExist;
        }

        public List<string> GetCredentialValuesForApplication(string applicationUrl, string credentialUniformResourceName)
        {
            const string StoredProcedureName = "[LAF].[csp_GetCredentialValuesForApplication]";
            var parameters = new
                                 {
                                     ApplicationUrl = applicationUrl,
                                     CredentialUniformResourceName = credentialUniformResourceName
                                 };

            var values = this.RunQuery<string>(StoredProcedureName, parameters).ToList();
            return values;
        }

        public List<string> GetCredentialValuesForUser(string applicationUrl, string credentialUniformResourceName, string username)
        {
            const string StoredProcedureName = "[LAF].[csp_GetCredentialValuesForUser]";
            var parameters = new
                                 {
                                     ApplicationUrl = applicationUrl,
                                     CredentialUniformResourceName = credentialUniformResourceName,
                                     Username = username
                                 };

            var values = this.RunQuery<string>(StoredProcedureName, parameters).ToList();
            return values;
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

        public CredentialValue GetCredentialValueById(int id)
        {
            const string StoredProcedureName = "[LAF].[csp_GetCredentialValue_V2]";
            var parameters = new {Id = id};
            var found = this.RunQuery<CredentialValue>(StoredProcedureName, parameters).FirstOrDefault();
            return found;
        }

        public List<CredentialValueAssociation> GetUserGroupsAssociatedToCredentialValues(int[] ids)
        {
            const string StoredProcedureName = "[LAF].[csp_GetUserGroupsAssociatedToCredentialValues]";
            const string ParameterName = "CredentialValueIds";

            var foundUserGroups =
                this.RunQuery<CredentialValueAssociation>(StoredProcedureName, new IntDynamicParameter(ParameterName, ids)).ToList();
            return foundUserGroups;
        }

        //TODO - move these UserGroup methods into their own UserGroup Repository, and use dependency injection to allow the Credential and UserGroup repositories to use each other's methods. 

        // I wish whole-heartedly and unreservedly to apologise to all who may look at this code in times to come, for the haphazard methods, with their inexplicable 
        // mixes of functionality (I give you AddDevolvedAdminUserGroupForGivenUserGroup and AddDevolvedAdminUserGroupForGivenUserGroup by way of example), for the bits that are
        // in the wrong place, and for - well, just for the mess, really. In my defence, this has been written so bittily, so hurriedly, and with so many interruptions
        // that the fact it works at all is a small miracle of which I am, dare I boast it, even a little proud, despite its manifold horrors, because it was written in 
        // adversity, and with project deadlines coming at me from all directions. 
        // Nonetheless, it is what it is, and I throw myself on the mercy of the court. Please try to be understanding. PM 18/09/2013

        

        public void AddDevolvedAdminUserGroupForGivenUserGroup(string name, string description,
                                                               UserGroup userGroupForWhichADevolvedAdminUserGroupIsToBeCreated,
                                                               out UserGroup createdDaUserGroup,
                                                               out CredentialValue createdLafUserGroupCredentialValueForCreatedDaUserGroup)
        {
            var lafUserGroupCredentialId = GetIdForLafUserGroupCredential();

            createdDaUserGroup = new UserGroup {Description = description, Name = name};
            InsertDevolvedAdminUserGroup(createdDaUserGroup);
            InsertDevolvedAdminRoleUserGroupCredentialValue(createdDaUserGroup.UserGroupId);

            createdLafUserGroupCredentialValueForCreatedDaUserGroup = new CredentialValue
                {
                    CredentialId = lafUserGroupCredentialId,
                    SourceApplicationId = userGroupForWhichADevolvedAdminUserGroupIsToBeCreated.ApplicationId,
                    Value = userGroupForWhichADevolvedAdminUserGroupIsToBeCreated.UserGroupId.ToString(CultureInfo.InvariantCulture),
                    Description = string.Format("LAF {0} LafUserGroup", userGroupForWhichADevolvedAdminUserGroupIsToBeCreated.Name)
                };

            InsertCredentialValue(createdLafUserGroupCredentialValueForCreatedDaUserGroup);
            var userGroupCredentialValue = new UserGroupCredentialValue
                {
                    CredentialValueId = createdLafUserGroupCredentialValueForCreatedDaUserGroup.CredentialValueId,
                    UserGroupId = createdDaUserGroup.UserGroupId
                };

            InsertUserGroupCredentialValue(userGroupCredentialValue);
        }

        public List<CredentialValue> SearchForCredentialValues(CredentialValueSearchOptions searchOptions)
        {
            const string StoredProcedureName = "[LAF].[csp_SearchForCredentialValues]";
            var parameters = new
                {
                    searchOptions.ApplicationId,
                    searchOptions.Urn,
                    searchOptions.Value,
                    searchOptions.SourceApplicationId
                };

            var foundItems = this.RunQuery<CredentialValue>(StoredProcedureName, parameters).ToList();
            return foundItems;
        }

        public void Save(CredentialValue value)
        {
            if (value.IsNewEntity())
            {
                this.InsertCredentialValue(value);
            }
            else
            {
                UpdateCredentialValue(value);
            }
        }

        private void UpdateCredentialValue(CredentialValue value)
        {
            const string StoredProcedureName = "[LAF].[csp_UpdateCredentialValue]";
            var parameters = new {value.CredentialValueId, value.Value, value.Description};
            this.RunQuery<int>(StoredProcedureName, parameters);
        }

        public List<CredentialValue> GetManyCredentialValues(string urn, params string[] values)
        {
            const string StoredProcedureName = "[LAF].[csp_GetManyCredentialValues]";
            const string ParameterName = "Values";


            var additionalParameters = new List<SqlParameter>
                {
                    new SqlParameter("Urn", SqlDbType.VarChar, 400) {Value = urn}
                };

            var credentialValues = this.RunQuery<CredentialValue>(StoredProcedureName, new VarDynamicParameter(ParameterName, values, additionalParameters.ToArray())).ToList();
            return credentialValues;
        }

        public void DeleteCredentialValuesAndAssociatedRelationships(params int[] credentialValueIds)
        {
            const string StoredProcedureName = "[LAF].[csp_DeleteCredentialValuesAndAssociatedRelationships]";
            const string ParameterName = "credentialValueIds";
            this.RunQuery<int>(StoredProcedureName, new IntDynamicParameter(ParameterName, credentialValueIds));
        }

        private int GetIdForLafUserGroupCredential()
        {
            const string StoredProcedureName = "[LAF].[csp_GetIdForLafUserGroupCredential]";
            return this.RunQuery<int>(StoredProcedureName, null).FirstOrDefault();
        }

        private void InsertUserGroupWithGeneratedApplicationDetails(UserGroup userGroup, int credentialId)
        {
            const string UserGroupStoredProcedureName = "[LAF].[csp_InsertUserGroupWithGeneratedApplicationDetailsAndOutputsForGeneratedFields]";

            var parameters = new DynamicParameters(new
                {
                    credentialId,
                    userGroup.Name,
                    userGroup.Description,
                }
            );

            parameters.Add("@outputApplicationId", 0, size: null, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@outputName", null, size: 200, dbType: DbType.String, direction: ParameterDirection.Output);

            var foundId = this.RunQuery<int>(UserGroupStoredProcedureName, parameters).FirstOrDefault();
            userGroup.UserGroupId = Convert.ToInt32(foundId);
            userGroup.ApplicationId = parameters.Get<int>("@outputApplicationId");
            userGroup.Name = parameters.Get<string>("@outputName");
        }

        private void InsertCredentialValueAndAssociatedUserGroup(CredentialValue credentialValue, out UserGroup createdUserGroup,
                                                            out UserGroupCredentialValue createdUserGroupCredentialValue)
        {
            InsertCredentialValue(credentialValue);
            createdUserGroup = new UserGroup { Description = credentialValue.Description, Name = credentialValue.Value };
            InsertUserGroupWithGeneratedApplicationDetails(createdUserGroup, credentialValue.CredentialId);
            createdUserGroupCredentialValue = new UserGroupCredentialValue
            {
                CredentialValueId = credentialValue.CredentialValueId,
                UserGroupId = createdUserGroup.UserGroupId
            };

            InsertUserGroupCredentialValue(createdUserGroupCredentialValue);
        }
        
        private void InsertDevolvedAdminUserGroup(UserGroup daUserGroup)
        {
            const string StoredProcedureName = "[LAF].[csp_InsertDAUserGroup]";

            var parameters = new DynamicParameters(new
                {
                   daUserGroup.Name,
                   daUserGroup.Description
                }
            );

            parameters.Add("@outputLAFApplicationId", 0, size: null, dbType: DbType.Int32, direction: ParameterDirection.Output);

            var foundId = this.RunQuery<int>(StoredProcedureName, parameters).FirstOrDefault();
            daUserGroup.ApplicationId = parameters.Get<int>("@outputLAFApplicationId");
            daUserGroup.UserGroupId = Convert.ToInt32(foundId);
        }

        private void InsertCredentialValue(CredentialValue credentialValue)
        {
            const string CredentialValueStoredProcedureName = "[LAF].[csp_InsertCredentialValueWithSourceApplicationId]";

            var sourceApplicationId = credentialValue.SourceApplicationId == 0
                                          ? null
                                          : credentialValue.SourceApplicationId;

            var parameters = new DynamicParameters(new
            {
                credentialValue.CredentialId,
                sourceApplicationId,
                credentialValue.Value,
                credentialValue.Description,
            });
            
            var foundId = this.RunQuery<int>(CredentialValueStoredProcedureName, parameters).FirstOrDefault();
            credentialValue.CredentialValueId = Convert.ToInt32(foundId);
        }

        private int InsertDevolvedAdminRoleUserGroupCredentialValue(int daUserGroupId)
        {
            const string StoredProcedureName = "[LAF].[csp_InsertDevolvedAdminRoleUserGroupCredentialValue]";

            var parameters = new DynamicParameters(new
            {
                DAUserGroupId = daUserGroupId
            });

            var foundId = this.RunQuery<int>(StoredProcedureName, parameters).FirstOrDefault();
            return Convert.ToInt32(foundId);
        }

        private void InsertUserGroupCredentialValue(UserGroupCredentialValue value)
        {
            const string RelationshipStoredProcedureName = "[LAF].[csp_InsertUserGroupCredentialValueWithExpiryDate]";

            var parameters = new DynamicParameters(new
            {
                userGroupId = value.UserGroupId,
                credentialValueId = value.CredentialValueId,
                value.ExpiryDate
            }
            );

            var foundId = this.RunQuery<int>(RelationshipStoredProcedureName, parameters).FirstOrDefault();
            value.UserGroupCredentialValueId = Convert.ToInt32(foundId);
        }
    }
}