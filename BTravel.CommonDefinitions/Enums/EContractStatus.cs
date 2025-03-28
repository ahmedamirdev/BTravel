using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.Enums
{
    public enum EContractStatus
    {
        Pending = 1 , // created and not signed (Open for edit for both)
        Opened = 2 , // Opened and not signed (Open for edit for both)
        Sigend = 3  // Closed for edit for both // Ask Mahmoud 

    }
}