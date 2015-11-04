using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Core.Models.Orders
{
    public enum OrderStatus
    {
        Imported = 1,
        ReadyInStorage = 5,
        Printing = 10,
        Printed = 15,
        Packaged = 20,
        DeliveryInTheDay = 25,
        ReadyInStateprovince = 30,
        BeDelivered = 35
    }
}
