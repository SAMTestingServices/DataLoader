using System.Collections.Generic;
using Lloyds.LAF.Agent.Restricted.Domain;
using Lloyds.LAF.Agent.Restricted.Domain.Search;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Infrastructure
{
    public interface ICredentialRepository
    {
        void AddCredentialValueAndAssociatedUserGroup(CredentialValue credentialValue, out UserGroup createdUserGroup);

        void AddCredentialValueAndAssociatedUserGroupAndDaUserGroup(CredentialValue credentialValue,
                                                                    out UserGroup createdUserGroup,
                                                                    out UserGroup createdDaUserGroup,
                                                                    out CredentialValue
                                                                        createdLafUserGroupCredentialValueForCreatedDaUserGroup);

        void AddCredentialValueAndAssociatedUserGroupToExistingDaUserGroup(CredentialValue credentialValue,
                                                                           string sourceApplicationUrl,
                                                                           int daUserGroupId,
                                                                           out UserGroup createdUserGroup,
                                                                           out CredentialValue createdLafUserGroupCredentialValueForExistingDaUserGroup);

        int GetCredentialId(string applicationUrl, string credentialUniformResourceName);

        bool DoesCredentialValueAlreadyExist(int credentialId, string credentialValue);

        List<string> GetCredentialValuesForApplication(string applicationUrl, string credentialUniformResourceName);

        List<string> GetCredentialValuesForUser(string applicationUrl, string credentialUniformResourceName, string username);

        void AddDevolvedAdminUserGroupForGivenUserGroup(string name, string description,
                                       UserGroup userGroupForWhichADevolvedAdminUserGroupIsToBeCreated,
                                       out UserGroup createdDaUserGroup,
                                       out CredentialValue createdLafUserGroupCredentialValueForCreatedDaUserGroup);

        List<UserGroupCredentialValue> GetUserGroupCredentialValuesByUserGroupId(int userGroupId);

        List<CredentialValue> SearchForCredentialValues(CredentialValueSearchOptions searchOptions);

        void Save(CredentialValue value);

        CredentialValue GetCredentialValueById(int id);
        
        List<CredentialValueAssociation> GetUserGroupsAssociatedToCredentialValues(int[] ids);

        List<CredentialValue> GetManyCredentialValues(string urn, params string[] values);
        
        void DeleteCredentialValuesAndAssociatedRelationships(params int[] credentialValueIds);
    }
}