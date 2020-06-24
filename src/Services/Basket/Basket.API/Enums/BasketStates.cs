using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Enums
{
    public enum BasketStates
    {
        PENDING = 1,
        WAITING_FOR_VALIDATION,
        VALIDATED,
        SENT
    }
}
