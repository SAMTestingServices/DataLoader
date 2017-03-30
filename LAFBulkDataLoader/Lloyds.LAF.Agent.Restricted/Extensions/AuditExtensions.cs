using System.Xml.Linq;
using Lloyds.LAF.Agent.Restricted.Domain;
using Lloyds.LAF.Audit;

namespace Lloyds.LAF.Agent.Restricted.Extensions
{
    internal static class AuditExtensions
    {
        internal static AuditInformation WithCredentialValues(this AuditInformation information,
                                                              params CredentialValue[] credentialValues)
        {
            var xCredentialValues = new XElement("credentialvalues");
            foreach (var credentialValue in credentialValues)
            {
                var xCredentialValue = new XElement("credentialvalue");
                xCredentialValue.SetAttributeValue("id", credentialValue.CredentialValueId);
                xCredentialValue.SetAttributeValue("credentialid", credentialValue.CredentialId);
                xCredentialValue.SetAttributeValue("description", credentialValue.Description);
                xCredentialValue.SetAttributeValue("value", credentialValue.Value);
                xCredentialValues.Add(xCredentialValue);
            }

            information.Information.Add(xCredentialValues);
            return information;
        }

        internal static AuditInformation WithUserGroup(this AuditInformation information, UserGroup userGroup)
        {
            if (userGroup == null)
            {
                return information;
            }

            var xUserGroup = new XElement("usergroup");
            xUserGroup.SetAttributeValue("id", userGroup.UserGroupId);
            xUserGroup.SetAttributeValue("name", userGroup.Name);
            xUserGroup.SetAttributeValue("applicationid", userGroup.ApplicationId);
            information.Information.Add(xUserGroup);
            return information;
        }

        internal static AuditInformation WithUserGroupCredentialValues(this AuditInformation information, params UserGroupCredentialValue[] values)
        {
            var xParent = new XElement("usergroupcredentialvalues");
            foreach (var userGroupCredentialValue in values)
            {
                if (userGroupCredentialValue == null)
                {
                    continue;
                }

                var xUserGroupCredentialValue = new XElement("usergroupcredentialvalue");
                xUserGroupCredentialValue.SetAttributeValue("id", userGroupCredentialValue.UserGroupCredentialValueId);
                xUserGroupCredentialValue.SetAttributeValue("credentialvalueid", userGroupCredentialValue.CredentialValueId);
                xUserGroupCredentialValue.SetAttributeValue("usergroupid", userGroupCredentialValue.UserGroupId);
                xParent.Add(xUserGroupCredentialValue);
            }

            information.Information.Add(xParent);
            return information;
        }
    }
}