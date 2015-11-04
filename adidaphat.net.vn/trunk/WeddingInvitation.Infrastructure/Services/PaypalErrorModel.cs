using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Infrastructure.Services
{
    public class PaypalErrorModel
    {
        public string ErrorCode { get; set; }
        public string Desc { get; set; }
        public string Desc2 { get; set; }
    }
}
