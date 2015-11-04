using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.Customers
{
    public class CustomerLogo
    {
        public int CustomerLogoId { get; set; }
        [StringLength(128)]
        public string CustomerName { get; set; }
        [StringLength(128)]
        public string LogoUrl { get; set; }
        [StringLength(128)]
        public string HomePageUrl { get; set; }
    }
}
