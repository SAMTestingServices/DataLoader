using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    class LAFUserData
    {
       
        public Guid Uid { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public bool isActive { get; set; }
        public bool isDisabled { get; set; }
        public int userID { get; set; }






    }
}
