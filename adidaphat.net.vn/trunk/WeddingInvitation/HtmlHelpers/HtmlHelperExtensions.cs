using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using WeddingInvitation.HtmlHelpers.Factories;
using WeddingInvitation.HtmlHelpers.Builders;

namespace WeddingInvitation.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static Factory Osd<TModel>(this HtmlHelper<TModel> helper)
        {
            return new Factory<TModel>(helper);
        }

        public static Factory Osd(this HtmlHelper helper)
        {
            return new Factory(helper);
        }

        public static MvcHtmlString RenderPartial<TModel>(this HtmlHelper<TModel> helper, string parialName, ViewDataDictionary viewData, object attachment)
        {
            viewData.Add("Attachment", attachment);
            return helper.Partial(parialName, viewData);
        }

        public static MvcHtmlString RenderPartial(this HtmlHelper helper, string parialName, ViewDataDictionary viewData, object attachment)
        {
            viewData.Add("Attachment", attachment);
            return helper.Partial(parialName, viewData);
        }
        //TODO:
        //Follow the component/builder/factory parttern for this

        //#region EnumDropDownListFor Extensions

        //private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        //{
        //    Type realModelType = modelMetadata.ModelType;

        //    Type underlyingType = Nullable.GetUnderlyingType(realModelType);
        //    if (underlyingType != null)
        //    {
        //        realModelType = underlyingType;
        //    }
        //    return realModelType;
        //}

        //private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };

        //public static string GetEnumDescription<TEnum>(TEnum value)
        //{
        //    FieldInfo fi = value.GetType().GetField(value.ToString());

        //    var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        //    if ((attributes.Length > 0))
        //        return attributes[0].Description;
        //    else
        //        return value.ToString();
        //}

        ///// <summary>
        ///// HUYDO:
        ///// Copied from http://stackoverflow.com/questions/388483/how-do-you-create-a-dropdownlist-from-an-enum-in-asp-net-mvc (thanks to Simon goldstone)
        ///// The key idea is to make an extesion for Enum (which ardorned with an [System.ComponentModel.Description] attribute)
        ///// </summary>
        //public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        //{
        //    return EnumDropDownListFor(htmlHelper, expression, null);
        //}

        ///// <summary>
        ///// HUYDO:
        ///// Copied from http://stackoverflow.com/questions/388483/how-do-you-create-a-dropdownlist-from-an-enum-in-asp-net-mvc (thanks to Simon goldstone)
        ///// The key idea is to make an extesion for Enum (which ardorned with an [System.ComponentModel.Description] attribute)
        ///// </summary>
        //public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        //{
        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    Type enumType = GetNonNullableModelType(metadata);
        //    IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

        //    IEnumerable<SelectListItem> items = from value in values
        //                                        select new SelectListItem
        //                                        {
        //                                            Text = GetEnumDescription(value),
        //                                            Value = value.ToString(),
        //                                            Selected = value.Equals(metadata.Model)
        //                                        };

        //    // If the enum is nullable, add an 'empty' item to the collection
        //    if (metadata.IsNullableValueType)
        //        items = SingleEmptyItem.Concat(items);

        //    return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        //}

        //#endregion

    }
}