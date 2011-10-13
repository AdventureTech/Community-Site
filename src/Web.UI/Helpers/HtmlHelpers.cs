using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CommunitySite.Web.UI.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString MenuItem(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var listItem = new TagBuilder("li");
    
            if (actionName == currentAction && controllerName == currentController)
                listItem.Attributes.Add("id", "current");
            
            listItem.InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString();

            return htmlHelper.Raw(listItem.ToString());
        }
    }
}