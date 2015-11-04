using System;
using System.Text;

namespace WeddingInvitation.Infrastructure.Unity
{
    public class StringHelper
    {
        private const string CharsConstant = "Aa9BbCc8Dd7Ee6Ff5Gg4Hh3Ii2Jj1Kk9Ll8Mm7Nn6Oo5Pp4Qq3Rr2Ss1TtUuVvWwXxYyZz";
        /// <summary>
        /// Random String with length
        /// </summary>
        /// <param name="size"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static string RandomString(int size, Random random)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                //26 letters in the alfabet, ascii + 65 for the capital letters
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
            }
            return builder.ToString();
        }

        public static string GenerateRandomString(int size, Random random)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = CharsConstant[random.Next(CharsConstant.Length)];
            }
            return new string(buffer);
        }
        public static string StripHtml(string html, bool allowHarmlessTags)
        {
            if (html == null || html == string.Empty)
                return string.Empty;
            if (allowHarmlessTags)
                return System.Text.RegularExpressions.Regex.Replace(html, "", string.Empty);
            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
        }

    }
}
