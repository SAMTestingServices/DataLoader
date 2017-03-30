using System;

namespace Lloyds.LAF.Agent.Restricted.Domain
{
    public class UserGroupCredentialValue
    {
        public int UserGroupCredentialValueId { get; internal set; }

        public int CredentialValueId { get; set; }

        public int UserGroupId { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}