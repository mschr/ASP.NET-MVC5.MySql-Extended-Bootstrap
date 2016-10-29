using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq;

namespace my.ns.client.Helpers
{
    using System.Collections;
    using System.Reflection;
    using System.Text;
    using Translation = Resources.Views.Common;
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified URL to an action method by using
        /// the specified action name, controller name and route values.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteAction(this UrlHelper url,
            string actionName, string controllerName, object routeValues = null)
        {
            string scheme = url.RequestContext.HttpContext.Request.Url.Scheme;

            return url.Action(actionName, controllerName, routeValues, scheme);
        }

        public static HtmlString DisplayForPhone(this HtmlHelper helper, string phone)
        {
            if (phone == null)
            {
                return new HtmlString(string.Empty);
            }
            string formated = phone.Replace(" ", "");
            if (phone.Length == 10)
            {
                formated = string.Format("({0}) {1}-{2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6, 4));
            }
            else if (phone.Length == 7)
            {
                formated = string.Format("{0}-{1}", phone.Substring(0, 3), phone.Substring(3, 4));
            }
            else if (phone.Length == 8)
            {
                formated = string.Format("+45 {0} {1} {2} {3}", phone.Substring(0, 2), phone.Substring(2, 2), phone.Substring(4, 2), phone.Substring(6, 2));
            }
            string s = string.Format("<a href='tel:{0}'>{1}</a>", phone, formated);
            return new HtmlString(s);
        }
        public static HtmlString DisplayForPhone<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            Func<TModel, TValue> deleg = expression.Compile();
            var result = deleg(helper.ViewData.Model);
            return helper.DisplayForPhone(result + "");
        }

        public static HtmlString GlyphIcon(this HtmlHelper helper, string glyphClass, int size = 16)
        {
            if (glyphClass == null)
            {
                return new HtmlString(string.Empty);
            }
            string formatted = string.Format("<span style=\"font-size:{0}px;\" class=\"pull-right hidden-xs showopacity glyphicon glyphicon-{1}\"></span>", size, glyphClass);
            return new HtmlString(formatted);

        }


        public static HtmlString Button(this HtmlHelper helper, string innerHtml, object htmlAttributes)
        {
            return Button(helper, innerHtml, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        public static HtmlString Button(this HtmlHelper helper, string innerHtml, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("button");
            builder.InnerHtml = innerHtml;
            builder.MergeAttributes(htmlAttributes);
            builder.AddCssClass("btn");
            return MvcHtmlString.Create(builder.ToString());
        }

        public static HtmlString ButtonBack(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.BackButtonText, action, routeValues, new { @class = "btn btn-primary" });
        }
        public static HtmlString ButtonCreate(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.CreateButtonText, action, routeValues, new { @class = "btn btn-primary" });
        }
        public static HtmlString ButtonDelete(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.DeleteButtonText, action, routeValues, new { @class = "btn btn-danger" });
        }
        public static HtmlString ButtonEdit(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.EditButtonText, action, routeValues, new { @class = "btn btn-default" });
        }
        public static HtmlString ButtonSave(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.SaveButtonText, action, routeValues, new { @class = "btn btn-default" });
        }
        public static HtmlString ButtonCancel(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.CancelButtonText, action, routeValues, new { @class = "btn btn-default" });
        }
        public static HtmlString ButtonSearch(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.SearchButtonText, action, routeValues, new { @class = "btn btn-default" });
        }
        public static HtmlString ButtonReset(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.ResetButtonText, action, routeValues, new { @class = "btn btn-default" });
        }
        public static HtmlString ButtonSubmit(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.SubmitButtonText, action, routeValues, new { @class = "btn btn-default" });
        }
        public static HtmlString ButtonLogin(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.LoginButtonText, action, routeValues, new { @class = "btn btn-default" });
        }
        public static HtmlString ButtonRegister(this HtmlHelper helper, string action, object routeValues = null)
        {
            return helper.ActionLink(Translation.ViewTranslation.RegisterButtonText, action, routeValues, new { @class = "btn btn-default" });
        }
    }
}