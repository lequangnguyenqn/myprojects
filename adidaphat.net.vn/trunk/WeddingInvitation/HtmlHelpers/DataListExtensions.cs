using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace WeddingInvitation.Extensions
{
    public static class DataListExtensions
    {
        public static IHtmlString DataList<T>(this HtmlHelper helper, IEnumerable<T> items, int columns,
            Func<T, HelperResult> template) 
            where T : class
        {
            if (items == null)
                return new HtmlString("");
            bool hasItem = false;
            var sb = new StringBuilder();
            //TODO support custom attributes
            sb.Append("<table class=\"product-list-content-table\" width=\"100%\">");

            int cellIndex = 0;

            foreach (T item in items)
            {
                hasItem = true;
                if (cellIndex == 0)
                    sb.Append("<tr>");

                sb.Append("<td");
                sb.Append(">");
                
                sb.Append(template(item).ToHtmlString());
                sb.Append("</td>");

                cellIndex++;

                if (cellIndex == columns)
                {
                    cellIndex = 0;
                    sb.Append("</tr>");
                }
            }

            if (cellIndex != 0)
            {
                for (; cellIndex < columns; cellIndex++)
                {
                    sb.Append("<td>&nbsp;</td>");
                }

                sb.Append("</tr>");
            }

            sb.Append("</table>");
            if (hasItem)
                return new HtmlString(sb.ToString());
            else
                return new HtmlString("");
        }
    }
}
