using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAFBulkDataLoader
{
    interface IInputData
    {
        string Status { get; set; }

        string UserGroupName { get; set; }

        string Value { get; set; }
        string Description { get; set; }

        int UserGroupID { get; set; }

        int CredentialValueID { get; set; }
    }
}
