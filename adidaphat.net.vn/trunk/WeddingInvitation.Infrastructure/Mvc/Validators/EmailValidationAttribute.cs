using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WeddingInvitation.Infrastructure.Mvc.Validators
{
    public class EmailValidationAttribute : RegularExpressionAttribute
    {
        public EmailValidationAttribute()
            : base(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
        {
            ErrorMessage = "Invalid email";
        }

        static EmailValidationAttribute()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailValidationAttribute), typeof(RegularExpressionAttributeAdapter));
        }
    }
}
