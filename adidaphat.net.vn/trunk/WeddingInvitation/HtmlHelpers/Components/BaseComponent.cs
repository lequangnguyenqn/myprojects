using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Text;
using System.IO;

namespace WeddingInvitation.HtmlHelpers.Components
{
    /// <summary>
    /// HUYDO: [Story]
    /// This is a component, it is waiting for someone to buid
    /// This component has:
    /// 1. A name
    /// 2. Buit-in HTML render method
    /// </summary>
    public abstract class BaseComponent
    {
        private ViewContext _viewContext;
        private string _id;

        public ViewContext ViewContext
        {
            get { return _viewContext; }
        }

        public BaseComponent(ViewContext viewContext)
        {
            this._viewContext = viewContext;
        }

        public virtual string Name { get; set; }

        public virtual string ClientId
        {
            get
            {
                if ((string.IsNullOrEmpty(_id)) && (!string.IsNullOrEmpty(Name)))
                {
                    var tag = new TagBuilder(TagName);
                    tag.GenerateId(Name);
                    _id = tag.Attributes.ContainsKey("id") ? tag.Attributes["id"] : Name;
                }
                return _id;
            }
            private set { _id = value; }
        }

        public virtual string TagName
        {
            get
            {
                return "div";
            }
        }

        public virtual void Render(HtmlTextWriter writer)
        {
            RenderBeginContent(writer);

            RenderContent(writer);

            RenderEndTag(writer);
        }

        public virtual void RenderBeginContent(HtmlTextWriter writer)
        {
            var tag = new TagBuilder(TagName);

            if (!string.IsNullOrEmpty(Name))
            {
                tag.GenerateId(Name);
                if (!tag.Attributes.ContainsKey("id"))
                    tag.MergeAttribute("id", Name);

                _id = tag.Attributes["id"];

                if (TagName.Equals("input", StringComparison.OrdinalIgnoreCase) || TagName.Equals("textarea", StringComparison.OrdinalIgnoreCase))
                    tag.MergeAttribute("name", Name);
            }

            writer.Write(tag.ToString(TagRenderMode.StartTag));
        }

        public virtual void RenderContent(HtmlTextWriter writer) { }

        public virtual void RenderEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag(TagName);
        }

        public string GetHtml()
        {
            var result = new StringBuilder();
            using (var writer = new HtmlTextWriter(new StringWriter(result)))
            {
                Render(writer);
            }
            return result.ToString();
        }
    }

}