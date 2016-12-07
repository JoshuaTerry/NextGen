using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.DataModels
{
    public class Enums
    {
        public enum PaymentMethod { None = 0, Check = 1, ACH = 2, Wire = 3, SWIFT = 4 }
    }
}