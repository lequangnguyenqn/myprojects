using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Infrastructure.Security
{
    public class Escaping
    {
        /// <summary>
        /// Encode
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static string ToBase64String(string sInput)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(sInput));
        }

        /// <summary>
        /// Decode
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public static string FromBase64String(string sInput)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(sInput));
        }

    }
}
