using System;

namespace Lloyds.LAF.Agent.Restricted.Domain
{
    public class User
    {
        public int UserId { get; internal set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string UserName { get { return this.EmailAddress; } }

        public Guid UniqueIdentifier { get; set; }
    }
}