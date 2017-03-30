using System.Collections.Generic;
using Lloyds.LAF.Agent.Restricted.Domain;
using Lloyds.LAF.Agent.Restricted.Domain.Search;

namespace Lloyds.LAF.Agent.Restricted.Contracts.Tasks
{
    public interface ICredentialTasks : IObsoleteCredentialTasks
    {
        void AddCredentialValueAndAssociatedUserGroup(string applicationUrl, string credentialElementNid, string value, string description = null);

        void AddCredentialValueAndAssociatedUserGroupAndDaUserGroup(string applicationUrl,
                                                                    string credentialUrnName,
                                                                    string credentialValueValue,
                                                                    string credentialValueDescription,
                                                                    out CredentialValue createdCredentialValue,
                                                                    out UserGroup createdUserGroup,
                                                                    out UserGroup createdDaUserGroup);

        void AddCredentialValueAndAssociatedUserGroupToExistingDaUserGroup(string applicationUrl,
                                                                           string credentialUrnName,
                                                                           string credentialValueValue,
                                                                           string credentialValueDescription,
                                                                           int daUserGroupId,
                                                                           out CredentialValue createdCredentialValue,
                                                                           out UserGroup createdUserGroup);

        void AddDevolvedAdminUserGroupForGivenUserGroup(string name, string description,
                                               int idOfUserGroupForWhichADevolvedAdminUserGroupIsToBeCreated,
                                               out UserGroup createdDaUserGroup);

        void AddCredentialValue(CredentialValue credentialValue);

        CredentialValue AddLafCredentialValue(string lafApplicationUrl, string sourceApplicationUrl,
                                              string credentialUrnName, string credentialValueValue,
                                              string credentialValueDescription);
        
        CredentialValue AddCredentialValue(string applicationUrl,
                                string credentialUrnName,
                                string credentialValueValue,
                                string credentialValueDescription);
 
        List<string> GetCredentialValuesForApplication(string applicationUrl, string credentialElementNid);

        List<string> GetCredentialValuesForUser(string applicationUrl, string credentialElementNid, string username);

        List<CredentialValue> SearchForCredentialValues(CredentialValueSearchOptions searchOptions);

        void Save(CredentialValue value);

        CredentialValue GetCredentialValueById(int id);

        Dictionary<int, List<int>> GetUserGroupsAssociatedToCredentialValues(int[] ids);

        List<CredentialValue> GetManyCredentialValues(string urn, params string[] values);

        void DeleteCredentialValuesAndAssociatedRelationships(params int[] credentialValueIds);
    }
}