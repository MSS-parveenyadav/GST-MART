using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST_BLL.Enum
{
    public static class EnumClass
    {
        public enum ImportFamily
        {
            Company,
            Ledger,
            Supply,
            Purchase,
            Footer
        
        }

        public enum MessageFamily
        {
            Success,
            Error
        }

        public enum schedularStatus
        { 
            Running,
            Pause
        
        }

        public enum AuditFamily
        {
            Success,
            Error
        }

    }
}
