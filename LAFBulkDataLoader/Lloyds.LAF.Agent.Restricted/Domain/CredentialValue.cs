namespace Lloyds.LAF.Agent.Restricted.Domain
{
    public class CredentialValue
    {
        public string Urn { get; set; }

        public int ApplicationId { get; set; }

        public int CredentialValueId { get; set; }

        public int CredentialId { get; set; }

        public int? SourceApplicationId { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public UserGroup AssociatedUserGroup { get; set; }

        public UserGroupCredentialValue AssociatedUserGroupCredentialValue { get; set; }
    }
}