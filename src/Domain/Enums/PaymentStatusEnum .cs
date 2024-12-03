using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum PaymentStatusEnum
    {
        Approved,
        Pending,
        Authorized,
        InProcess,
        InMediation,
        Rejected,
        Cancelled,
        Refunded,
        ChargedBack,
        Unknown 
    }

}


