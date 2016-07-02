using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Niqiu.Core.Domain;

namespace Niqiu.Core.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string url, int length)
        {
            var tagA = new TagBuilder("a");
            tagA.MergeAttribute("href", url);
            tagA.MergeAttribute("target", "_blank");

            var img = new TagBuilder("img");
            img.MergeAttribute("src", url);
            img.MergeAttribute("style", string.Format("width:{0}px", length));

            tagA.InnerHtml = img.ToString();

            //return string.Format("<a href='{0}' target='_blank'> <img src='{0}' style='width:{1}px;'/></a>", url, length);
            return MvcHtmlString.Create(tagA.ToString());
        }

        public static MvcHtmlString EnumToDropDownList(this HtmlHelper helper, Enum eEnum,string name)
        {
            var selectList = new List<SelectListItem>();
            var enumType = eEnum.GetType();
            foreach (var value in Enum.GetValues(enumType))
            {
                var field = enumType.GetField(value.ToString());
                var option = new SelectListItem() { Value = value.ToString() };
                var display = field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                option.Text = display != null ? display.Name : value.ToString();
                option.Selected = Equals(value, eEnum);
                selectList.Add(option);
            }
            return helper.DropDownList(name, selectList);
        }

        public static string EnumToText(this HtmlHelper helper, Enum eEnum)
        {
            var enumType = eEnum.GetType();
            var field = enumType.GetField(eEnum.ToString());
            var display = field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            return display != null ? display.Name : eEnum.ToString();
        }

        public static string EnumToTextFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var enumType = modelMetadata.ModelType;
            var field = enumType.GetField(modelMetadata.Model.ToString());
            var display = field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            return display != null ? display.Name : modelMetadata.Model.ToString();
        }

        public static MvcHtmlString EnumToDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes=null)
        {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlAttributes2 = AnonymousObjectToHtmlAttributes(htmlAttributes);
            var enumType = modelMetadata.ModelType;
            var selectList = new List<SelectListItem>();
            foreach (var value in Enum.GetValues(enumType))
            {
                var field = enumType.GetField(value.ToString());
                var option = new SelectListItem() { Value = value.ToString() };
                var display = field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                option.Text = display != null ? display.Name : value.ToString();
                option.Selected = Equals(value, modelMetadata.Model);
                selectList.Add(option);
            }
            return html.DropDownList(modelMetadata.PropertyName, selectList,htmlAttributes2);
        }

        public static MvcHtmlString A(this HtmlHelper helper, string text, string url, int id)
        {
            var tagA = new TagBuilder("a");
            tagA.MergeAttribute("href", url);
            tagA.MergeAttribute("data-id", id.ToString());
            tagA.InnerHtml = text;
            return MvcHtmlString.Create(tagA.ToString());
        }


        internal static MvcHtmlString ImageHelper(HtmlHelper html, ModelMetadata metadata, IDictionary<string, object> htmlAttributes = null)
        {
            //属性值
            var value = metadata.Model.ToString();

            if (string.IsNullOrEmpty(value))
            {
                return MvcHtmlString.Empty;
            }
            var img = new TagBuilder("img");
            img.Attributes.Add("src", value);
            //属性名
            img.Attributes.Add("id", metadata.PropertyName);
            img.MergeAttributes(htmlAttributes, true);

            var tagA = new TagBuilder("a");
            tagA.MergeAttribute("href",value);
            tagA.MergeAttribute("target", "_blank");
            tagA.InnerHtml = img.ToString();
            return MvcHtmlString.Create(tagA.ToString());

        }

        public static MvcHtmlString ImageFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
           // var propertyname = ExpressionHelper.GetExpressionText(expression);
            var htmlAttributes2 = AnonymousObjectToHtmlAttributes(htmlAttributes);
            return ImageHelper(html, modelMetadata , htmlAttributes2);
        }

        public static MvcHtmlString ImageFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression)
        {
            return ImageFor(html, expression, null);
        }

        private static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    routeValueDictionary.Add(propertyDescriptor.Name.Replace('_', '-'), propertyDescriptor.GetValue(htmlAttributes));
                }
            }

            return routeValueDictionary;
        }
    }

    public class ImageModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType,
            string propertyName)
        {
            var meta= base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            if (meta.DataTypeName==DataType.ImageUrl.ToString() && string.IsNullOrEmpty(meta.TemplateHint))
            {
                meta.TemplateHint = "ImageLink";
            }
            return meta;
        }
    }
}
