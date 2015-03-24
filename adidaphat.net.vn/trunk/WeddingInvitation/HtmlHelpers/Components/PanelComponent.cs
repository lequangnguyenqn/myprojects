using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeddingInvitation.HtmlHelpers.Components
{
    /// <summary>
    /// HUYDO:
    /// this is a div block, create for fun
    /// Display:
    /// 
    /// <div>
    /// <div>[Title]</div>
    /// [Template]
    /// </div>
    /// 
    /// [Template] maybe some block of HTML, see it's Factory for the usage
    /// It looks like this, eg:
    /// 
    /// Panel().Title("Panel title").
    /// Body(
    /// @<text><p>Panel content.</p>
    /// </text>
    /// ).Render();   
    /// 
    /// </summary>
    public class PanelComponent : BaseComponent
    {
        public IHtmlTemplate Content { get; set; }

        public PanelComponent(ViewContext context): base(context) 
        {
            Content = new MvcHtmlTemplate(); 
        }
        
        public string Title { get; set; }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("div");
            writer.Write(Title);
            writer.WriteEndTag("div");
            //write content if we have some template
            if (!Content.IsEmpty)
                Content.WriteTo(writer);
        }
    }
}