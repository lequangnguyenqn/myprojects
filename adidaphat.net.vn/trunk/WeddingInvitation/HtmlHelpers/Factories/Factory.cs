using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeddingInvitation.HtmlHelpers.Builders;
using WeddingInvitation.HtmlHelpers.Components;
using Telerik.Web.Mvc.UI.Fluent;
using Telerik.Web.Mvc.UI;

namespace WeddingInvitation.HtmlHelpers.Factories
{
    public class Factory
    {
        protected readonly HtmlHelper Helper;
        public Factory(HtmlHelper helper)
        {
            Helper = helper;
        }

        /// <summary>
        /// Usage:
        /// 
        /// 
        /// Osd().Panel().Title("Panel title").
        /// Body(
        ///     @<text><p>Panel content.</p>
        ///     </text>).Render(); 
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public PanelComponentBuilder Panel()
        {
            return new PanelComponentBuilder(new PanelComponent(Helper.ViewContext)).GenerateId();
        }

        public WindowBuilder HiddenPopupWindow(string title, string html)
        {
            return Helper.Telerik().Window()
                     .Name("tMessageBox")
                     .Title(title)
                     .Draggable(true)
                     .Modal(true)
                     .Visible(false)
                     .Width(250)
                     .Height(100)
                     .Effects(fx => fx
                                   .Zoom()
                                   .Opacity()
                                   .OpenDuration(300)
                                   .CloseDuration(300)
                    )
                    .Content(html);
        }

        public WindowBuilder HiddenPopupWindow(string title, Func<object, System.Web.WebPages.HelperResult> func)
        {
            return Helper.Telerik().Window()
                     .Name("tMessageBox")
                     .Title(title)
                     .Draggable(true)
                     .Modal(true)
                     .Visible(false)
                     .Width(250)
                     .Height(100)
                     .Effects(fx => fx
                                   .Zoom()
                                   .Opacity()
                                   .OpenDuration(300)
                                   .CloseDuration(300)
                    )
                    .Content(func(null).ToHtmlString());
        }

        public WindowBuilder HiddenPopupWindow(string name, string title, int width, int height, Func<object, System.Web.WebPages.HelperResult> func)
        {
            return Helper.Telerik().Window()
                .Name(name)
                .Effects(fx => fx
                                   .Zoom()
                                   .Opacity()
                                   .OpenDuration(300)
                                   .CloseDuration(300)
                )
                .Title(title)
                .Draggable(true)
                .Resizable(resizing => resizing.Enabled(false))
                .Modal(true)
                .Width(width)
                .Visible(false)
                .Height(height)
                .Content(func(null).ToHtmlString());
        }

        public WindowBuilder HiddenPopupWindow(string name, string title, string onClose, int width, int height, Func<object, System.Web.WebPages.HelperResult> func)
        {
            return Helper.Telerik().Window()
                .Name(name)
                .ClientEvents(
                    events =>
                    events.OnClose(onClose))
                .Effects(fx => fx
                                   .Zoom()
                                   .Opacity()
                                   .OpenDuration(300)
                                   .CloseDuration(300)
                )
                .Title(title)
                .Draggable(true)
                .Resizable(resizing => resizing.Enabled(false))
                .Modal(true)                
                .Width(width)
                .Visible(false)
                .Height(height)
                .Content(func(null).ToHtmlString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="onClose"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public WindowBuilder HiddenPopupWindow(string name, string title, string onClose, int width, int height)
        {
            return Helper.Telerik().Window()
                .Name(name)
                .ClientEvents(
                    events =>
                    events.OnClose(onClose))
                .Effects(fx => fx
                                   .Zoom()
                                   .Opacity()
                                   .OpenDuration(300)
                                   .CloseDuration(300)
                )
                .Title(title)
                .Draggable(true)
                .Resizable(resizing => resizing.Enabled(false))
                .Modal(true)
                .Buttons(b => b.Maximize().Close())
                .Width(width)
                .Visible(false)
                .Height(height);
        }

        public WindowBuilder HiddenPopupWindow(string name, string title, string onOpen, string onClose, int width, int height, Func<object, System.Web.WebPages.HelperResult> func)
        {
            return Helper.Telerik().Window()
                .Name(name)
                .ClientEvents(
                    events =>
                    events.OnOpen(onOpen).OnClose(onClose))
                .Effects(fx => fx
                                   .Zoom()
                                   .Opacity()
                                   .OpenDuration(300)
                                   .CloseDuration(300)
                )
                .Title(title)
                .Draggable(true)
                .Resizable(resizing => resizing.Enabled(false))
                .Modal(true)
                .Buttons(b => b.Maximize().Close())
                .Width(width)
                .Visible(false)
                .Height(height)
                .Content(func(null).ToHtmlString());
        }

        /// OYVINDF:
        /// Make the grid for convenient binding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public GridBuilder<T> Grid<T>(IEnumerable<T> model, string name, string action, string controllerName, object routeValues) where T : class
        {
            return Helper.Telerik().Grid(model)
                .Name(name)
                .Localizable("vi-VN")
                .DataBinding(dataBinding => dataBinding.Ajax().Select(action, controllerName))
                .Sortable(sorting => sorting.Enabled(true))
                .Scrollable(scrolling => scrolling.Enabled(false))
                .Filterable(filtering => filtering.Enabled(true))
                .Pageable(paging => paging.Enabled(true))
                .Groupable(grouping => grouping.Enabled(false))
                .Selectable(selecting => selecting.Enabled(true))
                .ClientEvents(events => events.OnDataBinding("onDataBinding"))
                .EnableCustomBinding(true)
                .Footer(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public GridBuilder<T> Grid<T>(IEnumerable<T> model, string name, string controllerName) where T : class
        {
            return Helper.Telerik().Grid(model)
                .Name(name)
                .Localizable("vi-VN")
                .DataBinding(dataBinding => dataBinding.Ajax().Select("GridModel", controllerName))
                .Sortable(sorting => sorting.Enabled(true))
                .Scrollable(scrolling => scrolling.Enabled(false))
                .Filterable(filtering => filtering.Enabled(true))
                .Pageable(paging => paging.Enabled(true))
                .Groupable(grouping => grouping.Enabled(false))
                .Selectable(selecting => selecting.Enabled(true))
                .ClientEvents(events => events.OnDataBinding("onDataBinding"))
                .EnableCustomBinding(true)
                .Footer(true);
        }

        public UploadBuilder GetUploadComponent(string name, string controller, string action, string onUploadCallback)
        {
            return Helper.Telerik().Upload().Name(name).Multiple(false)
                                              .ShowFileList(false)
                                              .Async(async => async
                                                  .Save(@action, @controller)
                                                  .AutoUpload(true))
                                              .ClientEvents(events => events
                                                  .OnSuccess(onUploadCallback));
        }
    }


    public class Factory<TModel> : Factory
    {
        public Factory(HtmlHelper helper) : base(helper) { }
    }
}