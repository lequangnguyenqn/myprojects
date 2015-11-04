using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeddingInvitation.HtmlHelpers.Components;

namespace WeddingInvitation.HtmlHelpers.Builders
{
    public class PanelComponentBuilder : BaseComponentBuilder<PanelComponent, PanelComponentBuilder>
    {
        public PanelComponentBuilder(PanelComponent component) : base(component) { }

        public PanelComponentBuilder Title(string title)
        {
            Component.Title = title;
            return this;
        }

        public PanelComponentBuilder Body(Action body)
        {
            if (body != null)
                Component.Content.Content = body;
            return this;
        }

        public PanelComponentBuilder Body(Func<object, object> body)
        {
            if (body != null)
                Component.Content.InlineContent = body;
            return this;
        }

    }
}