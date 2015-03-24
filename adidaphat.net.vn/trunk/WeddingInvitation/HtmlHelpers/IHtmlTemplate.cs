using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace WeddingInvitation.HtmlHelpers
{
    public interface IHtmlTemplate
    {
        Action Content { get; set; }

        Func<object, object> InlineContent { get; set; }

        bool IsEmpty { get; }

        void WriteTo(HtmlTextWriter writer);
    }

    public class MvcHtmlTemplate : IHtmlTemplate
    {
        public Action Content { get; set; }

        public Func<object, object> InlineContent { get; set; }

        public void WriteTo(HtmlTextWriter writer)
        {
            if (Content != null) Content.Invoke();
            else
            {
                if (InlineContent != null) writer.Write(InlineContent(null).ToString());
            }
        }

        public bool IsEmpty
        {
            get
            {
                return ((Content == null) && (InlineContent == null));
            }
        }
    }
}