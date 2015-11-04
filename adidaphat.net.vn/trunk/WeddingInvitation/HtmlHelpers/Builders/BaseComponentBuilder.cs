using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeddingInvitation.HtmlHelpers.Components;
using System.Web.Mvc;
using System.Web.UI;

namespace WeddingInvitation.HtmlHelpers.Builders
{
    /// <summary>
    /// HUYDO: [Story]
    /// I am a component builder, i need some component to build
    /// I can do:
    /// 1. Render HTML based on what component supplies
    /// 2. Assign/Override a name of the component 
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    public class BaseComponentBuilder<TComponent, TBuilder> where TComponent : BaseComponent where TBuilder : BaseComponentBuilder<TComponent, TBuilder>
    {
        private TComponent _component;
        private System.Web.Mvc.ViewContext _context;

        public BaseComponentBuilder(TComponent component)
        {
            this.Component = component;
        }

        public BaseComponentBuilder(ViewContext context)
        {
            this._context = context;
        }

        public TComponent Component
        {
            get { return _component; }
            private set { _component = value; }
        }

        public ViewContext ViewContext
        {
            get
            {
                return _component.ViewContext;
            }
        }
        
        /// <summary>
        /// Auto generate a name if component does not have any
        /// </summary>
        /// <returns></returns>
        public TBuilder GenerateId()
        {
            if (string.IsNullOrEmpty(Component.Name))
            {
                string prefix = Component.GetType().Name;
                string key = "AUTOGEN_" + prefix;
                int seq = 1;

                if (ViewContext.HttpContext.Items.Contains(key))
                {
                    seq = (int)ViewContext.HttpContext.Items[key] + 1;
                    ViewContext.HttpContext.Items[key] = seq;
                }
                else
                    ViewContext.HttpContext.Items.Add(key, seq);
                Component.Name = prefix + seq.ToString();
            }

            return this as TBuilder;
        }

        public virtual TBuilder Name(string name)
        {
            Component.Name = name;
            return this as TBuilder;
        }

        public virtual void Render()
        {
            RenderComponent();
        }

        protected void RenderComponent()
        {
            using (var writer = new HtmlTextWriter(ViewContext.Writer))
            {
                Component.Render(writer);
            }
        }

        public virtual MvcHtmlString GetHtml()
        {
            return MvcHtmlString.Create(Component.GetHtml());
        }
    }
}